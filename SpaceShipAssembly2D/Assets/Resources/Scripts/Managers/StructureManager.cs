using UnityEngine;
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

	List<List<Vector2>> pathsOfStructure = new List<List<Vector2>> ();
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
		//Some thing or things have been added or removed to the structure
		if (structureStateChanged) {
			//Some thing or things have been added to the structure
			if (pathsBuild.Count > 0) {

				//Builds the new path
				pathsBuild = PathFunctions.Addition (ConvertToListList(poly), pathsBuild);

				//Only one path was returned
				if (pathsBuild.Count == 1) {
					poly.SetPath (0, pathsBuild [0].ToArray ());
				}


				//*****
				//Experiment Begin
				//*****
				else {
					poly.pathCount = pathsBuild.Count;
					int i = 0;

					foreach (List<Vector2> path in pathsBuild) {
						poly.SetPath (i, pathsBuild [i].ToArray ());
						i++;
					}

				}
				//*****
				//Experiment End
				//*****


				//Clears the path for next time
				pathsBuild.Clear ();

			}

			//Some thing or things have damaged the structure
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
	//Not sure what I'm going to do with these right now..
	public void CheckState () {
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

	//Converts provided Vector2 array to a Vector2 List
	public List <Vector2> ConvertPathToList (Vector2[] path){
		List <Vector2> temp = new List<Vector2> ();
		foreach (Vector2 point in path) {
			temp.Add (point);
		}
		return temp;
	}

	//Converts privided List<Vector2> to List<List<Vector2>>
	public List<List<Vector2>> ConvertToListList(PolygonCollider2D cols){

		List <List<Vector2>> listlist = new List<List<Vector2>> ();
		for (int i = 0; i < cols.pathCount; i++) {
			listlist.Add(ConvertPathToList(cols.GetPath(i)));
		}
		return listlist;
	}

	//Recieves paths to add to the structure
	public void StructureAdd (Vector2[] toAdd) {
		pathsBuild.Add (ConvertPathToList(toAdd));
		structureStateChanged = true;
	}

	//Receives paths to remove from the structure
	public void StructureSubtract (Vector2[] damage) {
		//pathsDamage.Add (damage);
		//structureStateChanged = true;
	}

	//Calculate the Center of the Polygon in world coordinates based on the path provided
	public Vector2 CalcCenter (Vector2[] path) {
		return new Vector2 ();
	}
}