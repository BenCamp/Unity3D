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
	void EventForInput (Message message){
		messageForInput = message;
	}

	/***Messages***/
	public Message messageCurrentScene = new Message();
	public Message messageForInput = new Message ();

	/***Variables***/
	bool amIListeningForInput = false;
	string currentScene = "";
	string currentData = "";

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
		ControllerGame.EventForInput += EventForInput;
	}
	void OnDisable (){
		//Disable Listeners for events
		ControllerGame.EventChangeScene -= EventChangeScene;
		ControllerGame.EventForInput -= EventForInput;
	}
	void Update (){
		/*Scene has changed
		 * 	SET currentScene to provided scene
		 */ 	
		if (messageCurrentScene.scene.ToString () != "") {
			currentScene = messageCurrentScene.scene.ToString ();

		}

		/*ControllerGame has sent a direct message
		 * 		currentScene matches the provided scene name
		 *			SET currentData to provided data
		 *
		 *		Default
		 * 			Error (ControllerInput -> Update -> ControllerGame message: currentScene does not match provided Scene name)
		 *			
		 */
		if (messageForInput.scene.ToString () != "") {
			if (currentScene == messageForInput.scene.ToString ()) {
				currentData = messageForInput.data;
			} 
			else {
				EventInput (new Message ("ERROR", ErrorStrings.GetError ("CI0001")));
			}
		}


		/*currentData is not empty
		 *	SET tempData equal to currentData
		 *	CLEAR currentData
		 *
		 *	currentScene is SCENE_ProgramLaunched
		 *		tempData is saying title has started
		 *			SET amIListeningForInput to TRUE
		 */
		if (currentData != "") {
			string tempData = currentData;
			currentData = "";

			if (currentScene == "SCENE_ProgramLaunched") {
				if (tempData == "title has started") {
					amIListeningForInput = true;
				}
			}
		}

		/*amIListeningForInput is TRUE
		 * 	currentScene is SCENE_ProgramLaunched
		 *		User Input something
		 *			TO Game: User input something
		 * 		
		 */
		if (amIListeningForInput == true) {
			if (currentScene == "SCENE_ProgramLaunched"){
				if (Input.anyKey == true) {
					EventInput (new Message ("SCENE_ProgramLaunched", ""));
				}
			}
		}
	}
}