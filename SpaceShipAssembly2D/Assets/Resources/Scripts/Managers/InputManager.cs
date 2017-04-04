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
		//TODO Add test to see if ANYTHING was input since last frame
		//Will reduce the number of unnecessary tests



		//Dealing with mouse actions
		if (Input.GetMouseButtonDown (0)) {
			if (!gameManager.paused) {
				if (gameManager.BuildingStructure && test.isActiveAndEnabled) {
					Vector2[] temp = test.OnClick ();
					shipManager.StructureAdd (temp);
				}

				else if (gameManager.placingModules) {

				}

				else {
					RaycastHit2D hit = Physics2D.Raycast (new Vector2 (mainCamera.ScreenToWorldPoint (Input.mousePosition).x, mainCamera.ScreenToWorldPoint (Input.mousePosition).y), Vector2.zero, Mathf.Infinity);
					if (hit.collider == null) {
						gameManager.selected = null;
					} else {
						gameManager.selected = hit.collider.gameObject;
					}
				}
			}
		}

		if (Input.GetMouseButtonDown (1)) {
			if (!gameManager.paused) {
				if (gameManager.BuildingStructure && test.isActiveAndEnabled) {
					Vector2[] temp = test.OnClick ();
					shipManager.StructureSubtract (temp);
				} else if (gameManager.placingModules) {

				} else {
					RaycastHit2D hit = Physics2D.Raycast (new Vector2 (mainCamera.ScreenToWorldPoint (Input.mousePosition).x, mainCamera.ScreenToWorldPoint (Input.mousePosition).y), Vector2.zero, Mathf.Infinity);
					if (hit == null) {
						gameManager.selected = null;
					} else {
						gameManager.selected = hit.collider.gameObject;
					}
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
}