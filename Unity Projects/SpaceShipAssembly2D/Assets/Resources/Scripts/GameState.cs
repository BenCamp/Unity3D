using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {
	private bool paused = true;
	private bool connecting = false;
	public GameObject selected;
	private byte backAndForth = 0;
	private byte leftAndRight = 0;
	private bool followCamera = false;

	public bool Paused { get { return paused; } set { paused = value; } }
	public bool Connecting {
		get { return connecting; }
		set { 
			if (selected != null && !connecting)
				connecting = true;
			else if (connecting)
				connecting = false;
		}
	}

	public GameObject Selected { get { return selected; } set { selected = value; } }

	public byte BackAndForth { get { return backAndForth; } set { backAndForth = value; } }
	public byte LeftAndRight { get { return leftAndRight; } set { leftAndRight = value; } }
	public bool FollowCamera { get { return followCamera; } 
		set {
			if (!followCamera && selected != null)
				followCamera = true;
			else {
				followCamera = false;
			}
		} }
}