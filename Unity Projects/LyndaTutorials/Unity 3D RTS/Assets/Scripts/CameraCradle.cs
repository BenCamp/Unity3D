﻿using UnityEngine;
using System.Collections;

public class CameraCradle : MonoBehaviour {

	public float  speed = 50;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Input.GetAxis ("Horizontal") * speed * Time.deltaTime,
			Input.GetAxis ("Vertical") * speed * Time.deltaTime,
			0);
	}
}
