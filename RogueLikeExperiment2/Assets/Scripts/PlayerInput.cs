using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***Singleton***/
public class PlayerInput : MonoBehaviour {
	/***GameObjects***/
	public static PlayerInput playerInput;

	/***Variables***/
	KeyCode right, left, up, down, rightAlt, leftAlt, upAlt, downAlt;

	/***MonoBehaviour Functions***/
	void Awake () {
		//This is the only playerInput on a GameObject (singleton)
		if (playerInput == null) {
			DontDestroyOnLoad (gameObject);
			playerInput = this;
		} 

		//This is NOT the only playerInput on a GameObject (singleton)
		else {
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		//Default Keymapping
		right = KeyCode.RightArrow;
		rightAlt = KeyCode.D;
		left = KeyCode.LeftArrow;
		leftAlt = KeyCode.A;
		up = KeyCode.UpArrow;
		upAlt = KeyCode.W;
		down = KeyCode.DownArrow;
		downAlt = KeyCode.S;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKey) {
			//Go right
			if (Input.GetKey (right) || Input.GetKey (rightAlt)) {
				GameManager.gameManager.PlayerMoveX (1);
			}

			//Go Left
			if (Input.GetKey (left) || Input.GetKey (leftAlt)) {
				GameManager.gameManager.PlayerMoveX (-1);
			}

			//Jump
			if (Input.GetKey (up) || Input.GetKey (upAlt)) {
				GameManager.gameManager.PlayerMoveY (1);
			}

			//Crouch
			if (Input.GetKey (down) || Input.GetKey (downAlt)) {
				GameManager.gameManager.PlayerMoveY (-1);
			}
		}
	}
}
