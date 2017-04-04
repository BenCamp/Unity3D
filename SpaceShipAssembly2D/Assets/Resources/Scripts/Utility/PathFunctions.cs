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


		Path allPolygonsPath = new Path ();

		for (int i = 0; i < poly.GetPath (0).Length; i++) {
			allPolygonsPath.Add (new IntPoint (Mathf.Floor (poly.GetPath(0) [i].x * scalingFactor), Mathf.Floor (poly.GetPath(0) [i].y * scalingFactor)));
		}

		//Loop converts List<Vector2[]> to Path and IntPoint, then adds them to clipper object for processing
		for (int i = 0; i < paths.Count; i++) {
			for (int j = 0; j < paths[i].Length; j++) {
				allPolygonsPath.Add (new IntPoint (Mathf.Floor (paths [i] [j].x * scalingFactor), Mathf.Floor (paths [i] [j].y * scalingFactor)));
			}
			clip.AddPath (allPolygonsPath, PolyType.ptSubject, true);
		}
		Paths solution = new Paths ();
		clip.Execute (ClipType.ctUnion, solution);

		for (int i = 0; i < solution.Count; i++) {
			Vector2[] unitedPolygon = new Vector2[solution[i].Count];
			for (int j = 0; j < solution[i].Count; j++){
				unitedPolygon[j] = new Vector2(solution[i][j].X / (float) scalingFactor, solution[i][j].Y / (float) scalingFactor);
			}
			polygonResult.Add (unitedPolygon);
		}

		return polygonResult; 
	}

	public static List <Vector2[]> Subtraction (PolygonCollider2D poly, List <Vector2[]> paths){

		return new List<Vector2[]> (); 
	}
}
