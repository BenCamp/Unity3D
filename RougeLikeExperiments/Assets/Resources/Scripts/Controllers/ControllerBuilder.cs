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
		ControllerGame.EventForBuilder += EventForBuilder;
	}
	void OnDisable (){
		//Disable Listeners for events
		ControllerGame.EventChangeScene -= EventChangeScene;
		ControllerGame.EventForBuilder -= EventForBuilder;
	}
	void Update (){

		/*Scene has been changed
		 * 	Program returned to the ProgramLaunched scene
		 * 		ERROR: Duplicate SCENE_ProgramLaunched message or somehow returned to SCENE_ProgramLaunched
		 */
		if (messageCurrentScene.scene.ToString () != "") {
			currentScene = messageCurrentScene.scene.ToString ();
			if (currentScene == "SCENE_ProgramLaunched" && wasProgramAlreadyLaunched == true) {
				EventBuilder (new Message ("ERROR", "CB0001"));
			}
		}

		//TODO
		/*In SCENE_ProgramLaunched
		 * 	wasProgramAlreadyLaunched is FALSE
		 * 		Build whatever is needed before the menus load
		 * 		SET wasProgramAlreadyLaunched to TRUE
		 * 
		 * 	wasProgramAlreadyLaunched is TRUE
		 * 		world is not built
		 * 
		 * 		world is built
		 * 			TO ControllerGame: Menus are ready
		 * 
		 * 
		 */
		if (currentScene == "SCENE_ProgramLaunched") {
			if (wasProgramAlreadyLaunched == false) {
				if (Time.time >= minTimeForSplash) {
					EventBuilder (new Message ("SCENE_ProgramLaunched", "done loading"));
					wasProgramAlreadyLaunched = true;
				}
			}
		}

		//TODO
		/* In SCENE_MenuNewGame
		 * 
		 */
		if (currentScene == "SCENE_MenuNewGame") {

		}

		//TODO
		/* In SCENE_NewGame
		 * 
		 */
		if (currentScene == "SCENE_NewGame") {

		}

		//TODO
		/* In SCENE_LoadGame
		 * 
		 */
		if (currentScene == "SCENE_LoadGame") {

		}

		//Clear message variables
		messageForBuilder = new Message ();
		messageCurrentScene = new Message ();
	}
}