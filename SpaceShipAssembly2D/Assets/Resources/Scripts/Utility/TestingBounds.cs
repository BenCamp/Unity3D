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

		Vector2[] temp = poly.GetPath (0);
		for (int i = 0; i < temp.Length; i++) {
			temp [i] = transform.TransformPoint (new Vector3 (temp [i].x, temp [i].y, 0));
		}
		return temp;
	}

	public void Enable () {
		gameObject.SetActive (true);
	}

	public void Disable () {
		gameObject.SetActive (false);
	}
}
