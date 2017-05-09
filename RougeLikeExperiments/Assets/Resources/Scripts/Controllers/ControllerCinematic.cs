using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
 
/***Singleton***/
public class ControllerCinematic : MonoBehaviour {
	public static ControllerCinematic controllerCinematic;

	/***Speakers***/
	public delegate void Status (Message message);
	public static event Status EventCinematic;

	/***Listeners***/
	void EventChangeScene (Message message){
		messageCurrentScene = message;
	}
	void EventForCinematic (Message message){
		messageForCinematic = message;
	}

	/***Messages***/
	public Message messageCurrentScene = new Message();
	public Message messageForCinematic = new Message();

	/***Variables***/
	bool isSplashStarted = false;
	VideoPlayer currentVideo;
	VideoClip screenSplash;
	VideoClip screenTitle;

	/***Monobehaviour functions***/
	void Awake () {
		//This is the only controllerCinematic on a GameObject
		if (controllerCinematic == null) {
			DontDestroyOnLoad (gameObject);
			controllerCinematic = this;
		} 

		//This is NOT the only controllerCinematic on a GameObject
		else {
			Destroy (gameObject);
		}
	}
	void Start (){
		//Set Components and Screens
		currentVideo = gameObject.GetComponent <VideoPlayer>();
		currentVideo.SetTargetAudioSource (0, gameObject.GetComponent<AudioSource> ());
		currentVideo.targetCamera = Camera.main;
		screenSplash = (VideoClip) Resources.Load("Movies/SplashScreen");
	}
	void OnEnable (){
		//Enable Listeners for events
		ControllerGame.EventChangeScene += EventChangeScene;
		ControllerGame.EventForCinematic += EventForCinematic;
	}
	void OnDisable (){
		//Disable Listeners for events
		ControllerGame.EventChangeScene -= EventChangeScene;
		ControllerGame.EventForCinematic -= EventForCinematic;
	}
	void Update (){
		//The Scene has changed
		if (messageCurrentScene.scene.ToString () != "") {
			//Changed to SCENE_ProgramLaunched
			if (messageCurrentScene.scene.ToString () == "SCENE_ProgramLaunched") {
				if (isSplashStarted == false) {
					currentVideo.clip = screenSplash;
					isSplashStarted = true;
					currentVideo.Play ();

					messageCurrentScene = new Message ();
				} else if (isSplashStarted == true) {
					EventCinematic (new Message ("ERROR", "Cinematic manager received a notification that the scene changed to SCENE_ProgramLaunched. Splash Screen was already active."));
				}
			}
			//Some other stuff probably
			//Default: don't do anything
		}

		//ControllerGame has sent a direct message
		if (messageForCinematic.scene.ToString () != "") {

		}


	}
}
