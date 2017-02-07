

using UnityEngine;
using System.Collections.Generic;

public class PlacementCloneManager : MonoBehaviour {
	public GameState gameState;

	private GameObject spawner;
	public GameObject Spawner { get { return spawner; } set { spawner = value; } }

	private bool hitOpenConnector = false;
	public bool HitOpenConnector { get { return hitOpenConnector; } }

	public Dictionary <int, GameObject> connectors = new Dictionary<int, GameObject>();
	private float distanceToConnector = Mathf.Infinity;
	private int keyOfClosest = -1;
	private CircleCollider2D circCollider;
	private GameObject [] cloneConnectors = new GameObject [4];
	private GameObject cloneConnectorPrefab;
	private int rotationModifier = 0;

	void Start () {
		gameState = GameObject.Find ("GameManager").GetComponent<GameState> ();
		circCollider = gameObject.GetComponent<CircleCollider2D> ();
		cloneConnectorPrefab = Resources.Load ("Prefabs/PlacementClonePrefabs/ConnectorClone") as GameObject;
		//Wait for the spawner to set itself as the spawner
		while (spawner == null);
		BuildCloneConnectors (spawner.GetComponent<StructureManager> ().ConnectorList);
		BuildCloneComponents ();
	}

	// Update is called once per frame
	void Update () {

		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		mousePos.z = 0;
		if (!hitOpenConnector) {
			gameObject.transform.position = mousePos;
			gameObject.transform.rotation = Quaternion.identity;
		} else {

			//Reset the connector distance
			//Needed because the connector was getting stuck
			//Each new Connector the clone would jump to had to be closer to the mouse then the last without this
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
					case "NorthConnector":
						transform.Rotate (0, 0, 180 + GetConnectorRotationOffset());
						break;
					case "EastConnector":
						transform.Rotate (0, 0, 90 + GetConnectorRotationOffset());
						break;
					case "SouthConnector":
						transform.Rotate (0, 0, 0 + GetConnectorRotationOffset());
						break;
					case "WestConnector":
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
		if (other.transform.tag == "Connector" 
			&& (other.transform.parent.GetInstanceID() != spawner.transform.GetInstanceID())
			&& (other.transform.parent.parent != null)
			&& (other.transform.parent.parent.name == "PlayerShip")){

			hitOpenConnector = true;
			if (other.tag == "Connector"){
				connectors.Add(other.GetInstanceID(), other.gameObject);
			}
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		connectors.Remove (other.GetInstanceID());
		if (connectors.Count == 0) {
			hitOpenConnector = false;
			gameObject.GetComponent<CircleCollider2D> ().offset = Vector3.zero;
		}
	}

	private void BuildCloneConnectors(Vector4 Connectors) {
		if (Connectors.x != 0) {
			Vector3 pos = new Vector3 (0, 11f, 0);
			cloneConnectors [0] = (GameObject) Instantiate (cloneConnectorPrefab, pos, Quaternion.identity);
			cloneConnectors [0].transform.parent = gameObject.transform;
			cloneConnectors [0].name = "NorthConnectorClone";
		}
		if (Connectors.y != 0) {
			Vector3 pos = new Vector3 (11f, 0, 0);
			cloneConnectors [1] = (GameObject) Instantiate (cloneConnectorPrefab, pos, Quaternion.identity);
			cloneConnectors [1].transform.parent = gameObject.transform;
			cloneConnectors [1].name = "EastConnectorClone";
		}
		if (Connectors.z != 0) {
			Vector3 pos = new Vector3 (0, -11f, 0);
			cloneConnectors [2] = (GameObject) Instantiate (cloneConnectorPrefab, pos, Quaternion.identity);
			cloneConnectors [2].transform.parent = gameObject.transform;
			cloneConnectors [2].name = "SouthConnectorClone";
		}
		if (Connectors.w != 0) {
			Vector3 pos = new Vector3 (-11f, 0, 0);
			cloneConnectors [3] = (GameObject) Instantiate (cloneConnectorPrefab, pos, Quaternion.identity);
			cloneConnectors [3].transform.parent = gameObject.transform;
			cloneConnectors [3].name = "WestConnectorClone";
		}
	}

	private void BuildCloneComponents(){
		foreach (Transform child in spawner.transform) {
			if (child.GetComponent<CloneReplacer>() != null) {
				Transform offset = child.GetComponent<CloneReplacer> ().GetPartTransform ();
				GameObject clone = child.GetComponent<CloneReplacer> ().Replace (gameObject.transform);
				clone.transform.parent = gameObject.transform;
				clone.transform.Translate (offset.localPosition);
				clone.transform.rotation = offset.localRotation;
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
				if (cloneConnectors [rotationModifier] != null)
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

				if (cloneConnectors [rotationModifier] != null)
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

	public void CreateConnectors (){
		spawner.transform.position = transform.position;
		spawner.transform.rotation = transform.rotation;
		spawner.GetComponent<StructureManager>().SetConnections ();
		Destroy (gameObject);
	}
}