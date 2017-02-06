using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StructureManager : ObjectManager {

	public GameState gameState;

	public GameObject wall;
	public GameObject floor;
	public GameObject connector;

	public GameObject[] walls = new GameObject[12];
	public GameObject[] floors = new GameObject[9];
	public GameObject[] connectors = new GameObject[4];

	private Vector4 connectorList;

	public Vector4 ConnectorList { get { return connectorList; } }

	private GameObject[] shipConnectorsIntersectingWithThisStructure = new GameObject[4];

	void Start (){
		IsSelectable = true;
		connectorList = new Vector4 (1, 1, 1, 1);
		wall = Resources.Load ("Prefabs/Structure/Wall") as GameObject;
		floor = Resources.Load ("Prefabs/Structure/Floor")as GameObject;
		connector = Resources.Load ("Prefabs/ShipParts/Connector")as GameObject;

		gameState = FindObjectOfType<GameState> ();
	}

	public bool CanThisConnect () {
		if (gameState.Selected.transform.parent != null) {
			if (gameState.Selected.transform.parent.name == "PlayerShip")
				return false;
		}

		if (ConnectorList.x == 1) {
			if (connectors [0].tag == "Connector")
				return true;
		}
		if (ConnectorList.y == 1) {
			if (connectors [1].tag == "Connector")
				return true;
		}
		if (ConnectorList.z == 1) {
			if (connectors [2].tag == "Connector")
				return true;
		}
		if (ConnectorList.w == 1) {
			if (connectors [3].tag == "Connector")
				return true;
		}
		return false; 
			
	}

	public GameObject SpawnPlaceable () {
		GameObject spawn = 
			Instantiate (
				Resources.Load (
					"Prefabs/PlacementClonePrefabs/PlacementClone"), 
				Vector3.zero, 
				Quaternion.identity) 
			as GameObject;
		spawn.GetComponent <PlacementCloneManager> ().Spawner = gameObject;
		return spawn;
	}

	public void SetConnections (){
		//TODO create the SetConnections 
		//Get the Connectors atached to structures attached to the player-ship that falls in the center of this structure
		//For each Connector of this structure, check if they fall in the center of a structure from the previous bunch of connectors
		//Delete any connector that satisfies both, and set each gameobject in the others respective connections array
		//Create the connection object between the two structures




		//Probably not useful
		/*
		CircleCollider2D collider;
		if (ConnectorList.x == 1) {
			if (connectors [0].tag == "Connector") {
				collider = connectors [0].GetComponent<CircleCollider2D> ();
				var listOfCollisions = Physics2D.OverlapCircleAll (transform.position, gameObject.GetComponent<CircleCollider2D> ().radius);
				foreach (Collider2D col in listOfCollisions) {
					if (collider.transform.parent != null
						&& collider.transform.parent.name == "PlayerShip") {
					}
				}
			}
		}
		if (ConnectorList.y == 1) {
			if (connectors [1].tag == "Connector") {
			}
		}
		if (ConnectorList.z == 1) {
			if (connectors [2].tag == "Connector") {
			}
		}
		if (ConnectorList.w == 1) {
			if (connectors [3].tag == "Connector") {
			}
		}
*/
		gameState.Connecting = false;
	}

	//TODO Create class that removes connector of a structure if it is the connectee
}