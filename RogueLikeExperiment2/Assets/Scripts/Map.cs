#region Using
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion
public class Map {
	public int current { get; set; }
	public Sector [,] sectorMap { get; set; }
	public List <Room> rooms { get; set; }
	public List <Vector2Int> doors { get; set; }
	public List <Vector2Int> hallways { get; set; }
}
