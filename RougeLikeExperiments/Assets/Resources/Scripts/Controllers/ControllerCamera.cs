using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***Singleton***/
public class ControllerCamera : MonoBehaviour {
	public static ControllerCamera controllerCamera;

	void Awake () {
		//This is the only controllerCamera on a GameObject
		if (controllerCamera == null) {
			DontDestroyOnLoad (gameObject);
			controllerCamera = this;
		} 

		//This is NOT the only controllerCamera on a GameObject
		else {
			Destroy (gameObject);
		}
	}
	/***Monobehavior functions***/
	void Start () {
		
	}
	void Update () {
		
	}
}
