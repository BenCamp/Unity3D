using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingBounds : MonoBehaviour {

	PolygonCollider2D poly;

	void Start () {
		
		Disable ();
		poly = gameObject.GetComponent<PolygonCollider2D> ();
	}


	// Update is called once per frame
	void Update () {

		Vector3 pos = Input.mousePosition;
		gameObject.transform.position = Camera.main.ScreenToWorldPoint(pos);

	}

	public Vector2[] OnClick () {
		return poly.GetPath (0);
	}

	public void Enable () {
		gameObject.SetActive (true);
	}

	public void Disable () {
		gameObject.SetActive (false);
	}
}
