using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FireControl : MonoBehaviour {

	protected string type;
	GameObject MaxRange;

	void Start (){
		Setup ();
		MaxRange = gameObject.transform.GetComponentInChildren<MaxRange> ().gameObject;
	}

	public abstract void FireCommandGiven ();
	public abstract void CeaseFireCommandGiven ();
	public abstract void Setup ();
}
