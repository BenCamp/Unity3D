using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ClipperLib;
using Path = System.Collections.Generic.List<ClipperLib.IntPoint>;
using Paths = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;

public static class PathFunctions {


	public static List <Vector2[]> Addition (PolygonCollider2D poly, List <Vector2[]> paths){

		//Will hold the results of the function
		List <Vector2[]> polygonResult = new List<Vector2[]> ();

		Clipper clip = new Clipper ();

		//Scaling factor to deal with float/int conversion
		int scalingFactor = 10000;

	/*	//Loop converts List<Vector2[]> to Path and IntPoint, then adds them to clipper object for processing
		for (int i = 0; i < poly.pathCount; i++) {
			Path allPolygonsPath = new Path (paths.Count);

			for (int j = 0; j < poly.GetPath (i).Length; j++) {
				//	allPolygonsPath.Add (new IntPoint (Mathf.Floor (polygons [i] [j])));
			}
		}*/
		return new List<Vector2[]> (); 
	}

	public static List <Vector2[]> Subtraction (PolygonCollider2D poly, List <Vector2[]> paths){

		return new List<Vector2[]> (); 
	}
}
