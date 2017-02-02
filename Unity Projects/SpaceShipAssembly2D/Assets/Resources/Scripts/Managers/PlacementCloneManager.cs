using UnityEngine;
using System.Collections.Generic;

public class PlacementCloneManager : MonoBehaviour {

	public GameObject spawner;
	public GameObject Spawner { get { return spawner; } set { spawner = value; } }
	private bool hit = false;
	private Dictionary <int, Vector3> connectors = new Dictionary<int, Vector3>();
	private float distanceToConnector = Mathf.Infinity;
	private int keyOfClosest = -1;
	private CircleCollider2D circCollider;

	void Start () {
		circCollider = gameObject.GetComponent<CircleCollider2D> ();
	}

	// Update is called once per frame
	void Update () {

		Vector3 newPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		newPos.z = 0;
		if (!hit) {
			gameObject.transform.position = newPos;
		} else {
			distanceToConnector = Mathf.Infinity;
			foreach (int key in connectors.Keys) {
				//Find the nearest open connector
				float distanceTemp = Vector3.Distance (newPos, connectors [key]);
				if (distanceTemp < distanceToConnector) {
					distanceToConnector = distanceTemp;
					gameObject.transform.position = connectors [key];

					//Offset the CircleCollider2D so it is on the Mouse
					circCollider.offset = new Vector2 (newPos.x - gameObject.transform.position.x, newPos.y - gameObject.transform.position.y);
				}
			}
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.transform.tag == "Connection"){
			hit = true;
			if (other.GetComponent<ConnecterConnection>().Connected == null){
				connectors.Add(other.GetInstanceID(), other.transform.position);
			}
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		connectors.Remove (other.GetInstanceID());
		if (connectors.Count == 0) {
			hit = false;
			gameObject.GetComponent<CircleCollider2D> ().offset = Vector3.zero;
		}
	}
}