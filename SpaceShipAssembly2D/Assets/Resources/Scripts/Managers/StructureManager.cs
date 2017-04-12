﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StructureManager : MonoBehaviour {
	private Hashtable modules = new Hashtable ();
	private Rigidbody2D body;
	private PolygonCollider2D poly;
	private Vector2 center = new Vector2 ();


	public int thrust { get; set; }
	public int torque { get; set; }
	public bool structureStateChanged { get; set; }


	List<List<Vector2>> pathsBuild = new List<List<Vector2>> ();
	List<List<Vector2>> pathsDamage = new List<List<Vector2>> ();

	void Start (){
		structureStateChanged = false;
	
		thrust = 100;
		torque = 100;
		body = gameObject.GetComponent<Rigidbody2D> ();
		poly = gameObject.GetComponent<PolygonCollider2D> ();
	}

	void Update (){
		if (structureStateChanged) {
			Debug.Log ("Structure State Changed");
			if (pathsBuild.Count > 1) {
				pathsBuild = PathFunctions.Addition (pathsBuild);
				poly.SetPath (0, pathsBuild[0].ToArray());
				if (pathsBuild.Count > 1) {
				}

				pathsBuild.Clear ();

				StructureAdd (poly.GetPath (0));
			}
			if (pathsDamage.Count > 0) {
				//PathFunctions.Subtraction (poly, pathsDamage);
				pathsDamage.Clear ();
			}
			structureStateChanged = false;
		}
	}
		
	//Directional controls
	public void ForwardCommandGiven () {
		body.AddForce (new Vector2(transform.up.x * thrust * Time.deltaTime, transform.up.y * thrust * Time.deltaTime));
	}

	public void LeftCommandGiven () {
		body.AddTorque (torque * Time.deltaTime);
	}

	public void BackCommandGiven () {

		body.AddForce (new Vector2(-transform.up.x * thrust * Time.deltaTime, -transform.up.y * thrust * Time.deltaTime));
	}

	public void RightCommandGiven () {
		body.AddTorque (-torque * Time.deltaTime);
	}


	//State checkers, basically has anything changed since the last time the state was checked
	public void CheckState () {
		for (int childNum = 0; childNum < transform.childCount; childNum++) {
			GameObject child = transform.GetChild (childNum).gameObject;
			if (!modules.ContainsKey(child.GetInstanceID())){
				modules.Add(child.GetInstanceID(), child);
			}
		}

		RecalcCOM ();
	}
	public void StructureCheckState () {
	}

	public void ConsoleCheckState () {
	}

	public void ConnectionCheckState () {
	}

	public void EngineCheckState () {
	}

	public void PowerCheckState () {
	}

	public void WeaponCheckState () {
	}


	//For Experimental purposes, final game should have selectable weapons
	public void FireWeapons () {
	}

	public void CeaseFire () {
	}
		



	//
	// Utility
	//

	//Recieves paths to add to the structure
	public void StructureAdd (Vector2[] toAdd) {

		//The path is a valid path to add
		if (ValidAddition (toAdd)) {
			List <Vector2> temp = new List<Vector2> ();
			foreach (Vector2 point in toAdd) {
				temp.Add (point);
			}
			pathsBuild.Add (temp);
			structureStateChanged = true;
		}
	}

	//Receives paths to remove from the structure
	public void StructureSubtract (Vector2[] damage) {
		//pathsDamage.Add (damage);
		//structureStateChanged = true;
	}

	//Calculate the Center of the Polygon in world coordinates based on the path provided
	public void CalcCenter (Vector2[] path) {
		
	}

	public bool ValidAddition (Vector2[] toAdd){
		return true;
	}
}