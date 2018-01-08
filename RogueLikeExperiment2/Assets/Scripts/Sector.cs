#region Using
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion
public class Sector {
	public int debugCount { get; set; }
	public string type { get; set; }
	public int roomNumber { get; set; }
	public List <string> environmentals = new List<string> ();
	public List <string> objects = new List<string> ();
	public List <string> neutrals = new List<string> ();
	public List <string> hostiles = new List<string> ();
	public List <string> friendlies = new List<string> ();
}
