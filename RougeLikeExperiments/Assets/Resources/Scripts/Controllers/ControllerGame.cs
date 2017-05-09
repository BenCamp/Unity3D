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
	}
	void EventCinematic (Message message){
		messageCinematic = message;
	}
	void EventInput (Message message){
		messageInput = message;
	}
	void EventBuilder (Message message){
		messageBuilder = message;
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

		/*LOAD SCENE_ProgramLaunched*/
		SceneManager.LoadScene("SCENE_ProgramLaunched");
		EventChangeScene (new Message ("SCENE_ProgramLaunched", ""));
	}
	void Update ()
	{
		/*A callback registered an error*/
		if (messageGUI.scene == "ERROR") {
			Debug.Log ("ControllerGame.Update: Something exploded in the GUI: " + messageGUI.data);
			Application.Quit ();
		}
		if (messageCinematic.scene == "ERROR"){
			Debug.Log ("ControllerGame.Update: Something exploded in the Cinematic: " + messageCinematic.data);
			Application.Quit ();
		}
		if (messageInput.scene == "ERROR") {
			Debug.Log ("ControllerGame.Update: Something exploded in the Input: " + messageInput.data);
			Application.Quit ();
		}
		if (messageBuilder.scene == "ERROR") {
			Debug.Log ("ControllerGame.Update: Something exploded in the Builder: " + messageBuilder.data);
			Application.Quit ();
		}

		/*isMenuOpen is TRUE*/
		if (isMenuOpen == true) {
			
			/*	
			 *	Double Check that the game is paused
			 *  isGamePaused is FALSE
			 *		Error (ControllerGame.Update.isMenuOpen:TRUE: There shouldn't be any circumstance that isMenuOpen is true if you aren't paused)
			 */
			if (isGamePaused == false) {
				Debug.LogError ("ControllerGame.Update.isMenuOpen:TRUE: There shouldn't be any circumstance that isMenuOpen is true if you aren't paused");
				Application.Quit ();
			}


			/*In SCENE_ProgramLaunched
			 * Builder Callback has registered some message
			 *		Loading Done
			 *			TO ControllerGUI: Listen for clicks to skip title
			 *			TO ControllerCinematic: SHOW screenTitle
			 *
			 *		Default
			 *			Error (ControllerGame.Update.isMenuOpen:TRUE.ProgramLaunched: Builder shouldn't be sending this kind of data)
			 *
			 *  
			 * Input Callback has registered some message
			 * 		User input something
			 * 			In screenTitle
			 * 				TO ControllerCinematic: STOP screenTitle
			 * 			
			 * 
			 * Cinematic Callback has registered some message
			 * 		Title ended
			 * 			LOAD SCENE_MenuStart
			 *	
			 */
			if (SceneManager.GetActiveScene().name == "SCENE_ProgramLaunched") {
				switch (messageBuilder.data.ToString()) {
				case "done loading":
					EventForCinematic (new Message ("", "end splash")); 
					break;

				default:
					break;
				}

				switch (messageCinematic.data.ToString()) {
				case "done splash":

					break;

				case "done title":

					break;

				default:
					break;
				}
			}

			/*	In SCENE_MenuStart	
			 *		GUI Callback has registered some message
			 *			User has selected "New Game"
			 *				LOAD SCENE_MenuNewGame
			 *		
			 *			User has selected "Load Game"
			 *				LOAD SCENE_MenuLoadGame
			 *		
			 *			User has selected "Credits"
			 *				LOAD SCENE_Credits
			 *
			 *			User has selected "Quit"
			 *				Load SCENE_Quit
			 *				or
			 *				Exit Game
			 *			
			 *			Default
			 *				Error (ControllerGame.Update.isMenuOpen:TRUE.MenuStart: GUI shouldn't be sending this kind of data)
			 *
			 */
			if (SceneManager.GetActiveScene().name == "SCENE_MenuStart") {
				string data = messageGUI.data;
				switch (data) {
				case "":
					break;

				case "new":

					break;

				case "load":

					break;

				case "credits":

					break;

				case "quit":

					break;

				default:
					break;
				}
			}

			/*	In SCENE_MenuNewGame
			 *		GUI Callback has registered some message
			 *			New-Game Values RETURNED
			 *				LOAD SCENE_NewGame
			 *					
			 *			Null RETURNED
			 *				LOAD SCENE_MenuStart				
		 	 *
			 *			Default
			 *				Error (ControllerGame.Update.isMenuOpen:TRUE.MenuNewGame: UI shouldn't be sending this kind of data)
			 *		
			 */	
			if (SceneManager.GetActiveScene().name == "SCENE_MenuNewGame") {
				string data = messageGUI.data;
				switch (data) {

				//Nothing received
				case "":
					break;

				case "error":

					break;

				//Something received
				default:
					break;
				}
				
			}

			/*	In SCENE_NewGame
			 *		New-Game Values are not null
			 *			PASS New-Game Values to BuilderWorld (will build the game in the background)
			 *			Set New-Game Values to null
			 *
			 *		isIntroFinished and isBuilderWorldFinished are TRUE
			 *			TO UI: CLOSE loading screen
			 *			LOAD SCENE_PlayingGame
			 *
			 *		ControllerCinematic Callback has registered some message
			 *			Intro has played successfully
			 *				SET isIntroFinished to TRUE
			 *						
			 *			Default
			 *				Error (ControllerGame.Update.isMenuOpen:TRUE.NewGame: ControllerCinematic shouldn't be sending this kind of data)
			 *
			 *		BuilderWorld Callback has registered some message
			 *			RETURNS TRUE (SCENE_PlayingGame is built, I'm assuming that you can dynamically build a scene you aren't in.. :/ we'll just have to wait and see)
			 *				SET isBuilderWorldFinished to TRUE
			 *			
			 *			Default
			 *				Error (ControllerGame.Update.isMenuOpen:TRUE.NewGame: BuilderWorld shouldn't be sending this kind of data)
			 *
			 *		ControllerInput Callback has registered some message
			 *			User Input
			 *				isIntroFinished is FALSE
			 *					TO ControlerCinematic: End Intro prematurely
			 *			Default
			 *				Error (ControllerGame.Update.isMenuOpen:TRUE.NewGame: ControllerInput shouldn't be sending this kind of data)
			 *
			 *		GUI Callback has registered some message
			 *			Default
			 *				Error (ControllerGame.Update.isMenuOpen:TRUE.NewGame: UI shouldn't be sending this kind of data)
			 */
			if (SceneManager.GetActiveScene().name == "SCENE_NewGame") {

			}

			/*	In SCENE_MenuLoadGame
			 *		UI Callback has registered some message
			 *			Load-Game Values RETURNED
			 *			LOAD SCENE_LoadGame
			 *
			 *		Null RETURNED
			 *			LOAD SCENE_MenuStart				
			 *
			 *		Default
			 *			Error (ControllerGame.Update.isMenuOpen:TRUE.MenuLoadGame: UI shouldn't be sending this kind of data)
			 */
			if (SceneManager.GetActiveScene().name == "SCENE_MenuLoadGame") {

			}

			/*	In SCENE_Credits
			 */
			if (SceneManager.GetActiveScene().name == "SCENE_Credits") {

			}

			/*	In SCENE_Quit (possibly going to be kicked)
			 */
			if (SceneManager.GetActiveScene().name == "SCENE_Quit") {

			}

			/*	In SCENE_PlayingGame
			 */
			if (SceneManager.GetActiveScene().name == "SCENE_PlayingGame") {

			}
		}

		/*isMenuOpen is FALSE*/
		if (isMenuOpen == false) {
			
			/*
			 *	Not in SCENE_PlayingGame
			 *		Error (ControllerGame.Update.isMenuOpen:FALSE.PlayingGame: There shouldn't be any circumstance that isMenuOpen is false if you aren't in SCENE_PlayingGame)
			 *
			 */
			if (SceneManager.GetActiveScene ().name != "SCENE_PlayingGame") {
				Debug.LogError ("ControllerGame.Update.isMenuOpen:FALSE.PlayingGame: There shouldn't be any circumstance that isMenuOpen is false if you aren't in SCENE_PlayingGame");
				Application.Quit ();
			}

			/* isGamePaused is TRUE*/
			if (isGamePaused == true) {

			}

			/*	isGamePaused is FALSE*/
			if (isGamePaused == false) {

			}
		}
	}
}