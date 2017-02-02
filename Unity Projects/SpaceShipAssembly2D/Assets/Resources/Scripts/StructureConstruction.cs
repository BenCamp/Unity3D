using UnityEngine;
using System.Collections;

public class StructureConstruction : MonoBehaviour {

	public StructureManager structure;
	private Vector3 translation;
	private Quaternion rotation;
	private Vector3 scale = new Vector3 (1, 1, 1);
	private Matrix4x4 rotMatrix;

	void Start () {
		translation = gameObject.transform.position;
		rotation  = gameObject.transform.rotation;
		rotMatrix  = Matrix4x4.TRS (translation, rotation, scale);
	}

	void Update () {
		if (structure.ConnectionList != null) {
			BuildStructure (structure.ConnectionList);
			Destroy (GetComponent <StructureConstruction>());
		}
	}

	public void BuildStructure(Vector4 connections){
		Floor ();
		NorthWall ();
		EastWall ();
		SouthWall ();
		WestWall ();
		Connections (connections);
	}

	private void Floor (){

		int count = 0;
		for (int i = -1; i < 2; i++) {
			for (int j = -1; j < 2; j++) {
				Vector3 oldPos = new Vector3 ((j*3.5f), i * 3.5f,0);
				Vector3 newPos = rotMatrix.MultiplyPoint3x4 (oldPos);
				structure.floors [count] = (GameObject)Instantiate (structure.floor, newPos, rotation);
				structure.floors [count].transform.parent = gameObject.transform;
				count++;
			}
		}
	}

	private void NorthWall (){
		for (int i = -1; i < 2; i++) {
			Vector3 oldPos = new Vector3 ((i*3.5f), 5.5f,0);
			Vector3 newPos = rotMatrix.MultiplyPoint3x4 (oldPos);
			structure.walls [i + 1] = (GameObject)Instantiate (structure.wall, newPos, rotation);
			structure.walls [i + 1].transform.parent = gameObject.transform;
		}
	}

	private void EastWall () {
		for (int i = -1; i < 2; i++) {
			Vector3 oldPos = new Vector3 (5.5f, i * 3.5f,0);
			Vector3 newPos = rotMatrix.MultiplyPoint3x4 (oldPos);
			structure.walls [i + 4] = (GameObject)Instantiate (structure.wall, newPos, rotation);
			structure.walls [i + 4].transform.parent = gameObject.transform;
			structure.walls [i + 4].transform.Rotate (Vector3.back* 90);
		}
	}

	private void SouthWall () {
		for (int i = -1; i < 2; i++) {
			Vector3 oldPos = new Vector3 ((i*3.5f), -5.5f,0);
			Vector3 newPos = rotMatrix.MultiplyPoint3x4 (oldPos);
			structure.walls [i + 7] = (GameObject)Instantiate (structure.wall, newPos, rotation);
			structure.walls [i + 7].transform.parent = gameObject.transform;
		}

	}

	private void WestWall () {
		for (int i = -1; i < 2; i++) {
			Vector3 oldPos = new Vector3 (-5.5f, i * 3.5f,0);
			Vector3 newPos = rotMatrix.MultiplyPoint3x4 (oldPos);
			structure.walls [i + 10] = (GameObject)Instantiate (structure.wall, newPos, rotation);
			structure.walls [i + 10].transform.parent = gameObject.transform;
			structure.walls [i + 10].transform.Rotate (Vector3.back* 90);
		}
	}

	private void Connections(Vector4 connections) {
		if (structure.ConnectionList.x != 0) {
			Vector3 oldPos = new Vector3 (0, 6.6f, 0);
			Vector3 newPos = rotMatrix.MultiplyPoint3x4 (oldPos);
			structure.connections[0] = (GameObject) Instantiate (structure.connection, newPos, gameObject.transform.rotation);
			structure.connections[0].transform.parent = gameObject.transform;
		}
		if (structure.ConnectionList.y != 0) {
			Vector3 oldPos = new Vector3 (6.6f, 0, 0);
			Vector3 newPos = rotMatrix.MultiplyPoint3x4 (oldPos);
			structure.connections[1] = (GameObject) Instantiate (structure.connection, newPos, gameObject.transform.rotation);
			structure.connections[1].transform.parent = gameObject.transform;
		}
		if (structure.ConnectionList.z != 0) {
			Vector3 oldPos = new Vector3 (0, -6.6f, 0);
			Vector3 newPos = rotMatrix.MultiplyPoint3x4 (oldPos);
			structure.connections[2] = (GameObject) Instantiate (structure.connection, newPos, gameObject.transform.rotation);
			structure.connections[2].transform.parent = gameObject.transform;
		}
		if (structure.ConnectionList.w != 0) {
			Vector3 oldPos = new Vector3 (-6.6f, 0, 0);
			Vector3 newPos = rotMatrix.MultiplyPoint3x4 (oldPos);
			structure.connections[3] = (GameObject) Instantiate (structure.connection, newPos, gameObject.transform.rotation);
			structure.connections[3].transform.parent = gameObject.transform;
		}
			
	}
}
