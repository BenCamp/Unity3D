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

	public int[,] map;
	Vector2 tempCenter = new Vector2();

	float playerStartX, playerStartY;

	void Start () {
		player = GameObject.Find ("PlayerSprite");
		rig2DPlayer = player.GetComponent<Rigidbody2D> ();
		level = gameObject.transform.Find ("Level").gameObject;
		levelTerrain = level.gameObject.GetComponent<ShowTerrain> ();
		levelCollider = level.gameObject.GetComponent<PolygonCollider2D> ();
		levelCollider.pathCount = 0;

		AddLevel (0, LevelGenerator.MakeLevel ());
		LoadLevel (0);
		rig2DPlayer.transform.position = new Vector3 ( playerStartX, playerStartY, rig2DPlayer.transform.position.z);
	}

	public void CreateLevel () {
		levelCollider.pathCount = 0;
		AddLevel (levels.Count, LevelGenerator.MakeLevel ());
		LoadLevel (levels.Count - 1);
		rig2DPlayer.transform.position = new Vector3 ( playerStartX, playerStartY, rig2DPlayer.transform.position.z);
	}

	public void AddLevel (int position, Level level){
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
		//levelTerrain.UpdateTerrainImage (width, height, levels[levelNum].paths);
	}
		

}