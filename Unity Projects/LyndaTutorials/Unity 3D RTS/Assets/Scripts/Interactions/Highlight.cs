using UnityEngine;
using System.Collections;

public class Highlight : Interaction {

	public GameObject displayItem;

	public override void Deselect ()
	{
		displayItem.SetActive (false);
	}

	public override void Select ()
	{
		displayItem.SetActive (true);
	}

	// Use this for initialization
	void Start () {
		displayItem.SetActive (false);
	}
}
