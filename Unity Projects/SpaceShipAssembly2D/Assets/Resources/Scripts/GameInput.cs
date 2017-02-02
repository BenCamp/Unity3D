using UnityEngine;
using System.Collections;

public class GameInput : MonoBehaviour {
	public GameState gameState;
	public UISelected uiSelected;
	public GameObject engine;
	public Camera mainCamera;

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
			if (hit) {
				if (gameState.Connecting) {
				/*	var connectee = hit.collider.gameObject;
					if (connectee != gameState.Selected && connectee.tag == "Connection") {
						gameState.Selected.GetComponent<ConnecterConnection> ().Connect (connectee);
					} else {
						Debug.Log ("Not Connectable");
					}*/
				} else {
					if ((hit.collider.gameObject != gameState.Selected) && (!hit.collider.isTrigger)) {
						Select (hit);
						uiSelected.UpdateText (hit.collider.name);
					}
				}
			}
		}

		if (Input.GetKey (KeyCode.W)) {
			engine.GetComponent<EngineThrust> ().forward ();
		}		

		if (Input.GetKey (KeyCode.A)) {
			engine.GetComponent<EngineThrust> ().turnLeft();
		}		

		if (Input.GetKey (KeyCode.D)) {
			engine.GetComponent<EngineThrust> ().turnRight();
		}
		if (!gameState.FollowCamera) {
			if (Input.GetKey (KeyCode.UpArrow)) {
				mainCamera.GetComponent<CameraControl> ().up ();
			}
			if (Input.GetKey (KeyCode.DownArrow)) {
				mainCamera.GetComponent<CameraControl> ().down ();
			}
			if (Input.GetKey (KeyCode.RightArrow)) {
				mainCamera.GetComponent<CameraControl> ().right ();
			}
			if (Input.GetKey (KeyCode.LeftArrow)) {
				mainCamera.GetComponent<CameraControl> ().left ();
			}
		}
		if (Input.GetKeyUp (KeyCode.C)) {
			if (gameState.Selected.GetComponent<StructureManager> ().CanThisConnect()) {
				gameState.Selected.GetComponent<ObjectManager> ().SpawnPlaceable ();
				uiSelected.UpdateText (uiSelected.GetText() + "\nConnectable");
			} else {
				uiSelected.UpdateText (uiSelected.GetText() + "\nNot Connectable");
			}
		}


	}

	private void Select (RaycastHit2D hit){
		gameState.Selected = hit.collider.gameObject;
	}
}
