using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
	public GameManager gameManager;
	public UIManager uiManager;
	public StructureManager shipManager;
	public Camera mainCamera;

	//TODO Testing
	TestingBounds test;

	void Start () {
		gameManager = GameObject.Find ("Game").GetComponent<GameManager> ();
		uiManager = GameObject.Find ("UI").GetComponent<UIManager> ();
		shipManager = GameObject.Find ("Ship").GetComponent<StructureManager>();
		test = GameObject.Find ("BoundsTester").GetComponent<TestingBounds> ();
		mainCamera = Camera.main;
	}

	// Update is called once per frame
	void Update () {
		//Dealing with mouse actions
		if (Input.GetMouseButtonDown (0)) {
			if (!gameManager.paused) {
				if (gameManager.BuildingStructure && test.isActiveAndEnabled) {
					test.OnLeftClick ();
				}

				if (gameManager.placingModules) {

				}

				if (gameManager.selected == null) {
					RaycastHit2D hit = new RaycastHit2D ();
					if (Physics2D.Raycast (mainCamera.ScreenPointToRay (Input.mousePosition), out hit)) {
						gameManager.selected = hit.transform.gameObject;
					}
				} else {
					RaycastHit2D hit = new RaycastHit2D ();
					if (Physics2D.Raycast (mainCamera.ScreenPointToRay (Input.mousePosition), out hit) && hit == null) {
						gameManager.selected = null;
					}
				}
			}
		}

		if (Input.GetMouseButtonDown (1)) {
			if (!gameManager.paused) {
				if (gameManager.BuildingStructure && test.isActiveAndEnabled) {
					test.OnRightClick ();
				}
			}
		}
			
		if (Input.GetKey (KeyCode.Escape)) {
			
		}

		//Dealing with engine controls
		if (Input.GetKey (KeyCode.W)) {
			shipManager.ForwardCommandGiven ();
		}

		if (Input.GetKey (KeyCode.A)) {
			shipManager.LeftCommandGiven ();
		}		

		if (Input.GetKey (KeyCode.S)) {
			shipManager.BackCommandGiven ();
		}

		if (Input.GetKey (KeyCode.D)) {
			shipManager.RightCommandGiven ();
		}

		if (!gameManager.CameraFollowSelected) {
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

		if (Input.GetKeyDown (KeyCode.Space)) {
			shipManager.FireWeapons ();
		}

		if (Input.GetKeyUp (KeyCode.Space)) {
			shipManager.CeaseFire ();
		}
	}

	private void Select (RaycastHit2D hit){
		gameManager.selected = hit.collider.gameObject;
	}
}