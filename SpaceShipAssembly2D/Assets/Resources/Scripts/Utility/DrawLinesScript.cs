/*
 * Used for testing the polygon collider 
 * 
 * */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLinesScript : MonoBehaviour {
	PolygonCollider2D poly;
	LineRenderer line;
	// Use this for initialization
	void Start () {
		line = gameObject.AddComponent<LineRenderer>();
		poly = gameObject.GetComponent<PolygonCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 temp;
		int i = 0;
		line.positionCount = poly.points.Length + 1;
		foreach (Vector2 vec in poly.points) {

			temp = transform.TransformPoint(new Vector3 (vec.x, vec.y, 0));
			line.SetPosition (i, temp);
			i++;
		}
		temp = transform.TransformPoint(new Vector3 (poly.points [0].x, poly.points [0].y, 0));
		line.SetPosition (i, temp);
	}

}
