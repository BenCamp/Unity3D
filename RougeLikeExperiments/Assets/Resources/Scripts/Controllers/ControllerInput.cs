using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : MonoBehaviour {

	public Message messageCurrentScene = new Message();

	public static ControllerInput controllerInput;

	public delegate void UserInput (Message message);

	public static event UserInput EventInput;
	void Awake () {
		if (controllerInput == null) {
			DontDestroyOnLoad (gameObject);
			controllerInput = this;
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
