/*
 * First! Beelzabee :P
 * 
 * This class will keep track of everything that doesn't have to do with direct object to object interaction
 * Things, such as a player, will inform this class when something happens to it. When the player gets hit,
 * for example, it will launch off a message to this class and inform it that the so-and-so hit it for 
 * such and such damage. The game will keep track of when the player dies or whatnot.
 * 
 * I want to go in this direction so I can have each gameobject doing as little as possible every turn.
 * I don't know if this is best practice, but I can imagine a scenario where there are a multitude of 
 * state checkers on each spawned object and I think just having a centralized state checker might be better
 * on the processor.. maybe even ram. I dunno, this is an experiment.
 * 
 * 
 * Please inform me if I misspell anything guys, I've become dependent on autochecker and am trying to 
 * break the habit :P
 * 
 * 
 * My (current) Coding Conventions I'm using for Unity 3D. I'm mostly following the Csharp Coding Guidelines
 * from the Unity wiki, but I figured I'd list my differences and specific quirks here. 
 * This also might not be a complete list so.. meh you'll figure it out.
 * Anywho, THAT:
 * 
 * Descriptive names tend to follow a particular formula, with the more general elements coming first,
 * and the more specific coming last. 
 * e.g.: 1st: Food - 2nd: Fruit - 3rd: Orange
 * 
 * Variables/Fields will almost always be camelCase with a descriptive name.
 * e.g.: foodFruitOrange or weaponMeleeKnife
 * However, if the name indicates some kind of state, like a bool, it usually follows the pattern of
 * is[Somesuch][Something] (keeping the camelCase)
 * e.g.: isMenuOpen or isPlayerDead
 * 
 * Anything that is a non-script resource or Unity Specific 'thing' like a Scene or a Prefab 
 * has that 'thing' as its first name ALL CAPS, an underline, 
 * and a descriptive name with no spaces, with all words Capitalized.
 * e.g.: SCENE_MenuStart or PREFAB_MonsterOrcGrunt or CINEMATIC_Intro
 * This doesn't include scripts.
 * 
 * Any Script that has to be constantly checking update will have a descriptive name that begins with Controller
 * e.g.: ControllerGame or ControllerArrow or ControllerMonster
 * I'm not 100% I need this however.. Again, this is a test.
 * 
 * All errors kill the game and do a log dump
 * 
 * Word of WARNING!! I am not above making a variable/field's or function's name an entire descriptive sentence :P 
 * 
 * Also, remember I have little experience in this area.. if something seems stupid either I'm smartenating
 * all over this or I'm doing something.. really stupid. Please inform me if you suspect the latter xD
 * 
 * Meanwhile, the theme of this game is going to be singletons and callbacks. I wanna see what happens.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/***Singleton***/
public class ControllerGame : MonoBehaviour {
	public static ControllerGame controllerGame;

	/***Speakers***/
	public delegate void EventGame (Message message);
	public static event EventGame EventChangeScene;
	public static event EventGame EventForGUI;
	public static event EventGame EventForCinematic;
	public static event EventGame EventForInput;
	public static event EventGame EventForBuilder;

	/***Listeners***/
	void EventGUI (Message message){
		messageGUI = message;
		Debug.Log ("From GUI: " + message.data);
		if (messageGUI.scene == "ERROR") {
			Debug.LogError (ErrorStrings.GetError ("CG0003")+ messageGUI.data);
			Application.Quit ();
		}
	}
	void EventCinematic (Message message){
		messageCinematic = message;
		Debug.Log ("From Cinematic: " + message.data);
		if (messageCinematic.scene == "ERROR"){
			Debug.LogError (ErrorStrings.GetError("CG0004") + messageCinematic.data);
			Application.Quit ();
		}
	}
	void EventInput (Message message){
		messageInput = message;
		Debug.Log ("From Input: " + message.data);
		if (messageInput.scene == "ERROR") {
			Debug.LogError (ErrorStrings.GetError("CG0005") + messageInput.data);
			Application.Quit ();
		}
	}
	void EventBuilder (Message message){
		messageBuilder = message;
		Debug.Log ("From Builder: " + message.data);
		if (messageBuilder.scene == "ERROR") {
			Debug.LogError (ErrorStrings.GetError("CG0006") + messageBuilder.data);
			Application.Quit ();
		}
	}

	/***Messages***/
	public Message messageGUI = new Message();
	public Message messageCinematic = new Message();
	public Message messageInput = new Message();
	public Message messageBuilder = new Message();
		
	/***Variables***/
	public bool isMenuOpen = true;
	public bool isGamePaused = true;
	public bool isIntroFinished = false;
	public bool isBuilderWorldFinished = false;
	//Cursor cursorMenu;

	/***GameObjects***/
	public GameObject world;
	public GameObject player;
	public ArrayList enemies;
	public ArrayList allies;
	public ArrayList itemsPickup;
	public ArrayList objectsWorld;

	/***MonoBehaviour Classes***/
	void Awake () {
		//This is the only controllerGame on a GameObject
		if (controllerGame == null) {
			DontDestroyOnLoad (gameObject);
			controllerGame = this;
		} 

		//This is NOT the only controllerGame on a GameObject
		else {
			Destroy (gameObject);
		}

	}
	void OnEnable (){
		//Enable Listeners for events
		ControllerGUI.EventGUI += EventGUI;
		ControllerCinematic.EventCinematic += EventCinematic;
		ControllerInput.EventInput += EventInput;
		ControllerBuilder.EventBuilder += EventBuilder;
	}
	void OnDisable (){
		//Disable Listeners for events
		ControllerGUI.EventGUI -= EventGUI;
		ControllerCinematic.EventCinematic -= EventCinematic;
		ControllerInput.EventInput -= EventInput;
		ControllerBuilder.EventBuilder -= EventBuilder;
	}
	void Start () 
	{	
		/*SET state bools
		*	SET isMenuOpen to TRUE
		*	SET isGamePaused to TRUE
		*	SET isIntroFinished to FALSE
		*	SET isBuilderWorldFinished to FALSE
		*/
		isMenuOpen = true;
		isGamePaused = true;
		isIntroFinished = true;
		isBuilderWorldFinished = true;

		if (SceneManager.GetActiveScene ().name != "working") {
			/*Deal With Mouse
			 *		Capture Mouse
			 *		Hide Mouse
			 */

			/*LOAD SCENE_ProgramLaunched*/
			ChangeScene("SCENE_ProgramLaunched");
		}
	}
	void Update ()
	{
		/*isMenuOpen is TRUE*/
		if (isMenuOpen == true) {
			
			/*	
			 *	Double Check that the game is paused
			 *  isGamePaused is FALSE
			 *		ERROR: There shouldn't be any circumstance that isMenuOpen is true if you aren't paused)
			 */
			if (isGamePaused == false) {
				Debug.LogError (ErrorStrings.GetError("CG0007"));
				Application.Quit ();
			}

			/* TODO 
			 * Remember to remove this "In Working" part when done the game...
			 */

			/* In working */
			if (SceneManager.GetActiveScene ().name == "working") {

			}

			/*In SCENE_ProgramLaunched
			 * ControllerBuilder has sent a message
			 * 		Provided scene is SCENE_ProgramLaunched
			 *			Loading Done
			 *				TO ControllerInput: Listen for user input to skip title
			 *				TO ControllerCinematic: SHOW screenTitle
			 *
			 *			Default
			 *				ERROR: Builder shouldn't be sending this kind of data)
			 *		
			 *		Default
			 *			ERROR: Builder shouldn't be sending this kind of scene
			 *  
			 * ControllerInput has sent a message
			 * 		User input something
			 * 			TO ControllerCinematic: STOP screenTitle
			 * 			
			 * 
			 * ControllerCinematic has sent a message
			 * 		Provided scene is SCENE_ProgramLaunched
			 * 			screenTitle ended
			 * 				LOAD SCENE_MenuStart
			 * 			Default  
			 *				ERROR: Cinematic shouldn't be sending this kind of data
			 *  
			 * 		Default
			 * 			ERROR: Cinematic shouldn't be sending this kind of data
			 *  
			 */
			if (SceneManager.GetActiveScene().name == "SCENE_ProgramLaunched") {

				//ControllerBuilder has sent a message
				if (messageBuilder.scene.ToString () != "") {
					//ControllerBuilder sent correct scene data
					if (messageBuilder.scene.ToString () == "SCENE_ProgramLaunched") {
						if (messageBuilder.data.ToString () == "done loading") {
							EventForCinematic (new Message ("SCENE_ProgramLaunched", "end splash")); 
							Debug.Log ("To Cinematic: end splash");
							EventForInput (new Message ("SCENE_ProgramLaunched", "title has started"));
							Debug.Log ("To Input: title has started");
						} 
						else {
							Debug.LogError (ErrorStrings.GetError ("CG0001"));
							Application.Quit ();
						}
					} 
					else {
						Debug.LogError (ErrorStrings.GetError ("CG0002"));
						Application.Quit ();
					}
				}
					
				//ControllerInput has sent a message
				if (messageInput.scene.ToString () != "") {
					//ControllerInput sent correct scene data
					if (messageInput.scene.ToString () == "SCENE_ProgramLaunched") {
						EventForCinematic (new Message ("SCENE_ProgramLaunched", "end title"));
						Debug.Log ("To Cinematic: end title");
					}
				}

				//ControllerCinematic has sent a message
				if (messageCinematic.scene.ToString () != "") {
					//ControllerCinematic sent correct scene data
					if (messageCinematic.scene.ToString () == "SCENE_ProgramLaunched") {
						if (messageCinematic.data == "done title") {
							ChangeScene ("SCENE_MenuStart");
						} 
						else {
							Debug.LogError (ErrorStrings.GetError ("CG0009"));
							Application.Quit ();
						}
					}
					else {
						Debug.LogError (ErrorStrings.GetError ("CG0010"));
						Application.Quit ();
					}
				}
			}

			/*	In SCENE_MenuStart	
			 * 		ControllerGUI has sent a message
			 * 			Provided Scene is SCENE_MenuStart
			 *				User has selected "New Game"
			 *					LOAD SCENE_MenuNewGame
			 *		
			 *				User has selected "Load Game"
			 *					LOAD SCENE_MenuLoadGame
			 *		
			 *				User has selected "Credits"
			 *					LOAD SCENE_Credits
			 *
			 *				User has selected "Quit"
			 *					Load SCENE_Quit
			 *					or
			 *					Exit Game
			 *			
			 *				Default
			 *					ERROR: GUI shouldn't be sending this kind of data
			 *		
			 *		ControllerBuilder has sent a message
			 *			ERROR: Builder shouldn't be sending data during Start Menu
			 *
			 *		ControllerInput has sent a message
			 *			Provided scene was SCENE_MenuStart
			 *				
			 *		ControllerCinematic has sent a message
			 *
			 *		ControllerCamera has sent a message
			 *
			 *		
			 */
			if (SceneManager.GetActiveScene().name == "SCENE_MenuStart") {
				//ControllerBuilder has sent a message
				if (messageBuilder.scene.ToString () != "") {
					Debug.LogError (ErrorStrings.GetError ("CG0011"));
					Application.Quit ();
				}

				//ControllerCinematic has sent a message
				if (messageCinematic.scene.ToString () != "") {
					Debug.LogError (ErrorStrings.GetError ("CG0014"));
				}

				//ControllerGUI has sent a message
				if (messageGUI.scene.ToString () != "") {
					if (messageGUI.scene.ToString () == "SCENE_MenuStart") {
						string data = messageGUI.data;
						if (data == "new") {
							ChangeScene ("SCENE_MenuNewGame");
						}
						else if (data == "load") {
							Debug.Log ("Load was sent from GUI");
						}
						else if (data == "credits") {
							Debug.Log ("Credits was sent from GUI");
						}
						else if (data == "quit") {
							Debug.Log ("Quit was sent from GUI");
						}
						else {
							Debug.LogError (ErrorStrings.GetError ("CG0012"));
							Application.Quit ();
						}
					}
					else {
						Debug.LogError (ErrorStrings.GetError ("CG0013"));
					}
				}

				//ControllerInput has sent a message
				if (messageInput.scene.ToString () != "") {
					//ControllerInput sent correct scene data
					if (messageInput.scene.ToString () == "SCENE_ProgramLaunched") {
						EventForCinematic (new Message ("SCENE_ProgramLaunched", "end title"));
						Debug.Log ("To Cinematic: end title");
					}
				}

				//ControllerCinematic has sent a message
				if (messageCinematic.scene.ToString () != "") {
					//ControllerCinematic sent correct scene data
					if (messageCinematic.scene.ToString () == "SCENE_ProgramLaunched") {
						if (messageCinematic.data == "done title") {
							ChangeScene ("SCENE_MenuStart");
						} 
						else {
							Debug.LogError (ErrorStrings.GetError ("CG0009"));
							Application.Quit ();
						}
					}
					else {
						Debug.LogError (ErrorStrings.GetError ("CG0010"));
						Application.Quit ();
					}
				}
			}

			//TODO
			/*	In SCENE_MenuNewGame
			 *		ControllerGUI has sent a message
			 *			New-Game Values RETURNED
			 *				TO ControllerBuilder: Build world with values from controllerGUI
			 *				LOAD SCENE_NewGame
			 *					
			 *			Null RETURNED
			 *				LOAD SCENE_MenuStart				
		 	 *
			 *			Default
			 *				ERROR: GUI shouldn't be sending this kind of data)
			 *		
			 */	
			if (SceneManager.GetActiveScene().name == "SCENE_MenuNewGame") {
				string data = messageGUI.data;
				switch (data) {

				case "cancel":
					ChangeScene ("SCENE_MenuStart");
					break;

				//Something received

				case "go":
					EventForBuilder (new Message ("SCENE_MenuNewGame", "go"));
					Debug.Log ("To Builder: go");
					ChangeScene ("SCENE_NewGame");
					break;

				default:
					break;
				}
				
			}

			//TODO
			/*	In SCENE_NewGame
			 *		isIntroFinished is TRUE and world is not null
			 *			TO ControlerCinematic: CLOSE loading screen
			 *			LOAD SCENE_PlayingGame
			 *
			 *		ControllerCinematic has sent a message
			 *			Intro has played successfully
			 *				SET isIntroFinished to TRUE
			 *						
			 *			Default
			 *				ERROR: Cinematic shouldn't be sending this kind of data
			 *
			 *		ControllerBuilder has sent a message
			 *			ControllerBuilder has returned the prepared world
			 *				SET local world to returned world
			 *			
			 *			Default
			 *				ERROR: Builder shouldn't be sending this kind of data
			 *
			 *		ControllerInput has sent a message
			 *			User Input
			 *				isIntroFinished is FALSE
			 *					TO ControlerCinematic: End Intro prematurely
			 *			Default
			 *				ERROR: Input shouldn't be sending this kind of data
			 *
			 *		ControllerGUI has sent a message
			 *			Default
			 *				ERROR: GUI shouldn't be sending this kind of data
			 */
			if (SceneManager.GetActiveScene().name == "SCENE_NewGame") {
				
				ChangeScene ("SCENE_PlayingGame");
			}

			//TODO
			/*	In SCENE_MenuLoadGame
			 *		ControllerGUI has sent a message
			 *			Load-Game Values RETURNED
			 *			LOAD SCENE_LoadGame
			 *
			 *		Null RETURNED
			 *			LOAD SCENE_MenuStart				
			 *
			 *		Default
			 *			ERROR: GUI shouldn't be sending this kind of data
			 */
			if (SceneManager.GetActiveScene().name == "SCENE_MenuLoadGame") {

			}

			//TODO
			/*	In SCENE_Credits
			 */
			if (SceneManager.GetActiveScene().name == "SCENE_Credits") {

			}
				
			//TODO
			/*	In SCENE_Quit (possibly going to be kicked)
			 */
			if (SceneManager.GetActiveScene().name == "SCENE_Quit") {

			}

			//TODO
			/*	In SCENE_PlayingGame
			 */
			if (SceneManager.GetActiveScene().name == "SCENE_PlayingGame") {

			}
		}

		//TODO
		/*isMenuOpen is FALSE*/
		if (isMenuOpen == false) {

			//TODO remove "working" scene check when game is complete
			/*
			 *	Not in SCENE_PlayingGame
			 *		ERROR: There shouldn't be any circumstance that isMenuOpen is false if you aren't in SCENE_PlayingGame
			 *
			 */
			if (SceneManager.GetActiveScene ().name != "SCENE_PlayingGame" || SceneManager.GetActiveScene ().name != "working") {
				Debug.LogError (ErrorStrings.GetError("CG0008"));
				Application.Quit ();
			}

			/* isGamePaused is TRUE*/
			if (isGamePaused == true) {

			}

			/*	isGamePaused is FALSE*/
			if (isGamePaused == false) {

			}
		}

		//Clear message variables
		messageBuilder = new Message ();
		messageCinematic = new Message ();
		messageGUI = new Message ();
		messageInput = new Message ();

	}

	void ChangeScene (string scene){
		SceneManager.LoadScene (scene);
		EventChangeScene (new Message (scene, ""));
		Debug.Log ("Changed Scene to " + scene);
	}
}