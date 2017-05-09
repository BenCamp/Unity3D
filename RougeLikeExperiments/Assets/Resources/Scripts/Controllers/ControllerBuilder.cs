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

	/***Messages***/
	public Message messageCurrentScene = new Message();

	/***Variables***/
	bool wasProgramAlreadyLaunched = false;

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
		if (messageCurrentScene.scene == "SCENE_ProgramLaunched") {
			if (wasProgramAlreadyLaunched == false) {
				wasProgramAlreadyLaunched = true;
				messageCurrentScene = new Message ();
				new WaitForSecondsRealtime (2f);

			}

			if (wasProgramAlreadyLaunched == true) {
				EventBuilder (new Message ("ERROR", "ControllerBuilder already received this message."));
			}
		}
	}
}