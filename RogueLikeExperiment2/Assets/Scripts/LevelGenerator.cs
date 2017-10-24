/*
 * This class handles all functions for generating a new level.
 * 
 * It uses information given it from the world to build an appropriate level.
 * 
 * Right now I'm going to use a simple 2 dim int array to build the map.
 * Based on the numbers in the array, I will then send this back to the world which
 * will build the colliders and textures based on the map.
 * 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using ClipperLib;
using VecPath = System.Collections.Generic.List<UnityEngine.Vector2>;
using VecPaths = System.Collections.Generic.List<System.Collections.Generic.List<UnityEngine.Vector2>>;
using Path = System.Collections.Generic.List<ClipperLib.IntPoint>;
using Paths = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;

public static class LevelGenerator {
	public static Level NewLevel (){
		Level tempLevel = new Level();

		int[,] tempMap = GenerateIntMap ();

		Paths tempPath = GeneratePaths (tempMap);


		return tempLevel;
	}

	public static int[,] GenerateIntMap () {
		int width = (int)Random.Range (0, Constants.MAXWIDTH);
		int height = (int) Random.Range (0, Constants.MAXHEIGHT);

		int [,] tempMap = new int[width,height];

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				tempMap [i, j] = 1;
			}
		}

		return tempMap;
	}

	static Paths GeneratePaths (int[,] map){
		Paths tempPaths = new Paths ();
		Path tempPath = new Path ();

		tempPaths.Add (PathFunctions.GetRectPath (new Vector2 (map.GetLength(0) / 2, map.GetLength (1) / 2), map.GetLength (0) + 2, map.GetLength (1) + 2));	

		tempPaths.Add (PathFunctions.GetRectPath (new Vector2 (map.GetLength(0) / 2, map.GetLength (1) / 2), map.GetLength (0), map.GetLength (1)));

		return tempPaths;
	}

}
/*
 * Old code for testing
 * 
 * dpublic Level GenerateLevel (int levelID, int directionFromSpawningLevel, int difficulty){
		Level level = new Level ();
		level.center = new Vector2 (0, 0);
		level.height = Random.Range (Constants.MINHEIGHT, Constants.MAXHEIGHT);
		level.width = Random.Range (Constants.MINWIDTH, Constants.MAXWIDTH);

		Path tempPath = new Path ();

		//Add exterior boundary
		level.paths.Add (PathFunctions.GetRectPath (level.center, level.width + 2, level.height + 2));

		//Add starting room
		level.paths.Add (GenerateStartingRoom (directionFromSpawningLevel, level.width, level.height));

		// Build other rooms
		for (int i = 0, j = 0; i < Constants.MAXTRIES && j < Constants.MAXROOMS; i++){
			tempPath = GenerateRoom (level.width, level.height);

			if (!PathFunctions.DoPathsOverlap (level.paths [1], tempPath) && PathFunctions.DoPathsOverlap (level.paths [0], tempPath)) {
				level.paths.Add (tempPath);
				roomCenters.Add (tempCenter);
				j++;
			}
		}

		//TODO build hallways


		Paths tempPaths = new Paths ();
		for (int i = 1; i < level.paths.Count; i++) {
			tempPaths.Add (level.paths[i]);
		}

		tempPaths = PathFunctions.CombinePaths (tempPaths);

		for (int i = level.paths.Count - 1; i > 0; i--) {
			level.paths.RemoveAt (i);
		}
		foreach (Path path in tempPaths){
			level.paths.Add (path);
		}
		return level;
	}

	public Path GenerateStartingRoom (int directionFromSpawningLevel, int width, int height){
		Vector2 rDimensions = FindRandomRoomWidthHeight();
		Vector2 rCenter = new Vector2();
		Path solution = new Path ();

		//First Level
		if (directionFromSpawningLevel == -1) {
			rCenter = FindRandomCenterInRange (width, height, (int) rDimensions.x, (int) rDimensions.y);
			roomCenters.Add (rCenter);
			solution = PathFunctions.GetRectPath (rCenter, rDimensions.x, rDimensions.y);
		}

		playerStartX = rCenter.x;
		playerStartY = rCenter.y - (rDimensions.y / 2) + Constants.PLAYERHEIGHT / 2;
		return solution;
	
	}

	public Path GenerateRoom (int width, int height){
		Vector2 rDimensions = FindRandomRoomWidthHeight();
		Vector2 rCenter = new Vector2();
		Path solution = new Path ();
		rCenter = FindRandomCenterInRange (width, height, (int) rDimensions.x, (int) rDimensions.y);
		tempCenter = rCenter;
		solution = PathFunctions.GetRectPath (rCenter, rDimensions.x, rDimensions.y);
		return solution;
	}
		
	public Vector2 FindRandomRoomWidthHeight () {
		return new Vector2 (Random.Range (Constants.MINROOMWIDTH, Constants.MAXROOMWIDTH), Random.Range (Constants.MINROOMHEIGHT, Constants.MAXROOMHEIGHT));
	}

	public Vector2 FindRandomCenterInRange (int width, int height, int rWidth, int rHeight){

		return new Vector2 (
			Random.Range (-(width / 2) + (rWidth / 2) , (width / 2) - (rWidth / 2))
			, 
			Random.Range (-(height / 2) + (rHeight / 2), (height / 2) - (rHeight / 2))
		);
	}
*/