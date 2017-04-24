using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using LitJson;

public class GameManager : MonoBehaviour {

	public static GameManager gm;
	public CameraManager cam;
	public UIManager ui;

	public bool cameraFollowSelected = true;
	private	bool paused;
	private bool placingModules;
	private	bool buildingStructure;
	private float temporaryStatusMessageTimer = 0f;
	private string temporaryStatusMessage;

	private	GameObject selected;
	public GameObject world;

	//Holds a list of the prefabs for loading
	private Dictionary <string, GameObject> preFabList= new Dictionary<string, GameObject> ();

	private List<RaycastHit2D> hits = new List<RaycastHit2D> ();

	private Collider2D[] cols;

	public 	TestingBounds test;

	//Makes sure there is only one GameManager at a time
	void Awake (){
		if (gm == null){
			DontDestroyOnLoad (gameObject);
			gm = this;
		} else if (gm != this) {
			Destroy (gameObject);
		}
	}
		
	void Start (){  
		
		//Get the other singleton managers
		cam = CameraManager.cam;
		ui = UIManager.ui;

		//Set the state of the game
		buildingStructure = false;
		cameraFollowSelected = false;
		paused = false;
		placingModules = false;

		//Get the world
		world = GameObject.Find("World");

		//TODO Create children from loaded manifest

		//For Testing
		test = GameObject.Find ("BoundsTester").GetComponent<TestingBounds> ();
	}

	void Update () {
		if (temporaryStatusMessageTimer > 0) {
			temporaryStatusMessageTimer -= Time.deltaTime;
		} else {
			temporaryStatusMessage = "";
		}

		if (selected != null) {
			ui.UpdateText ("Selected Object: " + selected.gameObject.name
			+ "\n"
			+ cam.CameraStatus ()
			+ temporaryStatusMessage);
		} else {
			ui.UpdateText (cam.CameraStatus ()
				+ temporaryStatusMessage);
		}
	}

	//Set the game camera to follow the selected GameObject
	public bool CameraFollowSelected { get { return cameraFollowSelected; } 
		set {

			//The camera isn't already following something 
			if (!cameraFollowSelected && selected != null) {
				cam.follow = selected;
				cameraFollowSelected = true;
			}

			//The camera is already following something
			else {
				cam.follow = null;
				cameraFollowSelected = false;
			}

		}
	}


	private void SetSelected (GameObject obj) {
		selected = obj;
	}



	//These functions determine the actions taken by the GameManager after
	// the InputManager registers which key/button/axis is recieved
	//TODO Add logic for other states of the game.
	public void PrimaryButtonPressed () {
		//Mouse Pointer is over a GUI element
		if (EventSystem.current.IsPointerOverGameObject ()) {
			
		}

		//Mouse Pointer is not over a GUI element
		else {
			//Something is selected
			if (selected != null) {
				cols = new Collider2D[10];
				//Adding onto a structure. Checks if in building structure mode, whether the "brush" is active
				// and enabled, and whether selected object has a StructureManager script attached to it.
				// "GetSM" Stands for Get Structure Manager
				if (buildingStructure && test.isActiveAndEnabled && GetSM (selected) != null) {
					test.GetComponent<PolygonCollider2D> ().OverlapCollider (new ContactFilter2D (), cols);

					//Checks through the 
					foreach (Collider2D col in cols) {
						if (col != null && col.gameObject == selected) {
							Vector2[] temp = test.OnClick ();
							GetSM (selected).StructureAdd (temp);
							break;
						}
					}
				} 

				//Select something else or the same thing
				else {
					SetSelected ();
				}
			}

			//Nothing is selected
			else {
				SetSelected ();
			}
		}
	}
	public void SecondaryButttonPressed () {

	}
	public void UpCamButtonPressed () {

	}
	public void DownCamButtonPressed () {

	}
	public void LeftCamButtonPressed () {

	}
	public void RightCamButtonPressed () {

	}
	public void ForwardButtonPressed () {

	}
	public void ReverseButtonPressed () {

	}
	public void LeftButtonPressed () {

	}
	public void RightButtonPressed () {

	}


	//
	//Utility functions
	//

	//Returns the StructureManager of an object if it exists or null otherwise
	// "GetSM" Stands for Get Structure Manager
	private StructureManager GetSM (GameObject obj){
		if (obj.GetComponent<StructureManager> () != null) {
			return obj.GetComponent<StructureManager> ();
		}
		return null;
	}
		
	//Check and see if the user has selected something 
	// or clicked in empty space to deselect
	private void SetSelected () {

		//Check all the colliders under the pointer
		foreach (RaycastHit2D hit in cam.MousePoint ()){

			//The collider isn't the "brush"
			if (hit.collider.gameObject.name != "BoundsTester")
				hits.Add(hit);
		}

		//Nothing was hit
		if (hits.Count == 0) {
			selected = null;
		} 

		//Something that wasn't a GUI or the "brush" was hit
		else {
			selected = hits[0].collider.gameObject;
		}

		//Reset the 'hits' list
		hits.Clear ();
	}

	//TODO Build a better brush object for building
	//Toggle between building and not building
	//Also enables and disables the brush for building
	//For now called test
	public void SetBuilding () {
			
		//Already in the building structure state
		if (buildingStructure) {
			temporaryStatusMessage += "\nNo longer Building";
			temporaryStatusMessageTimer = 5f;
			test.Disable ();
			buildingStructure = false;
		} else if (placingModules || selected == null){

		} else {
			temporaryStatusMessage += "\nBuilding";
			temporaryStatusMessageTimer = 5f;
			test.Enable ();
			buildingStructure = true;
		}
	}

	//TODO 
	//For now, I'm doing it like this for the sake of simplicity.
	//If at all possible, I need to find a way to do this without grabbing EVERYTHING...
	public void Save (){

		GameData data = new GameData ();

		//Get GameManager's state
		data.cameraFollowSelected 	= cameraFollowSelected;
		data.paused 				= paused;
		data.placingModules 		= placingModules;
		data.buildingStructure 		= buildingStructure;
		data.temporaryStatusMessage = temporaryStatusMessage;


		//Save the World!
		data.world = Deflator.Deflate(world);


		//Destroy The WORLD!!
		if (world.gameObject.transform.childCount > 0) {
			for (int i = 0; i < world.gameObject.transform.childCount; i++) {
				Destroy (world.gameObject.transform.GetChild (i).gameObject);
			}
		}
		Destroy (world);


		//Turn all data into Json string
		JsonData jsonWorld = JsonMapper.ToJson (data);
		Debug.Log (jsonWorld.ToString());
		File.WriteAllText (Application.persistentDataPath + "/gameInfo.dat", jsonWorld.ToString());


	}

	//TODO Inflator
	public void Load () {
		/*
		if (File.Exists (Application.persistentDataPath + "/gameInfo.dat")) {
			//Inflator inflator = new Inflator ();

			JsonData jsonWorld = JsonMapper.ToObject (File.ReadAllText(Application.persistentDataPath + "/gameInfo.dat"));				
			GameData data = jsonWorld.<GameData>(beforeJson);


			//Set GameManager's state
			cameraFollowSelected 	= data.cameraFollowSelected;
			paused 					= data.paused;
			placingModules 			= data.placingModules;
			buildingStructure 		= data.buildingStructure;
			temporaryStatusMessage 	= data.temporaryStatusMessage;
			world 					= Inflator.Inflate(data.world);
		}
		*/
	}
}


//
//Serializable Classes for saving and loading game data
//

[Serializable]
class GameData
{
	public bool cameraFollowSelected;
	public	bool paused;
	public bool placingModules;
	public	bool buildingStructure;

	//Reset the length of the temporaryStatusMessage after loading
	public string temporaryStatusMessage;
	public SaveableObject world;

}