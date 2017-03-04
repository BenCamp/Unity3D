using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {
	private bool paused = false;
	public bool Paused { get { return paused; } set { paused = value; } }

	private bool placing = false;
	public bool Placing{ get { return placing; } set { placing = value; } }

	public GameObject selected;
	public GameObject Selected { get { return selected; } set { selected = value; } }

	private byte backAndForth = 0;
	public byte BackAndForth { get { return backAndForth; } set { backAndForth = value; } }

	private byte leftAndRight = 0;
	public byte LeftAndRight { get { return leftAndRight; } set { leftAndRight = value; } }

	private bool followCamera = false;
	public bool FollowCamera { get { return followCamera; } 
		set {
			if (!followCamera && selected != null)
				followCamera = true;
			else {
				followCamera = false;
			}
		} }
}