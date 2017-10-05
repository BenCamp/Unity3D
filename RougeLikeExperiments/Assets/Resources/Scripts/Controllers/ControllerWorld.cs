using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerWorld : MonoBehaviour {
	public static ControllerWorld controllerWorld;

	/***MonoBehaviour Classes***/
	void Awake () {
		//This is the only controllerGame on a GameObject
		if (controllerWorld == null) {
			DontDestroyOnLoad (gameObject);
			controllerWorld = this;
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
