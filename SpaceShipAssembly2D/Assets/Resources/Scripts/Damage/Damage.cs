using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damage {

	string type;
	Vector2 origin;
	double amount;

	List<Modifier> modifier;

	public Shape shape;

	public void Hello () {
		Debug.Log ("Damage!");
	}

}