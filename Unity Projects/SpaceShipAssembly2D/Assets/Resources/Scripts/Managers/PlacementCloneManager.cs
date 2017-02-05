

using UnityEngine;
using System.Collections.Generic;

public class PlacementCloneManager : MonoBehaviour {
	public GameState gameState;

	private GameObject spawner;
	public GameObject Spawner { get { return spawner; } set { spawner = value; } }

	private bool hitOpenConnection = false;
	public bool HitOpenConnection { get { return hitOpenConnection; } }

	public Dictionary <int, GameObject> connectors = new Dictionary<int, GameObject>();
	private float distanceToConnector = Mathf.Infinity;
	private int keyOfClosest = -1;
	private CircleCollider2D circCollider;
	private GameObject [] cloneConnections = new GameObject [4];
	private GameObject cloneConnectionPrefab;
	private int rotationModifier = 0;

	void Start () {
		gameState = GameObject.Find ("GameManager").GetComponent<GameState> ();
		gameState.Connecting = true;
		circCollider = gameObject.GetComponent<CircleCollider2D> ();
		cloneConnectionPrefab = Resources.Load ("Prefabs/PlacementClonePrefabs/ConnectionClone") as GameObject;
		//Wait for the spawner to set itself as the spawner
		while (spawner == null);
		BuildCloneConnections (spawner.GetComponent<StructureManager> ().ConnectionList);
		BuildCloneComponents ();
	}

	// Update is called once per frame
	void Update () {

		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		mousePos.z = 0;
		if (!hitOpenConnection) {
			gameObject.transform.position = mousePos;
			gameObject.transform.rotation = Quaternion.identity;
		} else {

			//Reset the connector distance
			//Needed because the connector was getting stuck
			//Each new connection the clone would jump to had to be closer to the mouse then the last without this
			distanceToConnector = Mathf.Infinity;

			//Check each connector 
			foreach (int key in connectors.Keys) {
				
				//Find the nearest open connector
				float distanceTemp = Vector3.Distance (mousePos, connectors [key].transform.position);
				if (distanceTemp < distanceToConnector) {
					distanceToConnector = distanceTemp;

					//Place the clone on the connector's position and give it the connector's parent's rotation
					gameObject.transform.position = connectors[key].transform.position;
					gameObject.transform.rotation = connectors [key].transform.parent.transform.rotation;

					//Have the north side pointing toward the center of the connectee structure
					switch (connectors [key].name) {
					case "NorthConnection":
						transform.Rotate (0, 0, 180 + GetConnectorRotationOffset());
						break;
					case "EastConnection":
						transform.Rotate (0, 0, 90 + GetConnectorRotationOffset());
						break;
					case "SouthConnection":
						transform.Rotate (0, 0, 0 + GetConnectorRotationOffset());
						break;
					case "WestConnection":
						transform.Rotate (0, 0, 270 + GetConnectorRotationOffset());
						break;
					default:
						break;
					}

				}
			}
			//Offset the CircleCollider2D so it is on the Mouse
			circCollider.offset = 
				new Vector2 (
					transform.InverseTransformPoint(mousePos).x,
					transform.InverseTransformPoint(mousePos).y);
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.transform.tag == "Connection"){
			hitOpenConnection = true;
			if (other.GetComponent<ConnecterConnection>().Connected == null){
				connectors.Add(other.GetInstanceID(), other.gameObject);
			}
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		connectors.Remove (other.GetInstanceID());
		if (connectors.Count == 0) {
			hitOpenConnection = false;
			gameObject.GetComponent<CircleCollider2D> ().offset = Vector3.zero;
		}
	}

	private void BuildCloneConnections(Vector4 connections) {
		if (connections.x != 0) {
			Vector3 pos = new Vector3 (0, 11f, 0);
			cloneConnections [0] = (GameObject) Instantiate (cloneConnectionPrefab, pos, Quaternion.identity);
			cloneConnections [0].transform.parent = gameObject.transform;
			cloneConnections [0].name = "NorthConnectionClone";
		}
		if (connections.y != 0) {
			Vector3 pos = new Vector3 (11f, 0, 0);
			cloneConnections [1] = (GameObject) Instantiate (cloneConnectionPrefab, pos, Quaternion.identity);
			cloneConnections [1].transform.parent = gameObject.transform;
			cloneConnections [1].name = "EastConnectionClone";
		}
		if (connections.z != 0) {
			Vector3 pos = new Vector3 (0, -11f, 0);
			cloneConnections [2] = (GameObject) Instantiate (cloneConnectionPrefab, pos, Quaternion.identity);
			cloneConnections [2].transform.parent = gameObject.transform;
			cloneConnections [2].name = "SouthConnectionClone";
		}
		if (connections.w != 0) {
			Vector3 pos = new Vector3 (-11f, 0, 0);
			cloneConnections [3] = (GameObject) Instantiate (cloneConnectionPrefab, pos, Quaternion.identity);
			cloneConnections [3].transform.parent = gameObject.transform;
			cloneConnections [3].name = "WestConnectionClone";
		}
	}

	private void BuildCloneComponents(){
		var componentArray = spawner.GetComponentsInChildren(typeof(Component));
		foreach (Component element in componentArray) {
			Debug.Log (element);
			if (element.GetComponent<CloneReplacer>() != null) {
				element.GetComponent<CloneReplacer> ().Replace (gameObject.transform).transform.parent = gameObject.transform;
			}
		}
	}
	public void ConnectorRotationOffset(int change){
		bool done = false;
		while (!done) {
			if (change == 1) {
				//If the rotationModifier returns to zero, then there is only 1 connector
				//We Won't need to change the rotation
				if (rotationModifier == 3) {
					rotationModifier = 0;
					done = true;
				} else {
					//Increment Rotation Modifier
					rotationModifier++;
				}

				//Check if the current array item has a ConnectorClone
				if (cloneConnections [rotationModifier] != null)
					done = true;
			} else if (change == -1) {
				if (rotationModifier == 1) {
					rotationModifier = 0;
					done = true;
				} else if (rotationModifier == 0) {
					rotationModifier = 3;
				} else {
					rotationModifier--;
				}

				if (cloneConnections [rotationModifier] != null)
					done = true;
			} else {
				Debug.Log ("Error: unknown command for PlacementClone!");
			}
		}
	}

	private int GetConnectorRotationOffset (){
		switch (rotationModifier) {
		case 0:
			return 0;
			break;
		case 1:
			return 90;
			break;
		case 2:
			return 180;
			break;
		case 3:
			return 270;
			break;
		default:
			return 0;
			break;
		}
	}
}