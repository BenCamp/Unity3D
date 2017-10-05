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
	string currentScene = "";
	string currentData = "";
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
		screenTitle = (VideoClip)Resources.Load ("Movies/TitleScreen");
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
		
		/*Scene has changed
		 * 	SET currentScene to provided scene
		 * 	
		 * 	currentScene SCENE_ProgramLaunched
		 * 		isSplashStarted is FALSE
		 *			CLEAR messageCurrentScene
		 * 			SET isSplashStarted to TRUE
		 * 			SET currentVideo clip to screenSplash
		 * 			PLAY the video
		 * 
		 * 		isSplashStarted is TRUE
		 * 			Error (ControllerCinematic -> Update -> messageCurrentScene -> scene is not empty -> currentScene equals SCENE_ProgramLaunched: Splash Screen was already active.)
		 * 
		 * 	currentScene is 
		 * 
		 * 
		 * 	currentScene is something else
		 * 		DO nothing
		 * 	
		 */
		if (messageCurrentScene.scene.ToString () != "") {
			currentScene = messageCurrentScene.scene.ToString ();

			//Changed to SCENE_ProgramLaunched
			if (currentScene == "SCENE_ProgramLaunched") {
				//Haven't already started the splash
				if (isSplashStarted == false) {
					isSplashStarted = true;
					currentVideo.clip = screenSplash;
					currentVideo.Play ();
				}

				//Splash already started and shouldn't have
				else if (isSplashStarted == true) {
					EventCinematic (new Message ("ERROR", ErrorStrings.GetError("CCIN0001")));
				}
			}
			//Some other stuff probably
			//Default: don't do anything
		}

		/*ControllerGame has sent a direct message
		 * 		currentScene does not match provided scene name
		 * 			Error (ControllerCinematic -> Update -> messageForCinematic -> scene is not empty: Missed a scene change.)
		 * 
		 * 		SET currentData to provided data
		 * 		CLEAR messageForCinematic
		 * 
		 * 		currentScene is SCENE_ProgramLaunched
		 * 			currentData is "end splash"
		 * 				
		 * 			currentData is "end title"
		 * 
		 * 			else
		 * 				Error (ControllerCinematic -> Update -> messageForCinematic -> scene is not empty -> currentScene equals SCENE_ProgramLaunched: Game shouldn't be sending this kind of data)
		 * 
		 * 		currentScene is 
		 * 
		 * 		currentScene is something else
		 * 			Error (ControllerCinematic -> Update -> messageForCinematic -> scene is not empty -> else: Game shouldn't be sending data to Cinematic in this scene)
		 * 
		 */
		if (messageForCinematic.scene.ToString () != "") {
			//currentScene does not match provided scene name
			if (messageForCinematic.scene.ToString () != currentScene) {
				EventCinematic (new Message ("ERROR", ErrorStrings.GetError("CCIN0002")));
			}

			//Save the data from the message and clear the message
			currentData = messageForCinematic.data.ToString ();

			/*In SCENE_ProgramLaunched*/
			if (currentScene == "SCENE_ProgramLaunched") {
				if (currentData == "end splash") {
					if (currentVideo.clip.originalPath == "Assets/Resources/Movies/SplashScreen.mp4") {
						currentVideo.Stop ();
						currentVideo.clip = screenTitle;
						currentVideo.Play ();
						currentData = "";
					}
					else {
						EventCinematic (new Message ("ERROR", ErrorStrings.GetError("CCIN0003")));
					}
				}

				else if (currentData == "end title") {
					if (currentVideo.clip.originalPath ==  "Assets/Resources/Movies/TitleScreen.mp4"){
						currentVideo.Stop ();
						EventCinematic (new Message ("SCENE_ProgramLaunched", "done title"));
					}
					else {
						EventCinematic (new Message ("ERROR", ErrorStrings.GetError("CCIN0004")));
					}
				}

				else {
					EventCinematic (new Message ("ERROR", ErrorStrings.GetError("CCIN0005")));
				}
			}
		}

		//Clear message variables
		messageCurrentScene = new Message ();
		messageForCinematic = new Message ();
	}
}
