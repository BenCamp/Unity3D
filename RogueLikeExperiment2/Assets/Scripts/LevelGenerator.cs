/*
 * This class handles all functions for generating a new level.
 * 
 * It uses information given it from a map provided by a map generator
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
#region Structs
public struct Edge {
	public bool north;
	public bool east;
	public bool south;
	public bool west;
}
struct PathAndEdges {
	public List <Vector2> path;
	public Edge[,] edges;
}
#endregion
public static class LevelGenerator {
	public static Level MakeNewLevel () {
		Level solution = new Level ();
		Map map = MapGenerator.MakeMap ();
		solution = ConvertMapToLevel (map);
		return solution;
	}
	static Level ConvertMapToLevel (Map map) {
		//Debugging
		World world = GameObject.Find ("World").GetComponent<World> ();
		//
		Level solution = new Level ();
		int width = map.sectorMap.GetLength (0);
		int height = map.sectorMap.GetLength (1);
		Edge[,] edges = FindEdges(map, width, height);
		List <List<Vector2>> paths = EdgeArrToVecArr(edges, map);
		world.PathsDebug (paths);
		return solution;
	}
	#region Edges
	static Edge[,] FindEdges (Map map, int width, int height){
		Edge[,] solution = new Edge[width, height];
		Sector temp;
		for (int j = 0; j < height; j++){
			for (int i = 0; i < width; i++) {
				temp = map.sectorMap [i, j];
				if (temp.type == Constants.TYPE_HALLWAY) {
					if (temp.roomNumber == 0)
						solution [i, j] = EdgesOfSector (map, i, j);
				}
				else if (temp.type == Constants.TYPE_DOOR
				    || temp.type == Constants.TYPE_ROOM) {
					solution [i, j] = EdgesOfSector (map, i, j);
				}
			}
		}
		return solution;
	}
	static Edge EdgesOfSector (Map map, int i, int j){
		Edge solution = new Edge ();
		Sector temp = map.sectorMap [i, j];
		solution.north = false;
		solution.east = false;
		solution.south = false;
		solution.west = false;
		if (i != 0 && i != map.sectorMap.GetLength (0) - 1 && j != 0 && j != map.sectorMap.GetLength (1) - 1) {
			if (map.sectorMap [i, j - 1].roomNumber == -1)
				solution.north = true;
			if (map.sectorMap [i + 1, j].roomNumber == -1)
				solution.east = true;
			if (map.sectorMap [i, j + 1].roomNumber == -1)
				solution.south = true;
			if (map.sectorMap [i - 1, j].roomNumber == -1)
				solution.west = true;
		}
		return solution;
	}
	#endregion
	#region Paths


	static List <List<Vector2>> EdgeArrToVecArr (Edge[,] edges, Map map){
		
		List <List<Vector2>> solution = new List<List<Vector2>>();
		bool done = false;
		int xCount = 1;
		int yCount = 1;
		Edge[,] tempEdges = edges;
		PathAndEdges tempPathAndEdges;
		//The Border for the whole level
		List<Vector2> temp = new List<Vector2>();
		temp.Add (new Vector2 (0, 0));
		temp.Add (new Vector2 ((map.sectorMap.GetLength (0) - 1) * Constants.PLAYERHEIGHT, 0));
		temp.Add (new Vector2 ((map.sectorMap.GetLength (0) - 1)* Constants.PLAYERHEIGHT, -((map.sectorMap.GetLength (1) - 1)* Constants.PLAYERHEIGHT)));
		temp.Add (new Vector2 (0, -((map.sectorMap.GetLength (1) - 1)* Constants.PLAYERHEIGHT)));
		//The Paths for the level
		solution.Add (temp);
		while (!done) {
			done = true;
			for (int j = 1; j < tempEdges.GetLength (1); j++) {
				for (int i = 1; i < tempEdges.GetLength (0); i++) {
					if (tempEdges [i, j].north || tempEdges [i, j].east ||tempEdges [i, j].south || tempEdges [i, j].west) {
						tempPathAndEdges = FindPath (i, j, 0, tempEdges);
						solution.Add (tempPathAndEdges.path);
						tempEdges = tempPathAndEdges.edges;
					}
				}
			}
		}
		return solution;
	}
	static PathAndEdges FindPath (int i, int j, int dir, Edge [,] edges){
		PathAndEdges solution = new PathAndEdges ();
		int next = dir;
		if (dir == 0) {
			if (edges [i, j].north) {
				solution.path.Add (new Vector2 (i * Constants.PLAYERHEIGHT, -(j * Constants.PLAYERHEIGHT)));
				solution.path.Add (new Vector2 ((i + 1) * Constants.PLAYERHEIGHT, -(j * Constants.PLAYERHEIGHT)));
				dir = FindNextEdge (i++, j, 2, edges);
			}
			else if (edges [i, j].east) {
				solution.path.Add (new Vector2 ((i + 1) * Constants.PLAYERHEIGHT, -(j * Constants.PLAYERHEIGHT)));
				solution.path.Add (new Vector2 ((i + 1) * Constants.PLAYERHEIGHT, -((j + 1) * Constants.PLAYERHEIGHT)));
				dir = FindNextEdge (i, j++, 3, edges);
			}
			else if (edges [i, j].south) {
				solution.path.Add (new Vector2 ((i + 1) * Constants.PLAYERHEIGHT, -((j + 1) * Constants.PLAYERHEIGHT)));
				solution.path.Add (new Vector2 (i * Constants.PLAYERHEIGHT, -((j + 1) * Constants.PLAYERHEIGHT)));
				dir = FindNextEdge (i--, j, 4, edges);
			}
			else if (edges [i, j].west) {
				solution.path.Add (new Vector2 (i * Constants.PLAYERHEIGHT, -((j + 1) * Constants.PLAYERHEIGHT)));
				solution.path.Add (new Vector2 (i * Constants.PLAYERHEIGHT, -(j * Constants.PLAYERHEIGHT)));
				dir = FindNextEdge (i, j--, 1, edges);
			}

		}
		else if (dir == 1) {

		}
		else if (dir == 2) {

		}
		else if (dir == 3) {

		}
		else if (dir == 4) {

		}
		return solution;
	}
	static int FindNextEdge (int i, int j, int dir, Edge[,] edges){
		int solution = 0;
		return solution;
	}
	#endregion
}