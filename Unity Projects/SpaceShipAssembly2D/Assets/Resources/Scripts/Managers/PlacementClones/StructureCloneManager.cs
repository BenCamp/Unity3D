using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureCloneManager : CloneManager {


	public Dictionary <int, GameObject> connectors = new Dictionary<int, GameObject>();
	private float distanceToConnector = Mathf.Infinity;
	private int keyOfClosest = -1;
	public GameObject [] cloneConnectors = new GameObject [4];
	private GameObject cloneConnectorPrefab;

	private Vector3 translation;
	private Vector3 eulerAngles;
	private Vector3 scale = new Vector3 (1,1,1);

	void Start () {
		CommonCloneSetup ();
		circCollider.radius = 30;
	}

	// Update is called once per frame
	void Update () {

		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		mousePos.z = 0;
		if (!canPlace) {
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

					//Have the north side pointing toward the center of the connectee structure then change for rotation
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

			canPlace = true;
			if (other.tag == "Connector"){
				connectors.Add(other.GetInstanceID(), other.gameObject);
			}
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		connectors.Remove (other.GetInstanceID());
		if (connectors.Count == 0) {
			canPlace = false;
			gameObject.GetComponent<CircleCollider2D> ().offset = Vector3.zero;
		}
	}

	public override void AttemptToAttachClone(){
		if (canPlace) {
			spawner.transform.position = transform.position;
			spawner.transform.rotation = transform.rotation;
			spawner.gameObject.GetComponent<StructureManager>().SetConnection();
			gameState.Placing = false;
			KillClone ();
		}
	}

	public override void ConnectorRotationOffset(int change){
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
				if (cloneConnectors [(int)rotationModifier] != null)
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

				if (cloneConnectors [(int)rotationModifier] != null)
					done = true;
			} else {
				Debug.Log ("Error: unknown command for PlacementClone!");
			}
		}
	}

	public void SetClone (int pos, GameObject con){
		cloneConnectors [pos] = con;
	}

	private int GetConnectorRotationOffset (){
		switch ((int) rotationModifier) {
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