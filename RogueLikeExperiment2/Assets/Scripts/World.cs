using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClipperLib;
using VecPath = System.Collections.Generic.List<UnityEngine.Vector2>;
using VecPaths = System.Collections.Generic.List<System.Collections.Generic.List<UnityEngine.Vector2>>;
using Path = System.Collections.Generic.List<ClipperLib.IntPoint>;
using Paths = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;
public class World : MonoBehaviour {
	public GameObject empty;
	public GameObject border;
	public GameObject door;
	public GameObject hallway;
	public GameObject room;
	public Map map;
	GameObject player;
	Rigidbody2D rig2DPlayer;
	GameObject level;
	PolygonCollider2D levelCollider;
	ShowTerrain levelTerrain;
	List<Level> levels = new List<Level> ();
	int currentlyLoadedLevel = -1;
	Vector2 tempCenter = new Vector2();
	float playerStartX, playerStartY;
	void Start () {
		map = new Map ();
		player = GameObject.Find ("PlayerSprite");
		rig2DPlayer = player.GetComponent<Rigidbody2D> ();
		level = gameObject.transform.Find ("Level").gameObject;
		levelTerrain = level.gameObject.GetComponent<ShowTerrain> ();
		levelCollider = level.gameObject.GetComponent<PolygonCollider2D> ();
		levelCollider.pathCount = 0;

		AddLevel (0, LevelGenerator.MakeLevel ());
		//LoadLevel (0);
		rig2DPlayer.transform.position = new Vector3 ( playerStartX, playerStartY, rig2DPlayer.transform.position.z);
	}
	public void CreateLevel () {
		levelCollider.pathCount = 0;
		//AddLevel (levels.Count, LevelGenerator.MakeLevel ());
		//LoadLevel (levels.Count - 1);
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
	public void MapDebug (Map map){
		int mult = 10;
		BlockDebug bd;
		GameObject temp = new GameObject ();
		for (int j = 1; j < map.sectorMap.GetLength(1); j++) {
			for (int i = 1; i < map.sectorMap.GetLength (0); i++) {
				if (map.sectorMap [i, j].type == Constants.TYPE_EDGE)
					temp = Instantiate (border);
				else if (map.sectorMap[i,j].type == Constants.TYPE_ROOM)
					temp = Instantiate (room);
				else if (map.sectorMap[i,j].type == Constants.TYPE_DOOR)
					temp = Instantiate (door);
				else if (map.sectorMap[i,j].type == Constants.TYPE_EMPTY)
					temp = Instantiate (empty);
				else if (map.sectorMap[i,j].type == Constants.TYPE_HALLWAY)
					temp = Instantiate (hallway);
				temp.gameObject.transform.position = new Vector3(i * mult, -j * mult, 0);
				bd = temp.GetComponent<BlockDebug> ();
				bd.roomNumber = map.sectorMap [i, j].roomNumber;
				bd.count = map.sectorMap [i, j].debugCount;
			}
		}
	}

}