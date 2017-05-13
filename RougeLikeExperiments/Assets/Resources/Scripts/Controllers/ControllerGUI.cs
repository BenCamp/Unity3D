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
		currentScene = message.scene;
	}

	/***Messages***/
	public Message messageForGUI = new Message ();
	public Message messageCurrentScene = new Message();

	/***Variables***/
	string currentScene = "";
	string currentData = "";

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
			if (currentScene == "SCENE_ProgramLaunched"){
			} 
			else if (currentScene == "SCENE_MenuStart"){
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
			}
			else if (currentScene == "SCENE_MenuNewGame"){}
			else if (currentScene == "SCENE_NewGame"){}
			else if (currentScene == "SCENE_MenuLoadGame"){}
			else if (currentScene == "SCENE_Credits"){}
			else if (currentScene == "SCENE_Quit"){}
			else if (currentScene == "SCENE_PlayingGame") {}
			else {
				EventGUI (new Message ("ERROR", ErrorStrings.GetError("CGUI0001")));
			}
		} catch (Exception e){
			if (EventGUI != null) {
				EventGUI (new Message ("ERROR", e.Message));
			}
		}

		if (messageForGUI.scene.ToString () != "") {
			if (messageForGUI.scene.ToString () == currentScene) {
				
			}
			else {
				EventGUI (new Message ("ERROR", ErrorStrings.GetError ("CGUI0002")));
			}
		}

		//Clear message variables
		messageForGUI = new Message();
		messageCurrentScene = new Message ();
	}
}
