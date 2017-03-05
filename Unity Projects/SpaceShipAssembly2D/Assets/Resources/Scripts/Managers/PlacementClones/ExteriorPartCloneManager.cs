using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExteriorPartCloneManager : CloneManager {
	public int rotationRate = 10;
	public Dictionary <int, GameObject> walls = new Dictionary<int, GameObject>();
	private float distanceToWall = Mathf.Infinity;
	private int keyOfClosest = -1;
	private int rotationModifier = 0;

	private Vector3 eulerAngles;
	private Vector3 scale = new Vector3 (1,1,1);

	public float testX;
	public float testY;
	public float testZ;
	void Start () {
		CommonCloneSetup ();
		circCollider.radius = 20f;
	}


	void Update () {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		mousePos.z = 0;
		if (!canPlace) {
			gameObject.transform.position = mousePos;
			gameObject.transform.rotation = Quaternion.identity;
		} else {
			//Reset the wall distance
			//Needed because the wall was getting stuck
			//Each new Wall the clone would jump to had to be closer to the mouse then the  last without this
			distanceToWall = Mathf.Infinity;

			//Check each connector 
			foreach (int key in walls.Keys) {

				//Find the nearest open wall
				float distanceTemp = Vector3.Distance (mousePos, walls [key].transform.position);
				if (distanceTemp < distanceToWall) {
					distanceToWall = distanceTemp;
					//Calculate position
					Vector3 pos = walls[key].transform.position;
					pos += walls [key].transform.up * 10;
					transform.rotation = Quaternion.Euler (0, 0, walls [key].transform.rotation.eulerAngles.z + rotationModifier);
					transform.position = pos;

					keyOfClosest = key;
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
		if (other.transform.tag == "Wall" 
			&& (other.transform.parent.GetInstanceID() != spawner.transform.GetInstanceID())
			&& (other.transform.parent.parent != null)
			&& (other.transform.parent.parent.name == "PlayerShip"))
		{

			canPlace = true;
			walls.Add(other.GetInstanceID(), other.gameObject);

		}
	}

	void OnTriggerExit2D (Collider2D other) {
		walls.Remove (other.GetInstanceID());
		if (walls.Count == 0) {
			canPlace = false;
			gameObject.GetComponent<CircleCollider2D> ().offset = Vector3.zero;
		}
	}
	public override void AttemptToAttachClone(){
		if (canPlace) {
			spawner.transform.position = transform.position;
			spawner.transform.rotation = transform.rotation;
			spawner.transform.parent = walls [keyOfClosest].transform.parent;
			spawner.GetComponent<ObjectManager>().SetShip (spawner.GetComponentInParent<ShipManager> ());
			spawner.gameObject.AddComponent <FixedJoint2D> ();
			spawner.GetComponent <FixedJoint2D> ().connectedBody = spawner.transform.parent.GetComponent<Rigidbody2D> ();
			spawner.GetComponent<ObjectManager> ().StateChange ();
			gameState.Placing = false;
			KillClone ();
		}
	}

	public override void ConnectorRotationOffset (int change){
		if (change == -1) {
			if (rotationModifier > 0) {
				rotationModifier += -1*rotationRate;
			} else {
				rotationModifier = 359;
			}
		} else if (change == 1) {
			if (rotationModifier < 360) {
				rotationModifier += 1*rotationRate;
			} else {
				rotationModifier = 0;
			}
		} else {
			Debug.Log ("Error: unknown command for ExteriorPlacementClone!");
		}
	}

	private int GetConnectorRotationOffset (){
		return 0;
	}
}
