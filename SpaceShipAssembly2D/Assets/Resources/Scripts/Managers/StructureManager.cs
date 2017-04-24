using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using VecPath = System.Collections.Generic.List<UnityEngine.Vector2>;
using VecPaths = System.Collections.Generic.List<System.Collections.Generic.List<UnityEngine.Vector2>>;

//Removed state checkers until I need to do something with them..
public class StructureManager : MonoBehaviour {
	public Rigidbody2D body;
	public PolygonCollider2D poly;
	public Vector2 center = new Vector2 ();

	public int thrust { get; set; }
	public int torque { get; set; }
	public bool structureStateChanged { get; set; }

	public VecPaths pathsOfStructure = new VecPaths ();
	public VecPaths pathsBuild = new VecPaths  ();
	public List <Damage> pathsDamage = new List <Damage> ();
	    
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
				pathsBuild[0] = PathFunctions.Addition (ConvertColliderToVecPaths(poly), pathsBuild);

				SetPolyCollider(pathsBuild [0].ToArray ());
				

				//Clears the path for next time
				pathsBuild.Clear ();

			}

			//Some thing or things have damaged the structure
			if (pathsDamage.Count > 0) {

				foreach (Damage damage in pathsDamage) {
					
				}
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
		



	//
	// Utility
	//

	//TODO Creates new structures
	public void CreateNewStructure (Vector2[] path){
		
	}

	//TODO set PolygonCollider2D to the Vector2[] provided
	public void SetPolyCollider (Vector2[] path){
		poly.SetPath (0, path);
	}

	//TODO take list of children of structure and create gameObjects for them
	public void BuildShipManifest (){

	}

	//TODO Call children to check what their parent is
	//After taking damage the structure may have split, children will need to know
	// which of the new structures is now their parent object
	public void CallChildCheckParent (){

	}

	//Converts provided Vector2 array to a Vector2 List
	public VecPath ConvertPointsToPath (Vector2[] path){
		VecPath  temp = new VecPath ();
		foreach (Vector2 point in path) {
			temp.Add (point);
		}
		return temp;
	}

	//Converts privided List<Vector2> to List<List<Vector2>>
	public VecPaths  ConvertColliderToVecPaths (PolygonCollider2D cols){

		VecPaths  vecPaths = new VecPaths  ();
		for (int i = 0; i < cols.pathCount; i++) {
			vecPaths.Add(ConvertPointsToPath(cols.GetPath(i)));
		}
		return vecPaths;
	}

	//Recieves paths to add to the structure
	public void StructureAdd (Vector2[] toAdd) {
		pathsBuild.Add (ConvertPointsToPath(toAdd));
		structureStateChanged = true;
	}

	//Receives paths to remove from the structure
	public void StructureAddDamage (Damage damage) {
		pathsDamage.Add (damage);
		structureStateChanged = true;
	}

	//Convert Damage to Vector2[]
	public void DamageToVector2Array (Damage damage){
		Vector2[] path;

	}


	//Calculate the Center of the Polygon in world coordinates based on the path provided
	public Vector2 CalcCenter (Vector2[] path) {
		return new Vector2 ();
	}
}