using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	private bool 			cameraFollowSelected;
	private	bool			buildingStructure;
	private	bool 			paused;
	private bool 			placingModules;

	private	GameObject 		selected;

	private RaycastHit2D 	hit = new RaycastHit2D ();

	public 	TestingBounds 	test;
	public 	UIManager 		uiManager;
	public	CameraManager 	cameraManager;



	void Start (){
		cameraManager = gameObject.GetComponent<CameraManager> ();
		uiManager = GameObject.Find ("UI").GetComponent<UIManager> ();
		buildingStructure = false;
		cameraFollowSelected = false;
		paused = false;
		placingModules = false;
		test = GameObject.Find ("BoundsTester").GetComponent<TestingBounds> ();
	}

	void Update () {
		if (selected != null) {
			uiManager.UpdateText ("Selected Object: " + selected.gameObject.name
			+ "\n"
			+ cameraManager.CameraStatus ());
		} else {
			uiManager.UpdateText (cameraManager.CameraStatus ());
		}
	}

	//Set the game camera to follow the selected GameObject
	public bool CameraFollowSelected { get { return cameraFollowSelected; } 
		set {

			//The camera isn't already following something 
			if (!cameraFollowSelected && selected != null) {
				cameraManager.follow = selected;
				cameraFollowSelected = true;
			}

			//The camera is already following something
			else {
				cameraManager.follow = null;
				cameraFollowSelected = false;
			}

		}
	}

	public bool BuildingStructure { get { return buildingStructure; } 
		set {

			//Already in the building structure state
			if (buildingStructure) {
				test.Disable ();
				buildingStructure = false;
			} else {
				test.Enable ();
				buildingStructure = true;
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
		//Something is selected
		if (selected != null) {
			
			//Adding onto a structure 
			// "GetSM" Stands for Get Structure Manager
			if (buildingStructure && test.isActiveAndEnabled && GetSM(selected) != null) {
				Vector2[] temp = test.OnClick ();
				GetSM(selected).StructureAdd (temp);
			}
				
			SetSelected ();
		}

		//Nothing is selected
		else {
			SetSelected ();
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
		hit = cameraManager.MousePoint ();
		if (hit.collider == null) {
			selected = null;
		} else {
			selected = hit.collider.gameObject;
		}
	}
}
