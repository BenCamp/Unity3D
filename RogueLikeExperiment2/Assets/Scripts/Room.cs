using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {
	public int width;
	public int height;
	public int parent;
	public Vector2Int upperLeft;
	public List <Vector2Int> doors;
}
