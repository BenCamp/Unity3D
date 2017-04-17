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

	//TODO Subtraction function





	/*
	 * 
	 * PRIVATE FUNCTIONS (Utility of Utility :P)
	 * 
	 */

	//Convert VecPaths to Paths, also adds the scaling factor
	private static Paths ConvertVecPathsToPaths (VecPaths vecPaths){

		//The List<List<IntPoints>> (A.K.A. Paths) that will be returned. 
		Paths solution = new Paths ();

		//Each vector2 of a VecPath for each polygon will be converted into IntPoints
		// and stored as a Path 
		Path allPolygonsPath;

		for (int i = 0; i < vecPaths.Count; i++)
		{
			allPolygonsPath = new Path(vecPaths[i].Count);

			for (int j = 0; j < vecPaths[i].Count; j++)
			{
				//Adds the scaling factor as it converts the Vector2 to an IntPoint
				allPolygonsPath.Add(new IntPoint(Mathf.Floor(vecPaths[i][j].x * SCALING_FACTOR), Mathf.Floor(vecPaths[i][j].y * SCALING_FACTOR)));
			}
			solution.Add(allPolygonsPath);

		}


		return solution;
	}


	//Convert Paths to VecPaths, also removes the scaling factor
	private static VecPaths ConvertPathsToVecPaths (Paths paths){

		VecPaths solution = new VecPaths ();
		foreach (Path path in paths)
		{
			VecPath unitedPolygon = new VecPath();
			foreach (IntPoint point in path)
			{
				unitedPolygon.Add(new Vector2(point.X / (float)SCALING_FACTOR, point.Y / (float)SCALING_FACTOR));
			}
			solution.Add(unitedPolygon);
		}

		return solution;
	}
		

	//TODO Remove totally overlapped paths.
	//For now it just returns the largest area 
	private static Path RemoveOverlappedPaths (Paths paths){
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
	private static VecPaths RemoveClosePointsInPolygons(VecPaths polygons)
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