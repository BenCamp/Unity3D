using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ClipperLib;
using VecPath = System.Collections.Generic.List<UnityEngine.Vector2>;
using VecPaths = System.Collections.Generic.List<System.Collections.Generic.List<UnityEngine.Vector2>>;
using Path = System.Collections.Generic.List<ClipperLib.IntPoint>;
using Paths = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;

/*****
 * Ben Camp <-- Da coder
 * 
 * This utility class allows us to use the Clipper Library to (hopefully) quickly determine the resulting shapes of 
 * additions and subtractions to polygons used for collisions in the game.
 * 
 * Modified from code posted on : gamedev.stackexchange.com/questions/125927/how-do-i-merge-colliders-in-a-tile-based-game
 * 
 * 
*****/
public static class PathFunctions {
	//clipper only works with ints, so if we're working with floats, we need to multiply all our floats by
	//a scaling factor, and when we're done, divide by the same scaling factor again
	const int SCALING_FACTOR = 10000;


	/*
	 * 
	 * PUBLIC FUNCTIONS
	 * 
	 */

	//Takes two VecPaths and combines them into a single Path if possible
	public static VecPath Addition(VecPaths mainPolys, VecPaths polygons){


		//this is going to be the result of the method
		VecPaths unitedPolygons = new VecPaths();

		//this will be the result of the algorithm (will be converted into VecPaths and stored in unitedPolygons)
		Paths solution = new Paths();

		Clipper clipper = new Clipper();



		//Add the main polygons provided by the structure
		clipper.AddPaths(ConvertVecPathsToPaths(mainPolys), PolyType.ptSubject, true);

		//Add the polygons given by the "brush"
		clipper.AddPaths(ConvertVecPathsToPaths(polygons), PolyType.ptClip, true);

		//Clipper executes a union combining all the paths if possible
		clipper.Execute(ClipType.ctUnion, solution, PolyFillType.pftEvenOdd);

		//Something to stop it from leaving those small paths completely overlapped by the larger path
		solution[0] = RemoveOverlappedPaths(solution);

		//Convert solution into VecPaths while removing the scaling
		unitedPolygons = ConvertPathsToVecPaths (solution);

		//this removes some redundant vertices in the polygons when they are too close to each other
		unitedPolygons = RemoveClosePointsInPolygons(unitedPolygons);

		//everything done
		return unitedPolygons[0];
	}

	public static Paths CombinePaths (Paths paths) {
		Paths solution = new Paths ();

		Clipper clipper = new Clipper ();

		clipper.AddPaths (paths, PolyType.ptSubject, true);

		clipper.Execute (ClipType.ctUnion, solution, PolyFillType.pftNonZero);

		return solution;
	}

	//TODO Subtraction function
	public static VecPaths Subtraction (){
		VecPaths solution = new VecPaths ();

		return solution;
	}

	public static bool DoPathsOverlap (Path firstPath, Path secondPath){
		foreach (IntPoint point in secondPath) {
			if (Clipper.PointInPolygon (point, firstPath) == 1 || Clipper.PointInPolygon (point, firstPath) == -1) {
				return true;
			}
		}

		return false;
	}

	public static Path GetRectPath (Vector2 c, float w, float h){
		Path solution = new Path ();

		int centerX = (int) c.x * SCALING_FACTOR;
		int centerY = (int) c.y * SCALING_FACTOR;
		int width = (int) w * SCALING_FACTOR;
		int height = (int) h * SCALING_FACTOR;

		solution.Add (new IntPoint (centerX - (width / 2), centerY - (height / 2)));
		solution.Add (new IntPoint (centerX - (width / 2), centerY + (height / 2)));
		solution.Add (new IntPoint (centerX + (width / 2), centerY + (height / 2)));
		solution.Add (new IntPoint (centerX + (width / 2), centerY - (height / 2)));

		return solution;
	}

	public static Path GetRotatedRectPath (Vector2 firstCenter, Vector2 secondCenter, float diameter){
		Path solution = new Path ();

		float opp = (secondCenter.y - firstCenter.y) * SCALING_FACTOR;
		float adj = (secondCenter.x - firstCenter.x) * SCALING_FACTOR;
		float angle;

		if (opp == 0) {
			angle = 90f;
		}
		else if (adj == 0) {
			angle = 0f;
		}
		else {
			angle = Mathf.Rad2Deg * Mathf.Atan (opp / adj);
		}


		float xLength = ( Mathf.Cos (angle)) * (diameter / 2);
		float yLength = ( Mathf.Sin (angle)) * (diameter / 2);

		solution.Add (new IntPoint ((firstCenter.x - xLength) * SCALING_FACTOR, (firstCenter.y + yLength) * SCALING_FACTOR));
		solution.Add (new IntPoint ((firstCenter.x + xLength) * SCALING_FACTOR, (firstCenter.y - yLength) * SCALING_FACTOR));
		solution.Add (new IntPoint ((secondCenter.x + xLength) * SCALING_FACTOR, (secondCenter.y - yLength) * SCALING_FACTOR));
		solution.Add (new IntPoint ((secondCenter.x - xLength) * SCALING_FACTOR, (secondCenter.y + yLength) * SCALING_FACTOR));

		return solution;
	}

	//Convert VecPaths to Paths, also adds the scaling factor
	public static Paths ConvertVecPathsToPaths (VecPaths vecPaths){

		//The List<List<IntPoints>> (A.K.A. Paths) that will be returned. 
		Paths solution = new Paths ();

		foreach (VecPath vecPath in vecPaths){
			solution.Add(ConvertVecPathToPath(vecPath));
		}
		return solution;
	}


	public static Path ConvertVecPathToPath (VecPath vecPath){
		Path solution = new Path ();
		for (int i = 0; i < vecPath.Count; i++)
		{
			//Adds the scaling factor as it converts the Vector2 to an IntPoint
			solution.Add(new IntPoint(Mathf.Floor(vecPath[i].x * SCALING_FACTOR), Mathf.Floor(vecPath[i].y * SCALING_FACTOR)));
		}
		return solution;
	}

	//Convert Paths to VecPaths, also removes the scaling factor
	public static VecPaths ConvertPathsToVecPaths (Paths paths){

		VecPaths solution = new VecPaths ();
		foreach (Path path in paths)
		{
			
			solution.Add(ConvertPathToVecPath(path));
		}

		return solution;
	}


	public static VecPath ConvertPathToVecPath (Path path){
		VecPath solution = new VecPath (path.Count);

		foreach (IntPoint point in path) {
			solution.Add (new Vector2 (point.X / (float)SCALING_FACTOR, point.Y / (float)SCALING_FACTOR));
		}

		return solution;
	}

	public static Vector2[] ConvertVecPathToVecArray (VecPath vecPath){
		Vector2[] solution = new Vector2[vecPath.Count];

		for (int i = 0; i < vecPath.Count; i++){
			solution [i] = vecPath [i];
		}

		return solution;
	}

	//TODO Remove totally overlapped paths.
	//For now it just returns the largest area 
	public static Path RemoveOverlappedPaths (Paths paths){
		Path solution = new Path ();

		Clipper clip = new Clipper ();


		if (paths.Count > 1) {
			double longest = 0;
			int i = 0;
			int numOfPath = -1;

			foreach (Path path in paths) {

				double temp = Clipper.Area (path);

				if (temp > longest) {
					longest = temp;
					numOfPath = i;
				}

				i++;

			}

			solution = paths [numOfPath];

		} else {
			solution = paths [0];
		}



		return solution;
	}


	//Simplifies the polygon, removing redundant vertices
	public static VecPaths RemoveClosePointsInPolygons(VecPaths polygons)
	{
		float proximityLimit = 0.1f;

		VecPaths resultPolygons = new VecPaths();

		foreach(VecPath polygon in polygons)
		{
			VecPath pointsToTest = polygon;
			VecPath pointsToRemove = new VecPath();

			foreach (Vector2 pointToTest in pointsToTest)
			{
				foreach (Vector2 point in polygon)
				{
					if (point == pointToTest || pointsToRemove.Contains(point)) continue;

					bool closeInX = Mathf.Abs(point.x - pointToTest.x) < proximityLimit;
					bool closeInY = Mathf.Abs(point.y - pointToTest.y) < proximityLimit;

					if (closeInX && closeInY)
					{
						pointsToRemove.Add(pointToTest);
						break;
					}
				}
			}
			polygon.RemoveAll(x => pointsToRemove.Contains(x));

			if(polygon.Count > 0)
			{
				resultPolygons.Add(polygon);
			}
		}

		return resultPolygons;
	}
}