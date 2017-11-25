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

#region Using
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using ClipperLib;
using VecPath = System.Collections.Generic.List<UnityEngine.Vector2>;
using VecPaths = System.Collections.Generic.List<System.Collections.Generic.List<UnityEngine.Vector2>>;
using Path = System.Collections.Generic.List<ClipperLib.IntPoint>;
using Paths = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;
#endregion
#region My Structs and Whatnot

struct Rooms { 
	public Room [] rooms;
}

struct Room {
	public Vector2 upperLeft;
	public int width;
	public int height;
}

struct MapPointAndDirection {
	public Vector2 mapPoint;
	public byte direction;
}

struct PathAndDirection {
	public Path path;
	public byte direction;
}


public enum Direction : byte { Up = 1, Right, Down, Left };
#endregion
public class LevelGenerator {

	#region Main
	public Level MakeLevel (){
		Level tempLevel = new Level();

		//Map Stuff
		int[,] tempMap = GenerateIntMap ();
		tempLevel.width = tempMap.GetLength (0);
		tempLevel.height = tempMap.GetLength (1);
		tempMap = GenerateRooms (tempMap);

		//Path Stuff
		tempLevel.paths = MakePathsFromMap (tempMap);

		return tempLevel;
	}


	#endregion
	#region Map
	public int[,] MakeMap () {
		int[,] solution = GenerateIntMap ();
		Rooms rooms = GenerateRooms();

		return solution;
	}

	int[,] GenerateIntMap () {
		int width = (int)Random.Range (Constants.MINWIDTH, Constants.MAXWIDTH);
		int height = (int) Random.Range (Constants.MINHEIGHT, Constants.MAXHEIGHT);

		int [,] tempMap = new int[width,height];

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				tempMap [i, j] = 0;
			}
		}
		return tempMap;
	}

	Rooms GenerateRooms () {
		Rooms solution = new Rooms ();
		bool goodRoom = true;

		Room tempRoom = new Room ();

		return solution;
	}

	Room GenerateRoom (int [,] map) {
		Room tempRoom = new Room ();
		tempRoom.width = Random.Range (Constants.MINROOMWIDTH, Constants.MAXROOMWIDTH);
		tempRoom.height = Random.Range (Constants.MINROOMHEIGHT, Constants.MAXROOMHEIGHT);
		tempRoom.upperLeft = new Vector2 (Random.Range (1 , map.GetLength (0) - 2 - tempRoom.width), Random.Range (1, map.GetLength (1) - 2 - tempRoom.height));
		return tempRoom;
	}

	int [,] PutRoomsInMap (int[,] map, Rooms rooms){
		int[,] solution = map;
		return solution;
	}
	#endregion
	#region Paths

	/* Will start from the bottom left most point. */
	Paths MakePathsFromMap (int[,] map){
		bool done = false;
		Paths tempPaths = new Paths ();
		Path tempPath = new Path ();

		/* Add outer box */
		tempPaths.Add (PathFunctions.GetRectPath (new Vector2 (map.GetLength(0) * Constants.PLAYERHEIGHT / 2, map.GetLength (1) * Constants.PLAYERHEIGHT  / 2), map.GetLength (0) * Constants.PLAYERHEIGHT, map.GetLength (1) * Constants.PLAYERHEIGHT));

		/* Start path */
		Vector2 startPath = FindStartOfPath (map);
		if (startPath.y != 0) {// Indicates that a suitable point was found
			tempPath.Add (BottomLeft(startPath));
			PathAndDirection checkingToTheRightOfStart = FindPath (startPath, new Vector2 (startPath.x + 1, startPath.y), tempPath, map, 2);

		}

		tempPaths.Add (tempPath);
		return tempPaths;
	}

	/* Finds the LOWEST LEFTMOST Open Position on the Map */
	Vector2 FindStartOfPath (int [,] map) {
		Vector2 tempPath = new Vector2 (1, map.GetLength(1) - 2);
		while (map [(int) tempPath.x, (int) tempPath.y] != 1 && tempPath.y != 1) {
			if (tempPath.x <= map.GetLength (0) - 3) {
				tempPath.x++;
			}
			else {
				tempPath.x = 1;
				tempPath.y--;
			}
		}
		return tempPath;
	}

	PathAndDirection FindPath (Vector2 start, Vector2 checking, Path path, int [,] map, byte direction){
		PathAndDirection solution = new PathAndDirection ();
		Debug.Log ("X = " + checking.x + ", Y = " + checking.y);
		if (start == checking) {
			solution.path = path;
			solution.direction = direction;
			return solution;
		}

		switch (direction) {

		case 1://Entered going up
			if (map [(int) checking.x + 1, (int) checking.y] == 1) { //Right sector is open
				path.Add (BottomRight (checking));
				solution = FindPath (start, new Vector2 ((int) checking.x + 1, (int) checking.y), path, map, 2);
			}
			else if (map [(int) checking.x, (int) checking.y - 1] == 1) { //Up sector is open
				solution = FindPath (start, new Vector2 ((int) checking.x, (int) checking.y - 1), path, map, 1);
			}
			else if (map [(int) checking.x - 1, (int) checking.y] == 1) { //Left sector is open
				path.Add (TopRight(checking));
				solution = FindPath (start, new Vector2 ((int) checking.x - 1, (int) checking.y), path, map, 4);
			}
			else { // End of a hallway or something, go back
				path.Add (TopRight(checking));
				path.Add (TopLeft (checking));
				solution = FindPath(start, new Vector2 ((int) checking.x, (int) checking.y + 1), path, map, 3);
			}
			break;

		case 2: //Entered going right
			if (map [(int) checking.x, (int) checking.y + 1] == 1) { //Down sector is open
				path.Add (BottomLeft (checking));
				solution = FindPath (start, new Vector2 ((int) checking.x, (int) checking.y - 1), path, map, 3);
			}
			else if (map [(int) checking.x + 1, (int) checking.y] == 1) { //Right sector is open
				solution = FindPath (start, new Vector2 ((int) checking.x + 2, (int) checking.y), path, map, 2);
			}
			else if (map [(int) checking.x, (int) checking.y - 1] == 1) { //Up sector is open
				path.Add (BottomRight (checking));
				solution = FindPath (start, new Vector2 ((int) checking.x, (int) checking.y - 1), path, map, 1);
			}
			else {
				path.Add (BottomRight (checking));
				path.Add (TopRight (checking));
				solution = FindPath (start, new Vector2 ((int) checking.x - 1, (int) checking.y), path, map, 4);
			}
			break;

		case 3: //Entered going down
			if (map [(int) checking.x - 1, (int) checking.y] == 1) { //Left sector is open
				path.Add (TopLeft (checking));
				solution = FindPath (start, new Vector2 ((int) checking.x - 1, (int) checking.y), path, map, 4);
			}
			else if (map [(int) checking.x, (int) checking.y + 1] == 1) { //Down sector is open
				solution = FindPath (start, new Vector2 ((int) checking.x, (int) checking.y + 1), path, map, 3);
			}
			else if (map [(int) checking.x + 1, (int) checking.y] == 1) { //Right sector is open
				path.Add (BottomLeft (checking));
				solution = FindPath (start, new Vector2 ((int) checking.x + 1, (int) checking.y), path, map, 2);
			}
			else {
				path.Add (BottomLeft (checking));
				path.Add (BottomRight (checking));
				solution = FindPath (start, new Vector2 ((int) checking.x, (int) checking.y - 1), path, map, 1);
			}
			break;

		case 4: // Entered going left
			if (map [(int) checking.x, (int) checking.y - 1] == 1) { //Up sector is open
				path.Add (TopRight (checking));
				solution = FindPath (start, new Vector2 ((int) checking.x, (int) checking.y - 1), path, map, 1);
			}
			else if (map [(int) checking.x - 1, (int) checking.y] == 1) { //Left sector is open
				solution = FindPath (start, new Vector2 ((int) checking.x - 1, (int) checking.y), path, map, 4);
			}
			else if (map [(int) checking.x, (int) checking.y + 1] == 1) { //Down sector is open
				path.Add (TopLeft (checking));
				solution = FindPath (start, new Vector2 ((int) checking.x, (int) checking.y + 1), path, map, 3);
			}
			else {
				path.Add (TopLeft (checking));
				path.Add (BottomLeft (checking));
				solution = FindPath (start, new Vector2 ((int) checking.x + 1, (int) checking.y), path, map, 2);
			}
			break;

		default: // Probably an error

			break;
		}

		return solution;
	}

	IntPoint BottomLeft (Vector2 point){
		IntPoint tempPoint = new IntPoint ();
		tempPoint.X = ((int)point.x * Constants.PLAYERHEIGHT) * 10000;
		tempPoint.Y = ((int)point.y * Constants.PLAYERHEIGHT + Constants.PLAYERHEIGHT) * 10000;
		return tempPoint;
	}

	IntPoint BottomRight (Vector2 point) {
		IntPoint tempPoint = new IntPoint ();
		tempPoint.X = ((int)point.x * Constants.PLAYERHEIGHT + Constants.PLAYERHEIGHT) * 10000;
		tempPoint.Y = ((int)point.y * Constants.PLAYERHEIGHT + Constants.PLAYERHEIGHT) * 10000;
		return tempPoint;
	}

	static IntPoint TopLeft (Vector2 point){
		IntPoint tempPoint = new IntPoint ();
		tempPoint.X = ((int)point.x * Constants.PLAYERHEIGHT) * 10000;
		tempPoint.Y = ((int)point.y * Constants.PLAYERHEIGHT) * 10000;
		return tempPoint;
	}


	IntPoint TopRight (Vector2 point) {
		IntPoint tempPoint = new IntPoint ();
		tempPoint.X = ((int)point.x * Constants.PLAYERHEIGHT + Constants.PLAYERHEIGHT) * 10000;
		tempPoint.Y = ((int)point.y * Constants.PLAYERHEIGHT) * 10000;
		return tempPoint;
	}



	#endregion
	#region Environmentals
	#endregion
	#region Objects
	#endregion
	#region Neutrals
	#endregion
	#region Enemy
	#endregion
	#region Friendly
	#endregion
}