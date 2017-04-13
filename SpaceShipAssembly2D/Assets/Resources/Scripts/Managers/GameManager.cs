


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class GameManager : MonoBehaviour {

	private bool cameraFollowSelected;
	private	bool paused;
	private bool placingModules;
	private	bool buildingStructure;
	private float temporaryStatusMessageTimer = 0f;
	private string temporaryStatusMessage;

	private	GameObject selected;

	private List<RaycastHit2D> hits = new List<RaycastHit2D> ();

	private Collider2D[] cols;

	public 	TestingBounds test;
	public 	UIManager uiManager;
	public	CameraManager cameraManager;



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
		if (temporaryStatusMessageTimer > 0) {
			temporaryStatusMessageTimer -= Time.deltaTime;
		} else {
			temporaryStatusMessage = "";
		}

		if (selected != null) {
			uiManager.UpdateText ("Selected Object: " + selected.gameObject.name
			+ "\n"
			+ cameraManager.CameraStatus ()
			+ temporaryStatusMessage);
		} else {
			uiManager.UpdateText (cameraManager.CameraStatus ()
				+ temporaryStatusMessage);
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
		foreach (RaycastHit2D hit in cameraManager.MousePoint ()){

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
}
