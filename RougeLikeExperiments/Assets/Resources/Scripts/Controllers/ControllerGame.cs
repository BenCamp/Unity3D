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


public class ControllerGame : MonoBehaviour {

	/*
	 * Variables
	 */

	public static ControllerGame controllerGame;
	public ControllerGUI controllerGUI;
	public ControllerCinematic controllerCinematic;
	public ControllerInput controllerInput;

	public bool isMenuOpen = true;
	public bool isGamePaused = true;
	public bool isIntroFinished = false;
	public bool isBuilderWorldFinished = false;

	public Message messageGUI = new Message();
	public Message messageCinematic = new Message();
	public Message messageInput = new Message();

	//Cursor cursorMenu;


	/*
	 * MonoBehaviour Classes
	 */

	void Awake () {
		if (controllerGame == null) {
			DontDestroyOnLoad (gameObject);
			controllerGame = this;
		} else {
			Destroy (gameObject);
		}

	}

	void OnEnable (){
		//Enable Listeners for events
		ControllerGUI.EventGUI += EventGUI;
		ControllerCinematic.EventCinematic += EventCinematic;
		ControllerInput.EventInput += EventInput;
	}

	void OnDisable (){
		//Disable Listeners for events
		ControllerGUI.EventGUI -= EventGUI;
		ControllerCinematic.EventCinematic -= EventCinematic;
		ControllerInput.EventInput -= EventInput;
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
		/*LOAD SCENE_ProgramLaunched
		 *	TO UI: SHOW screenSplash
		 *	WAIT for PreparerGame to return bool (This class might be unnecessary if the game doesn't require any heavy loading, in which case, just delay for a time)
		 *		User: Furiously clicking and pressing keys to skip
		 *		Us: Laughing at the futility
		 *		PreparerGame RETURNED TRUE
		 *			BREAK out of WAIT
		 *		PreparerGame RETURNED FALSE
		 *			Error (Program Launch: PreparerGame exploded during the Splash Screen)
		 *	TO UI: CLOSE screenSplash
		 *	TO UI: SHOW screenTitle
		 *	WAIT for user to Input something
		 *		Error RETURNED from UI
		 *			Error (Program Launch: UI exploded during Title Screen)
		 *		Input registered
		 *			BREAK out of WAIT
		 *	TO UI: CLOSE screenTitle
		 */
		/*LOAD SCENE_MenuStart
		 *
		 */
	}
		
	void Update ()
	{
		
		/*isMenuOpen is TRUE*/
		if (isMenuOpen == true) {
			/*	
			 *	Double Check that the game is paused
			 *		isGamePaused is FALSE
			 *			Error (ControllerGame.Update.isMenuOpen:TRUE: There shouldn't be any circumstance that isMenuOpen is true if you aren't paused)
			 */
			if (isGamePaused == false) {
				Debug.LogError ("ControllerGame.Update.isMenuOpen:TRUE: There shouldn't be any circumstance that isMenuOpen is true if you aren't paused");
				Application.Quit ();
			}
			/*	UI is showing screenSplash or screenTitle
		 	 *		Error (ControllerGame.Update.isMenuOpen:TRUE: UI showing Splash or Title screen when it shouldn't)
		 	 */
			if (messageGUI.scene == "SCENE_ProgramLaunched") {
				Debug.LogError ("ControllerGame.Update.isMenuOpen:TRUE: UI showing Splash or Title screen when it shouldn't");
				Application.Quit ();
			}
			/*	In SCENE_MenuStart
			 *		Callback for UI not set
			 *			Error (ControllerGame.Update.isMenuOpen:TRUE.MenuStart: UI should have had a callback set in Start)
			 *		
			 *		UI Callback has registered some message
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
			 *			Error Value RETURNED
			 *				Error (ControllerGame.Update.isMenuOpen:TRUE.MenuStart: Something exploded in the UI)
			 *
			 *			Default
			 *				Error (ControllerGame.Update.isMenuOpen:TRUE.MenuStart: UI shouldn't be receiving this kind of data)
			 */
			/*	In SCENE_MenuNewGame
			 *		Callback for UI not set
			 *			Error (ControllerGame.Update.isMenuOpen:TRUE.MenuNewGame: UI should have had a callback set in Start)
		 	 *
			 *		UI Callback has registered some message
			 *			New-Game Values RETURNED
			 *				LOAD SCENE_NewGame
			 *					
			 *			Null RETURNED
			 *				LOAD SCENE_MenuStart				
		 	 *
			 *			Error Value RETURNED
			 *				Error (ControllerGame.Update.isMenuOpen:TRUE.MenuNewGame: UI exploded)
			 *
			 *			Default
			 *				Error (ControllerGame.Update.isMenuOpen:TRUE.MenuNewGame: UI shouldn't be receiving this kind of data)
			 */	
			/*	In SCENE_NewGame
			 *		Callback for BuilderWorld not set
			 *			Error (ControllerGame.Update.isMenuOpen:TRUE.NewGame: BuilderWorld should have had a callback set in Start)
			 *		Callback for UI not set
			 *			Error (ControllerGame.Update.isMenuOpen:TRUE.NewGame: UI should have had a callback set in Start)
			 *		Callback for ControllerCinematic not set
			 *			Error (ControllerGame.Update.isMenuOpen:TRUE.NewGame: ControllerCinematic should have had a callback set in Start)
			 *		Callback for ControllerInput not set
			 *			Error (ControllerGame.Update.isMenuOpen:TRUE.NewGame: ControllerInput should have had a callback set in Start)
		 	 *
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
			 *			Error Value RETURNED
			 *				Error (ControllerGame.Update.isMenuOpen:TRUE.NewGame: ControllerCinematic exploded while playing intro)
			 *		
			 *			Default
			 *				Error (ControllerGame.Update.isMenuOpen:TRUE.NewGame: ControllerCinematic shouldn't be receiving this kind of data)
			 *
			 *		BuilderWorld Callback has registered some message
			 *			RETURNS TRUE (SCENE_PlayingGame is built, I'm assuming that you can dynamically build a scene you aren't in.. :/ we'll just have to wait and see)
			 *				SET isBuilderWorldFinished to TRUE
			 *			
			 *			Error Value RETURNED
			 *				Error (ControllerGame.Update.isMenuOpen:TRUE.NewGame: BuilderWorld exploded)
			 *
			 *			Default
			 *				Error (ControllerGame.Update.isMenuOpen:TRUE.NewGame: BuilderWorld shouldn't be receiving this kind of data)
			 *
			 *		ControllerInput Callback has registered some message
			 *			User Input
			 *				isIntroFinished is FALSE
			 *					TO ControlerCinematic: End Intro prematurely
			 *
			 *			Error Value RETURNED
			 *				Error (ControllerGame.Update.isMenuOpen:TRUE.NewGame: ControllerInput exploded)
			 *			
			 *			Default
			 *				Error (ControllerGame.Update.isMenuOpen:TRUE.NewGame: ControllerInput shouldn't be receiving this kind of data)
			 *
			 *		UI Callback has registered some message
			 *			Error Value RETURNED
			 *				Error (ControllerGame.Update.isMenuOpen:TRUE.NewGame: UI exploded)
			 *			
			 *			Default
			 *				Error (ControllerGame.Update.isMenuOpen:TRUE.NewGame: UI shouldn't be receiving this kind of data)
			 */
			/*	In SCENE_MenuLoadGame
			 *		Callback for UI not set
			 *			Error (ControllerGame.Update.isMenuOpen:TRUE.MenuLoadGame: UI should have had a callback set in Start)
			 *
			 *		UI Callback has registered some message
			 *			Load-Game Values RETURNED
			 *			LOAD SCENE_LoadGame
			 *
			 *		Null RETURNED
			 *			LOAD SCENE_MenuStart				
			 *
			 *		Error Value RETURNED
			 *			Error (ControllerGame.Update.isMenuOpen:TRUE.MenuLoadGame: UI exploded)
		 	 *
			 *		Default
			 *			Error (ControllerGame.Update.isMenuOpen:TRUE.MenuLoadGame: UI shouldn't be receiving this kind of data)
			 */
			/*	In SCENE_Credits
			 */
			/*	In SCENE_Quit (possibly going to be kicked)
			 */
			/*	In SCENE_PlayingGame
			 *		Callback for UI not set
			 *			Error (ControllerGame.Update.isMenuOpen:TRUE.PlayingGame: UI should have had a callback set in Start)
			 *		Callback for ControllerInput not set
			 *			Error (ControllerGame.Update.isMenuOpen:TRUE.PlayingGame: ControllerInput should have had a callback set in Start)
			 *
			 */
			/*  In Testing or other
			 *
			 */
			if (messageGUI.data != ""){
				switch (messageGUI.data){
				case "unpause":
					isGamePaused = false;
					break;
				case "start":

					break;

				default:
					break;
				}
			}
			
		}

		/*isMenuOpen is FALSE*/
		if (isMenuOpen == false) {
			
		/*
		 *	Not in SCENE_PlayingGame
		 *		Error (ControllerGame.Update.isMenuOpen:FALSE.PlayingGame: There shouldn't be any circumstance that isMenuOpen is false if you aren't in SCENE_PlayingGame)
		 *	isGamePaused is TRUE
		 *
		 *	isGamePaused is FALSE
		 * 
		 */
		}
	}


	/*
	 * Events
	 */

	void EventGUI (Message message){
		messageGUI = message;
	}

	void EventCinematic (Message message){
		messageCinematic = message;
	}

	void EventInput (Message message){
		messageInput = message;
	}
}
