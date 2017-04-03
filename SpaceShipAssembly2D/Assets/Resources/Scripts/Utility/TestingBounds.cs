using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingBounds : MonoBehaviour {
	// Update is called once per frame
	void Update () {

		Vector3 pos = Input.mousePosition;
		gameObject.transform.position = Camera.main.ScreenToWorldPoint(pos);

	}

	public void OnLeftClick () {

	}
	public void OnRightClick() {

	}

}
