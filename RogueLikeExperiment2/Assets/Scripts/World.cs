using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ClipperLib;
using VecPath = System.Collections.Generic.List<UnityEngine.Vector2>;
using VecPaths = System.Collections.Generic.List<System.Collections.Generic.List<UnityEngine.Vector2>>;
using Path = System.Collections.Generic.List<ClipperLib.IntPoint>;
using Paths = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;

public class World : MonoBehaviour {
	GameObject player;
	Rigidbody2D rig2DPlayer;

	GameObject level;
	PolygonCollider2D levelCollider;
	ShowTerrain levelTerrain;
	List<Level> levels = new List<Level> ();

	int currentlyLoadedLevel = -1;

	public List <Vector2> roomCenters = new List<Vector2> ();
	Vector2 tempCenter = new Vector2();

	float playerStartX, playerStartY;

	void Start () {
		player = GameObject.Find ("PlayerSprite");
		rig2DPlayer = player.GetComponent<Rigidbody2D> ();
		level = gameObject.transform.Find ("Level").gameObject;
		levelTerrain = level.gameObject.GetComponent<ShowTerrain> ();
		levelCollider = level.gameObject.GetComponent<PolygonCollider2D> ();
		levelCollider.pathCount = 0;
		levels.Insert(0, GenerateLevel (0, -1, 1));
		LoadLevel (0);
		rig2DPlayer.transform.position = new Vector3 ( playerStartX, playerStartY, rig2DPlayer.transform.position.z);
	}
		
	public void AddLevel (Level level, int position){
		levels.Insert (position, level);
	}
		
	public void DestroyLevel (int position){
		levels.RemoveAt (position);
	}

	public void LoadLevel (int levelNum) {
		int width = levels [levelNum].width;
		int height = levels [levelNum].height;

		levelCollider.pathCount = 0;
		int num = 0;
		foreach (Path p in levels[levelNum].paths) {
			levelCollider.SetPath (num, PathFunctions.ConvertVecPathToVecArray(PathFunctions.ConvertPathToVecPath(p)));
			levelCollider.pathCount++;
			num++;
		}
		levelCollider.pathCount--;
		levelTerrain.UpdateTerrainImage (width, height, levels[levelNum].paths, Constants.PIXELSIZE);
	}
		
	public Level GenerateLevel (int levelID, int directionFromSpawningLevel, int difficulty){
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
}