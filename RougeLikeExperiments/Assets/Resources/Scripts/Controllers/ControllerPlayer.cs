using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPlayer : MonoBehaviour {
	public static ControllerPlayer controllerPlayer;

	/***MonoBehaviour Classes***/
	void Awake () {
		//This is the only controllerGame on a GameObject
		if (controllerPlayer == null) {
			DontDestroyOnLoad (gameObject);
			controllerPlayer = this;
		} 

		//This is NOT the only controllerGame on a GameObject
		else {
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
}
