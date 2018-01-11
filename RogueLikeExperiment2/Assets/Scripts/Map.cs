#region Using
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion
public class Map {
	public Sector [,] sectorMap { get; set; }
	public List <Room> rooms { get; set; }
	public List <bool> roomParents { get; set; }
	public List <Vector2Int> doors { get; set; }
	public List  <Vector2Int> open { get; set; }
}
