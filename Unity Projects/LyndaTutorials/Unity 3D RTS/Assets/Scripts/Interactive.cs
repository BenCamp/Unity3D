using UnityEngine;
using System.Collections;

public class Interactive : MonoBehaviour {

	private bool selected = false;
	public bool Selected { get { return selected; } }

	public bool swap = false;

	public void Select (){
		selected = true;
		foreach (var selection in GetComponents <Interaction> ()) {
			selection.Select ();
		}
	}

	public void Deselect () {
		selected = false;
		foreach (var selection in GetComponents <Interaction> ()) {
			selection.Deselect ();
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (swap) {
			swap = false;
			if (selected)
				Deselect ();
			else
				Select ();
		}
	}
}
