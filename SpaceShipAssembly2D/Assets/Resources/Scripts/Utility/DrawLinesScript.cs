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
		if (poly.pathCount > 0) {
			Vector3 temp;
			line.positionCount = 0;
			int j = 0;
			for (int i = 0; i < poly.pathCount; i++) {
				line.positionCount += poly.GetPath (i).Length + 1;

				foreach (Vector2 vec in poly.GetPath(i)) {
					temp = transform.TransformPoint (new Vector3 (vec.x, vec.y, 0));
					line.SetPosition (j, temp);
					j++;
				}
			}
			line.SetPosition (j, line.GetPosition (0));
		}
	}

}
