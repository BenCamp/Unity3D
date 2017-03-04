using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteriorPartCloneManager : CloneManager {
	protected Transform currentParent;

	protected Dictionary <int, GameObject> onTopOfList = new Dictionary <int, GameObject> ();
	public int rotationSpeed = 10;

	void Start () {
		CommonCloneSetup ();
	}

	void Update(){
		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		mousePos.z = 0;
		transform.position = mousePos;

	}

	void OnTriggerEnter2D (Collider2D other) {
		onTopOfList.Add(other.GetInstanceID(), other.gameObject);
	}

	void OnTriggerExit2D (Collider2D other) {
		onTopOfList.Remove (other.GetInstanceID());
	}

	public override void ConnectorRotationOffset (int change){
		transform.Rotate (new Vector3 (0, 0, change * rotationSpeed * Time.deltaTime));
	}

	public override void AttemptToAttachClone (){
		canPlace = true;
		var hits = onTopOfList.Values.ToArray ();

		if (hits.Length == 0)
			canPlace = false;
		
		//TODO Find a way to make this more generic, some sort of "CanBePlacedOn" tag might do the trick, but I'd need to deal with the structure element
		foreach (GameObject hit in hits) {
			if (hit.tag != "Floor" && hit.tag != "Structure") {
				Debug.Log ("Not Floor or Not Structure");
				canPlace = false;
				break;
			}
		}


		if (canPlace) {
			spawner.transform.position = transform.position;
			spawner.transform.parent = hits[0].transform.parent;
			gameState.Placing = false;
			KillClone ();
		}
	}
}
