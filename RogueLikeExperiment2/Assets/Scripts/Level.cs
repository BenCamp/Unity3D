using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ClipperLib;
using Path = System.Collections.Generic.List<ClipperLib.IntPoint>;
using Paths = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;

public class Level {

	public Paths paths = new Paths ();
	//Object list
	//Monster list
	public Vector2 center { get ; set; }
	public int height { get; set; }
	public int width { get; set; }
	public int difficulty { get; set; }

	public int self { get; set; }
	public int up { get; set; }
	public int down { get; set; }
	public int left { get; set; }
	public int right { get; set; }
	public int inward { get; set; } 
	public int outward { get; set; }
}