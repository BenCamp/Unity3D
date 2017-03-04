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

	void Start (){
		placement = "Structure";
		objectName = "Structure";
		IsSelectable = true;
		connectorList = new Vector4 (1, 1, 1, 1);
		wall = Resources.Load ("Prefabs/Objects/Integrity/Wall") as GameObject;
		floor = Resources.Load ("Prefabs/Objects/Integrity/Floor")as GameObject;
		connector = Resources.Load ("Prefabs/Placement/Connector")as GameObject;
	
		gameState = FindObjectOfType<GameState> ();
	}

	public override void StateChange() {
	}

	public override bool CanThisBePlaced () {
		if (transform.parent != null && transform.parent.name == "PlayerShip") {
			return false;
		} else {
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
		}
		return false;
	}

	public void SetConnection (){
		//Get the Connectors atached to structures attached to the player-ship that falls in the center of this structure
		//For each Connector of this structure, check if they fall in the center of a structure from the previous bunch of connectors
		//Delete any connector that satisfies both, and set each gameobject in the others respective connections array
		var listOfCollisions = Physics2D.OverlapBoxAll (transform.position, gameObject.GetComponent<BoxCollider2D> ().size, gameObject.transform.rotation.z);
		foreach (Collider2D col in listOfCollisions) {
			if (col.tag == "Connector") {
				GameObject colParent = col.transform.parent.gameObject;
				if (colParent.transform.parent != null && colParent.transform.parent.name == "PlayerShip") {
					var result = IsOneOfMyConnectorsOnTheStructureAttachedToThePlayerShip (colParent);
					if (result != -1) {
						gameObject.AddComponent <FixedJoint2D> ().connectedBody = colParent.GetComponent<Rigidbody2D> ();
						gameObject.transform.parent = colParent.transform.parent;
						if (colParent.GetComponent<StructureManager> ().SetConnectionToOtherStructure (gameObject, col)) {
							Destroy (connectors [result].gameObject);
							connectors [result] = colParent;
						} else {
							Debug.Log ("SetConnectionToOtherStructure returned false!");
						}
					} else {
						Debug.Log ("Result came back as -1!");
					}
				}
			}
		}
		gameState.Placing = false;
	}

	private int IsOneOfMyConnectorsOnTheStructureAttachedToThePlayerShip(GameObject colParent){
		int count = 0;
		if (ConnectorList.x == 1) {
			if (CheckPosition (count, colParent))
				return count;
		}
		count++;
		if (ConnectorList.y == 1) {
			if (CheckPosition (count, colParent))
				return count;
		}
		count++;
		if (ConnectorList.z == 1) {
			if (CheckPosition (count, colParent))
				return count;
		}
		count++;
		if (ConnectorList.w == 1) {
			if (CheckPosition (count, colParent))
				return count;
		}
		return -1;
	}

	private bool CheckPosition (int count, GameObject colParent){
		if (connectors [count].tag == "Connector") {
			var hits = Physics2D.CircleCastAll (connectors [count].transform.position, connectors [count].GetComponent<CircleCollider2D> ().radius, Vector2.zero);
			foreach (RaycastHit2D hit in hits) {
				if (hit.collider.gameObject == colParent) {
					return true;
				}
			}
		}
		return false;
	}

	public bool SetConnectionToOtherStructure(GameObject other, Collider2D con){
		if (ConnectorList.x == 1) {
			if (connectors [0] != null && con.gameObject.GetInstanceID() == connectors[0].gameObject.GetInstanceID()) {
				Destroy (connectors [0].gameObject);
				connectors [0] = other;
				return true;
			}
		}
		if (ConnectorList.y == 1) {
			if (connectors [1] != null && con.gameObject.GetInstanceID () == connectors [1].gameObject.GetInstanceID ()) {
				Destroy (connectors [1].gameObject);
				connectors [1] = other;
				return true;
			}
		}
		if (ConnectorList.z == 1) {
			if (connectors [2] != null && con.gameObject.GetInstanceID () == connectors [2].gameObject.GetInstanceID ()) {
				Destroy (connectors [2].gameObject);
				connectors [2] = other;
				return true;
			
			}
		}
		if (ConnectorList.w == 1) {
			if (connectors [3] != null && con.gameObject.GetInstanceID () == connectors [3].gameObject.GetInstanceID ()) {
				Destroy (connectors [3].gameObject);
				connectors [3] = other;
				return true;
			}
		}
		return false;
	}
}