using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
	public 	GameManager 		gameManager;
	public 	InputKeys			keys = new InputKeys();
	//TODO Testing
	TestingBounds test;

	void Start () {
		gameManager = GameObject.Find ("Game").GetComponent<GameManager> ();
	}

	// Update is called once per frame
	void Update () {
		
		//Test to see if ANYTHING was input since last frame
		//Will reduce the number of unnecessary tests
		if (Input.anyKey) {
			if (Input.GetKeyDown(keys.primary)) {
				gameManager.PrimaryButtonPressed ();
			}

			//TODO Add other Keys
		}
	}

	
}