using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StructureManager : MonoBehaviour {
	Hashtable modules = new Hashtable ();
	Rigidbody2D body;
	PolygonCollider2D poly;

	public int thrust { get; set; }
	public int torque { get; set; }
	public bool structureStateChanged { get; set; }

	List <Vector2[]> pathsBuild;
	List <Vector2[]> pathsDamage;

	void Start (){
		structureStateChanged = false;

		thrust = 100;
		torque = 100;
		body = gameObject.GetComponent<Rigidbody2D> ();
		poly = gameObject.GetComponent<PolygonCollider2D> ();
	}

	void Update (){
		if (structureStateChanged) {
			if (pathsBuild.Count > 0) {
				PathFunctions.Addition (poly, pathsBuild);
				pathsBuild.Clear ();
			}
			if (pathsDamage.Count > 0) {
				PathFunctions.Subtraction (poly, pathsDamage);
				pathsDamage.Clear ();
			}
			structureStateChanged = false;
		}
	}



	public List <Vector2[]> NewPathsFromDamage (){
		return new List<Vector2[]> ();
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

	//Recalculate the Center of Mass based on the modules attached to the structure
	public void RecalcCOM () {
	}

	//Recieves paths to add to the structure
	public void StructureAdd (Vector2[] toAdd) {
		pathsBuild.Add (toAdd);
		structureStateChanged = true;
	}

	//Receives paths to remove from the structure
	public void StructureSubtract (Vector2[] damage) {
		pathsDamage.Add (damage);
		structureStateChanged = true;
	}
}