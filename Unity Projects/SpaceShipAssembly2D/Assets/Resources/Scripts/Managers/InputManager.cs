using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
	public GameState gameState;
	public UIManager uiManager;
	public CloneManager cloneManager;
	public ShipManager shipManager;
	public Camera mainCamera;

	void Start () {
		shipManager = GameObject.Find ("PlayerShip").GetComponent<ShipManager>();
		uiManager = GameObject.Find ("UI").GetComponent<UIManager> ();
	}

	// Update is called once per frame
	void Update () {

		//Dealing with mouse actions
		if (Input.GetMouseButtonDown (0)) {
			if (gameState.Placing) {
				cloneManager.AttemptToAttachClone ();
			} else if (gameState.Paused) {
			} else {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity);
				gameState.Selected = null;
				foreach (RaycastHit2D hit in hits){
					if ((hit.collider.gameObject != gameState.Selected) && (hit.collider.gameObject.GetComponent<ObjectManager> () != null) &&(hit.collider.gameObject.GetComponent<ObjectManager> ().IsSelectable)) {
						Select (hit);
						uiManager.UpdateText (hit.collider.name);
						break;
					}
				}
			}
		}

		if (Input.GetKey (KeyCode.Escape)) {
			if (gameState.Placing) {
				gameState.Placing = false;
				cloneManager.KillClone ();
			}
		}

		//Dealing with engine controls
		if (Input.GetKey (KeyCode.W)) {
			if (gameState.Placing) {
			}
			shipManager.ForwardCommandGiven ();
		}		

		if (Input.GetKey (KeyCode.A)) {
			if (gameState.Placing) {
			}
			shipManager.LeftCommandGiven ();
		}		

		if (Input.GetKey (KeyCode.D)) {
			if (gameState.Placing) {
			}
			shipManager.RightCommandGiven ();
		}

		if (!gameState.FollowCamera) {
			if (Input.GetKey (KeyCode.UpArrow)) {
				mainCamera.GetComponent<CameraManager> ().up ();
			}
			if (Input.GetKey (KeyCode.DownArrow)) {
				mainCamera.GetComponent<CameraManager> ().down ();
			}
			if (Input.GetKey (KeyCode.RightArrow)) {
				mainCamera.GetComponent<CameraManager> ().right ();
			}
			if (Input.GetKey (KeyCode.LeftArrow)) {
				mainCamera.GetComponent<CameraManager> ().left ();
			}
		}

		//Dealing with PlacementClones
		if (Input.GetKeyDown (KeyCode.C)) {
			if (gameState.Selected != null) {
				if (!gameState.Placing) {
					if (gameState.Selected.GetComponent<ObjectManager> ().CanThisBePlaced ()) {
						cloneManager = gameState.Selected.GetComponent<ObjectManager> ().SpawnPlaceable ().GetComponent<CloneManager> ();
						gameState.Placing = true;
						uiManager.UpdateText (uiManager.GetText () + "\nConnectable");
					}
				} else {
					uiManager.UpdateText (uiManager.GetText () + "\nNot Connectable");
					if (gameState.Selected.transform.parent != null)
						if (gameState.Selected.transform.parent.name == "PlayerShip")
						uiManager.UpdateText (uiManager.GetText () + "\nAlready Part of the Ship");
				}

			}
		}
		if (Input.GetKeyDown (KeyCode.Q)) {
			if (gameState.Placing) {
				cloneManager.ConnectorRotationOffset (1);
			}
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			if (gameState.Placing) {
				cloneManager.ConnectorRotationOffset (-1);
			}
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			if (gameState.Placing) {
			}
			shipManager.FireWeapons ();
		}

		if (Input.GetKeyUp (KeyCode.Space)) {
			if (gameState.Placing) {
			}
			shipManager.CeaseFire ();
		}
	}
	private void Select (RaycastHit2D hit){
		gameState.Selected = hit.collider.gameObject;
	}
}
