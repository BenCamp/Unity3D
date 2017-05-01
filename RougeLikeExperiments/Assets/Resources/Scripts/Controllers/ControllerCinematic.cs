using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCinematic : MonoBehaviour {


	public Message messageCurrentScene = new Message();

	public static ControllerCinematic controllerCinematic;

	public delegate void Status (Message message);

	public static event Status EventCinematic;

	void Awake () {
		if (controllerCinematic == null) {
			DontDestroyOnLoad (gameObject);
			controllerCinematic = this;
		} else {
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

	void EventChangeScene (Message message){
		messageCurrentScene = message;
	}
}
