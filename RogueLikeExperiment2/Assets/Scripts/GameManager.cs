using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***Singleton***/
public class GameManager : MonoBehaviour {
	
	/***GameObjects***/
	public static GameManager gameManager;

	/***Variables***/
	public short playerMoveX = 0;
	public short playerMoveY = 0;
	public int worldMaxX = 17;
	public int worldMinX = -17;
	public int worldMaxY = 7;
	public int worldMinY = -7;


	/***MonoBehaviour Functions***/
	void Awake () {
		//This is the only gameManger on a GameObject
		if (gameManager == null) {
			DontDestroyOnLoad (gameObject);
			gameManager = this;
		} 

		//This is NOT the only gameManger on a GameObject
		else {
			Destroy (gameObject);
		}

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}

	/***Functions***/
	public void PlayerMoveX (short value) {
		playerMoveX = value;
	}

	public void PlayerMoveY (short value) {
		playerMoveY = value;
	}
}
