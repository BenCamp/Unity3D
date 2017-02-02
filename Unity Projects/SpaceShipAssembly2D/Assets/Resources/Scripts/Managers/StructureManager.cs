using UnityEngine;
using System.Collections;

public class StructureManager : ObjectManager {

	public GameObject wall;
	public GameObject floor;
	public GameObject connection;

	public GameObject[] walls = new GameObject[12];
	public GameObject[] floors = new GameObject[9];
	public GameObject[] connections = new GameObject[4];

	private Vector4 connectionList;

	public Vector4 ConnectionList { get { return connectionList; } }

	void Start (){
		IsSelectable = true;
		connectionList = new Vector4 (1, 1, 1, 1);
	}

	public bool CanThisConnect () {
		if (connectionList.x == 1) {
			if (connections [0].GetComponent<ConnecterConnection>().Connected == null)
				return true;
		}
		if (connectionList.y == 1) {
			if (connections [1].GetComponent<ConnecterConnection>().Connected == null)
				return true;
		}
		if (connectionList.z == 1) {
			if (connections [2].GetComponent<ConnecterConnection>().Connected == null)
				return true;
		}
		if (connectionList.w == 1) {
			if (connections [3].GetComponent<ConnecterConnection>().Connected == null)
				return true;
		}
		return false; 
			
	}
}
