using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ClipperLib;
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


	public static bool ValidAddition () {
		return true; 
	}

	//this function takes a list of polygons as a parameter, this list of polygons represent all the polygons that constitute collision in your level.
	public static List<List<Vector2>> Addition(List <Vector2> mainPoly, List<List<Vector2>> polygons){

		//this is going to be the result of the method
		List<List<Vector2>> unitedPolygons = new List<List<Vector2>>();
		Clipper clipper = new Clipper();

		//clipper only works with ints, so if we're working with floats, we need to multiply all our floats by
		//a scaling factor, and when we're done, divide by the same scaling factor again
		int scalingFactor = 10000;


		//Add the main polygon provided by the structure
		Path allPolygonsPath = new Path (mainPoly.Count);
		for (int i = 0; i < mainPoly.Count; i++) {
			allPolygonsPath.Add(new IntPoint(Mathf.Floor(mainPoly[i].x * scalingFactor), Mathf.Floor(mainPoly[i].y * scalingFactor)));
		}
		clipper.AddPath(allPolygonsPath, PolyType.ptSubject, true);

		//Add the polygons given by the "brush"
		for (int i = 0; i < polygons.Count; i++)
		{
			allPolygonsPath = new Path(polygons[i].Count);

			for (int j = 0; j < polygons[i].Count; j++)
			{
				allPolygonsPath.Add(new IntPoint(Mathf.Floor(polygons[i][j].x * scalingFactor), Mathf.Floor(polygons[i][j].y * scalingFactor)));
			}
			clipper.AddPath(allPolygonsPath, PolyType.ptClip, true);

		}

		//this will be the result
		Paths solution = new Paths();

		//having added all the Paths added to the clipper object, we tell clipper to execute an union
		clipper.Execute(ClipType.ctUnion, solution, PolyFillType.pftEvenOdd);

		//now we just need to convert it into a List<List<Vector2>> while removing the scaling
		foreach (Path path in solution)
		{
			List<Vector2> unitedPolygon = new List<Vector2>();
			foreach (IntPoint point in path)
			{
				unitedPolygon.Add(new Vector2(point.X / (float)scalingFactor, point.Y / (float)scalingFactor));
			}
			unitedPolygons.Add(unitedPolygon);
		}

		//this removes some redundant vertices in the polygons when they are too close from each other
		//may be useful to clean things up a little if your initial collisions don't match perfectly from tile to tile
		unitedPolygons = RemoveClosePointsInPolygons(unitedPolygons);

		//everything done
		return unitedPolygons;
	}

	//TODO Subtraction function


	//Used to simplify the polygon, removing redundant vertices
	public static List<List<Vector2>> RemoveClosePointsInPolygons(List<List<Vector2>> polygons)
	{
		float proximityLimit = 0.1f;

		List<List<Vector2>> resultPolygons = new List<List<Vector2>>();

		foreach(List<Vector2> polygon in polygons)
		{
			List<Vector2> pointsToTest = polygon;
			List<Vector2> pointsToRemove = new List<Vector2>();

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