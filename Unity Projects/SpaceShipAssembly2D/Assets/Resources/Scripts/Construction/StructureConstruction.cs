﻿using UnityEngine;
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
		if (structure.ConnectorList != null) {
			BuildStructure (structure.ConnectorList);
			structure.CanThisBePlaced ();
			Destroy (GetComponent <StructureConstruction>());
		}
	}

	public void BuildStructure(Vector4 connectors){
		Floor ();
		NorthWall ();
		EastWall ();
		SouthWall ();
		WestWall ();
		BuildConnectors (connectors);
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
			structure.walls [i + 7].transform.Rotate (Vector3.back* 180);
		}

	}

	private void WestWall () {
		for (int i = -1; i < 2; i++) {
			Vector3 oldPos = new Vector3 (-5.5f, i * 3.5f,0);
			Vector3 newPos = rotMatrix.MultiplyPoint3x4 (oldPos);
			structure.walls [i + 10] = (GameObject)Instantiate (structure.wall, newPos, rotation);
			structure.walls [i + 10].transform.parent = gameObject.transform;
			structure.walls [i + 10].transform.Rotate (Vector3.back* 270);
		}
	}

	private void BuildConnectors(Vector4 Connectors) {
		if (structure.ConnectorList.x != 0) {
			Vector3 oldPos = new Vector3 (0, 11f, 0);
			Vector3 newPos = rotMatrix.MultiplyPoint3x4 (oldPos);
			structure.connectors[0] = (GameObject) Instantiate (structure.connector, newPos, gameObject.transform.rotation);
			structure.connectors[0].transform.parent = gameObject.transform;
			structure.connectors [0].name = "NorthConnector";
		}

		if (structure.ConnectorList.y != 0) {
			Vector3 oldPos = new Vector3 (11f, 0, 0);
			Vector3 newPos = rotMatrix.MultiplyPoint3x4 (oldPos);
			structure.connectors[1] = (GameObject) Instantiate (structure.connector, newPos, gameObject.transform.rotation);
			structure.connectors[1].transform.parent = gameObject.transform;
			structure.connectors [1].name = "EastConnector";
		}

		if (structure.ConnectorList.z != 0) {
			Vector3 oldPos = new Vector3 (0, -11f, 0);
			Vector3 newPos = rotMatrix.MultiplyPoint3x4 (oldPos);
			structure.connectors[2] = (GameObject) Instantiate (structure.connector, newPos, gameObject.transform.rotation);
			structure.connectors[2].transform.parent = gameObject.transform;
			structure.connectors [2].name = "SouthConnector";
		}

		if (structure.ConnectorList.w != 0) {
			Vector3 oldPos = new Vector3 (-11f, 0, 0);
			Vector3 newPos = rotMatrix.MultiplyPoint3x4 (oldPos);
			structure.connectors[3] = (GameObject) Instantiate (structure.connector, newPos, gameObject.transform.rotation);
			structure.connectors[3].transform.parent = gameObject.transform;
			structure.connectors [3].name = "WestConnector";
		}
			
	}
}
