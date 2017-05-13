using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***Singleton***/
public class ControllerBuilder : MonoBehaviour {
	public static ControllerBuilder controllerBuilder;

	/***Speakers***/
	public static event Status EventBuilder;
	public delegate void Status (Message message);

	/***Listeners***/
	void EventChangeScene (Message message){
		messageCurrentScene = message;
	}
	void EventForBuilder (Message message) {
		messageForBuilder = message;
	}

	/***Messages***/
	public Message messageCurrentScene = new Message();
	public Message messageForBuilder = new Message ();

	/***Variables***/
	bool wasProgramAlreadyLaunched = false;
	float minTimeForSplash = 1f;
	string currentScene = "";
	string currentData = "";

	/***Monobehaviour functions***/
	void Awake () {
		//This is the only controllerBuilder on a GameObject
		if (controllerBuilder == null) {
			DontDestroyOnLoad (gameObject);
			controllerBuilder = this;
		} 

		//This is NOT the only controllerBuilder on a GameObject
		else {
			Destroy (gameObject);
		}
	}
	void OnEnable (){
		//Enable Listeners for events
		ControllerGame.EventChangeScene += EventChangeScene;
	}
	void OnDisable (){
		//Disable Listeners for events
		ControllerGame.EventChangeScene -= EventChangeScene;
	}
	void Update (){
		if (messageCurrentScene.scene.ToString () != "") {
			currentScene = messageCurrentScene.scene.ToString ();
			if (currentScene == "SCENE_ProgramLaunched" && wasProgramAlreadyLaunched == true) {
				EventBuilder (new Message ("ERROR", "ControllerBuilder -> Update -> messageCurrentScene -> scene is not empty: Duplicate SCENE_ProgramLaunched message."));
			}
		}

		if (currentScene == "SCENE_ProgramLaunched") {
			if (wasProgramAlreadyLaunched == false) {
				wasProgramAlreadyLaunched = true;
			}
			else if (wasProgramAlreadyLaunched == true) {
				if (Time.time >= minTimeForSplash) {
					EventBuilder (new Message ("SCENE_ProgramLaunched", "done loading"));
					this.enabled = false;
				}
			}
		}

		//Clear message variables
		messageForBuilder = new Message ();
		messageCurrentScene = new Message ();
	}
}