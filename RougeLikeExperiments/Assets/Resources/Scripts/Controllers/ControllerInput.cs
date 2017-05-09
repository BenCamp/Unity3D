using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***Singleton***/
public class ControllerInput : MonoBehaviour {
	public static ControllerInput controllerInput;

	/***Speakers***/
	public delegate void UserInput (Message message);
	public static event UserInput EventInput;

	/**Listeners***/
	void EventChangeScene (Message message){
		messageCurrentScene = message;
	}

	/***Messages***/
	public Message messageCurrentScene = new Message();

	/***Variables***/

	/***Monobehaviour functions***/
	void Awake (){
		//This is the only controllerInput on a GameObject
		if (controllerInput == null) {
			DontDestroyOnLoad (gameObject);
			controllerInput = this;
		} 

		//This is NOT the only controllerInput on a GameObject
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
		if (messageCurrentScene.scene != "") {

		}
	}
}