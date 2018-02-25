#region Using
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion
public class Level {
	public Map map { get; set; }
	public List <Vector2[]> paths { get; set; }
	//Object list
	//Monster list
	public int height { get; set; }
	public int width { get; set; }
	public int difficulty { get; set; }
	public int self { get; set; }
	#region Adjacent Levels
	public int up { get; set; }
	public int down { get; set; }
	public int left { get; set; }
	public int right { get; set; }
	public int inward { get; set; } 
	public int outward { get; set; }
	#endregion
}