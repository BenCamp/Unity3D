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
	List<Level> levels = new List<Level> ();


	const float MAXHEIGHT = 40f;
	const float MINHEIGHT = 20f;
	const float MAXWIDTH = 80f;
	const float MINWIDTH = 20f;

	float MAXROOMWIDTH = 10f;
	float MINROOMWIDTH = 4f;
	float MAXROOMHEIGHT  = 10f;
	float MINROOMHEIGHT = 4f;
	float MAXTRIES = 50f;
	float MAXROOMS = 9f;

	int currentlyLoadedLevel = -1;

	public List <Vector2> roomCenters = new List<Vector2> ();
	Vector2 tempCenter = new Vector2();

	float playerStartX, playerStartY;

	void Start () {
		player = GameObject.Find ("PlayerSprite");
		rig2DPlayer = player.GetComponent<Rigidbody2D> ();

		level = gameObject.transform.FindChild ("Level").gameObject;
		levelCollider = level.gameObject.GetComponent<PolygonCollider2D> ();
		levelCollider.pathCount = 0;
		levels.Insert(0, GenerateLevel (0, -1, 1));
		LoadLevel (0);

		rig2DPlayer.transform.position = new Vector3 ( playerStartX, playerStartY, rig2DPlayer.transform.position.z);


	}

	//TODO
	public void AddLevel (Level level, int position){
		levels.Insert (position, level);
	}



	public void DestroyLevel (int position){
		levels.RemoveAt (position);
	}

	//TODO UPDATE LEVELS MAP


	public void LoadLevel (int levelNum) {
		float width = levels [levelNum].width;
		float height = levels [levelNum].height;

		levelCollider.pathCount = 0;
		int num = 0;
		foreach (Path p in levels[levelNum].paths) {
			levelCollider.SetPath (num, PathFunctions.ConvertVecPathToVecArray(PathFunctions.ConvertPathToVecPath(p)));
			levelCollider.pathCount++;
			num++;
		}
		levelCollider.pathCount--;
	}


	public Level GenerateLevel (int levelID, int directionFromSpawningLevel, int difficulty){
		Level level = new Level ();
		level.center = new Vector2 (0, 0);
		level.height = Random.Range (MINHEIGHT, MAXHEIGHT);
		level.width = Random.Range (MINWIDTH, MAXWIDTH);

		Path tempPath = new Path ();

		Debug.Log ("Level: Width " + level.width + ", Height " + level.height);

		//Add exterior boundary
		level.paths.Add (PathFunctions.GetRectPath (level.center, level.width + 2, level.height + 2));

		//Add starting room
		level.paths.Add (GenerateStartingRoom (directionFromSpawningLevel, level.width, level.height));

		// Build other rooms
		for (int i = 0, j = 0; i < MAXTRIES && j < MAXROOMS; i++){
			tempPath = GenerateRoom (level.width, level.height);

			if (!PathFunctions.DoPathsOverlap (level.paths [1], tempPath) && PathFunctions.DoPathsOverlap (level.paths [0], tempPath)) {
				level.paths.Add (tempPath);
				roomCenters.Add (tempCenter);
				j++;
			}
		}
		// Sanity Check
		if (roomCenters.Count > 1) {
			// TODO Build hallways
			// Currently just testing
			for (int i = 0, j = 1; j < roomCenters.Count; i++, j++) {
				level.paths.Add (PathFunctions.GetRotatedRectPath (roomCenters [i], roomCenters [j], 5f));
			}
		}


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

	public Path GenerateStartingRoom (int directionFromSpawningLevel, float width, float height){
		Vector2 rDimensions = FindRandomRoomWidthHeight();
		Vector2 rCenter = new Vector2();
		Path solution = new Path ();

		//First Level
		if (directionFromSpawningLevel == -1) {
			rCenter = FindRandomCenterInRange (width, height, rDimensions.x, rDimensions.y);
			roomCenters.Add (rCenter);
			solution = PathFunctions.GetRectPath (rCenter, rDimensions.x, rDimensions.y);
		}

		playerStartX = rCenter.x;
		playerStartY = rCenter.y - (rDimensions.y / 2) + 0.3f;
		return solution;
	
	}


	public Path GenerateRoom (float width, float height){
		Vector2 rDimensions = FindRandomRoomWidthHeight();
		Vector2 rCenter = new Vector2();
		Path solution = new Path ();
		rCenter = FindRandomCenterInRange (width, height, rDimensions.x, rDimensions.y);
		tempCenter = rCenter;
		solution = PathFunctions.GetRectPath (rCenter, rDimensions.x, rDimensions.y);
		return solution;
	}

	public Vector2 FindRandomRoomWidthHeight () {
		return new Vector2 (Random.Range (MINROOMWIDTH, MAXROOMWIDTH), Random.Range (MINROOMHEIGHT, MAXROOMHEIGHT));
	}

	public Vector2 FindRandomCenterInRange (float width, float height, float rWidth, float rHeight){

		return new Vector2 (
			Random.Range (-(width / 2) + (rWidth / 2) , (width / 2) - (rWidth / 2))
			, 
			Random.Range (-(height / 2) + (rHeight / 2), (height / 2) - (rHeight / 2))
		);
	}
}