using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***Singleton***/
public class CameraManager : MonoBehaviour {

	/***GameObjects***/
	GameObject cameraMan;

	/***Variables***/
	public float cameraZoom = -4f;

	/***MonoBehaviour Functions***/
	void Start () {
		cameraMan = GameObject.Find ("CameraMan");
	}
	
	void Update () {
		//Follow the player
		Camera.main.transform.position = new Vector3 (cameraMan.transform.position.x, cameraMan.transform.position.y, cameraZoom);
	}

	/***Functions***/
}
