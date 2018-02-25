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
		VecPaths vecPaths = new VecPaths ();
		Level solution = new Level ();
		int width = map.sectorMap.GetLength (0);
		int height = map.sectorMap.GetLength (1);
		vecPaths = BuildWallsFromMap (map);
		vecPaths.Add (OuterBorder (map));
		solution.paths = ConvertVecPathsToVecArrs (vecPaths);
		solution.map = map;
		return solution;
	}
	#region VecPaths
	static VecPath OuterBorder (Map map){
		VecPath solution = new VecPath();
		solution.Add (new Vector2 (0, 0));
		solution.Add (new Vector2 ((map.sectorMap.GetLength (0)) * Constants.PLAYERHEIGHT, 0));
		solution.Add (new Vector2 ((map.sectorMap.GetLength (0))* Constants.PLAYERHEIGHT, -((map.sectorMap.GetLength (1))* Constants.PLAYERHEIGHT)));
		solution.Add (new Vector2 (0, -((map.sectorMap.GetLength (1))* Constants.PLAYERHEIGHT)));
		return solution;
	}
	static VecPaths BuildWallsFromMap (Map map){
		VecPaths solution = new VecPaths ();
		Sector temp;
		for (int j = 0; j < map.sectorMap.GetLength (1); j++) {
			for (int i = 0; i < map.sectorMap.GetLength (0); i++) {
				temp = map.sectorMap [i, j];
				if ((temp.type == Constants.TYPE_HALLWAY && temp.roomNumber == 0) || temp.type == Constants.TYPE_DOOR || temp.type == Constants.TYPE_ROOM) {
						solution = AddSectorToPath (i, j, solution);
				}
			}
		}
		return solution;
	}
	static VecPaths AddSectorToPath (int i, int j, VecPaths vPaths){
		VecPaths solution = vPaths;
		VecPath temp = new VecPath ();
		temp.Add (AddUL (i, j));
		temp.Add (AddUR (i, j));
		temp.Add (AddLR (i, j));
		temp.Add (AddLL (i, j));
		solution = CombinePaths (solution, temp);
		return solution;
	}
	static VecPaths CombinePaths (VecPaths vPaths, VecPath vPath){
		VecPaths solution = new VecPaths ();
		Paths paths = PathFunctions.ConvertVecPathsToPaths (vPaths);
		Path path = PathFunctions.ConvertVecPathToPath (vPath);
		Clipper clip = new Clipper ();
		clip.AddPaths (paths, PolyType.ptSubject, true);
		clip.AddPath (path, PolyType.ptSubject, true);
		clip.Execute (ClipType.ctUnion, paths, PolyFillType.pftEvenOdd);
		solution = PathFunctions.ConvertPathsToVecPaths (paths);
		return solution;
	}
	static Vector2 AddUL (int i, int j){
		Vector2 solution = new Vector2 (i * Constants.PLAYERHEIGHT, -(j * Constants.PLAYERHEIGHT));
		return solution;
	}
	static Vector2 AddUR (int i, int j){
		Vector2 solution = new Vector2 ((i + 1) * Constants.PLAYERHEIGHT, -(j * Constants.PLAYERHEIGHT));
		return solution;
	}
	static Vector2 AddLL (int i, int j){
		Vector2 solution = new Vector2 (i * Constants.PLAYERHEIGHT, -((j + 1) * Constants.PLAYERHEIGHT));
		return solution;
	}
	static Vector2 AddLR (int i, int j){
		Vector2 solution = new Vector2 ((i + 1) * Constants.PLAYERHEIGHT, -((j + 1) * Constants.PLAYERHEIGHT));
		return solution;
	}
	static List<Vector2[]> ConvertVecPathsToVecArrs (VecPaths vPaths){
		List<Vector2[]> solution = new List<Vector2[]> ();
		Vector2[] tempPathArr;
		foreach (VecPath p in vPaths) {
			tempPathArr = new Vector2[p.Count];
			for (int i = 0; i < p.Count; i++) {
				tempPathArr[i] = p [i];
			}
			solution.Add (tempPathArr);
		}
		return solution;
	}
	#endregion
}