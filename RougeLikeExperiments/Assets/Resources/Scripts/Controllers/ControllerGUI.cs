/* FIRST! Beelzabee :P
 * 
 * I decided to go with strings as the primary form of communication because it's obvious 
 * in the code what you are trying to accomplish with these.
 * 
 * Each thing the GUIEvent returns will have a signature first that tells the 
 * ControllerGame what scene the ControllerGUI thinks the game is in, a space, and a 
 * command.
 * e.g.: "[SCENE] [COMMAND]"
 *
 * Uniformity going forward will probably be best :P
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/***Singleton***/
public class ControllerGUI : MonoBehaviour {
	public static ControllerGUI controllerGUI;

	/***Speakers***/
	public delegate void ActionClick (Message message);
	public static event ActionClick EventGUI;

	/***Listeners***/
	void EventForGUI (Message message){
		messageForGUI = message;
	}
	void EventChangeScene (Message message){
		messageCurrentScene = message;
	}

	/***Messages***/
	public Message messageForGUI = new Message ();
	public Message messageCurrentScene = new Message();

	/***Variables***/

	/***Monobehavior functions***/
	void Awake () {
		//This is the only controllerGUI on a GameObject
		if (controllerGUI == null) {
			DontDestroyOnLoad (gameObject);
			controllerGUI = this;
		} 
		//This is NOT the only controllerGUI on a GameObject
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
	void OnGUI (){
		try{
			switch (messageCurrentScene.scene.ToString()){
			case "SCENE_ProgramLaunched":
				if (messageCurrentScene.data == "splash") {
					GUI.Label (new Rect (Screen.width / 2 - 100, Screen.height / 2 - 15, 200, 30), "Splash Screen");
					if (GUI.Button (new Rect (Screen.width / 2 - 300, 5, 200, 30), "Go to Next Screen")) {
						if (EventGUI != null) {
							EventGUI (new Message ("SCENE_ProgramLaunched", "next"));
						}
					}
				} 
				else if (messageCurrentScene.data == "title") {
					GUI.Label (new Rect (Screen.width / 2 - 100, Screen.height / 2 - 15, 200, 30), "Title Screen");
					if (GUI.Button (new Rect (Screen.width / 2 - 300, 5, 200, 30), "Move to Next Scene")) {
						if (EventGUI != null) {
							EventGUI (new Message ("SCENE_ProgramLaunched", "next"));
						}
					}
				} 
				else {
					if (EventGUI != null) {
						EventGUI (new Message ("SCENE_ProgramLaunched", ""));
					}
				}
				break;

			case "SCENE_MenuStart":
				GUI.Label (new Rect (5, 5, 200, 30), "Start Menu");
				if (GUI.Button (new Rect (5, 55, 200, 30), "New Game")) {
					if (EventGUI != null) {
						EventGUI (new Message("SCENE_MenuStart", "new"));
					}
				}
				if (GUI.Button (new Rect (5, 90, 200, 30), "Load Game")) {
					if (EventGUI != null) {
						EventGUI (new Message ("SCENE_MenuStart", "load"));
					}
						
				}
				if (GUI.Button (new Rect (5, 135, 200, 30), "Credits")) {
					if (EventGUI != null) {
						EventGUI (new Message("SCENE_MenuStart", "credits"));
					}

				}
				if (GUI.Button (new Rect (5, 150, 200, 30), "Quit")) {
					if (EventGUI != null) {
						EventGUI (new Message("SCENE_MenuStart", "quit"));
					}

				}
				break;

			case "SCENE_MenuNewGame":
				break;

			case "SCENE_NewGame":
				break;

			case "SCENE_MenuLoadGame":
				break;

			case "SCENE_Credits":
				break;

			case "SCENE_Quit":
				break;

			case "SCENE_PlayingGame":
				break;

			default:
				if (GUI.Button (new Rect (Screen.width / 2 - 50, 5, 200, 30), "Click to Start Game")) {
					if (EventGUI != null) {
						EventGUI (new Message ("SCENE_Testing", "start"));
					}
				}

				if (GUI.Button (new Rect (Screen.width / 2 - 50, 60, 100, 30), "Unpause")) {
					if (EventGUI != null) {
						EventGUI (new Message ("SCENE_Testing", "unpause"));
					}
				}
				break;
			}
		} catch (Exception e){
			if (EventGUI != null) {
				EventGUI (new Message ("ERROR", e.Message));
			}
		}
	}
}
