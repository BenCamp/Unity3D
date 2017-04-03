using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	private bool 		cameraFollowSelected;
	private	bool buildingStructure;
 	
	public 	bool 		paused 				{ get ; set; }
	public 	bool 		placingModules 		{ get ; set; }
	public 	GameObject 	selected 			{ get ; set; }

	//Set the game camera to follow the selected GameObject
	//Essentially a toggle
	public bool CameraFollowSelected { get { return cameraFollowSelected; } 
		set {

			//The camera isn't already following something
			if (!cameraFollowSelected && selected != null)
				cameraFollowSelected = true;

			//The camera is already following something
			else {
				cameraFollowSelected = false;
			}
		} 
	}

	public bool BuildingStructure { get { return buildingStructure; } 
		set { 


		} 
	}


	void Start (){
		buildingStructure = false;
		cameraFollowSelected = false;
		paused = false;
		placingModules = false;
	}
}
