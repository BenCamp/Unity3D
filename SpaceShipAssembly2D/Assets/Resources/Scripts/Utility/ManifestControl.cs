using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManifestControl : MonoBehaviour {

	//Holds a list of all the gameobjects that aren't a child of another gameobject,
	// organized by their ID
	public Dictionary <int, GameObject> manifest = new Dictionary<int, GameObject> ();

	//Take whats in children and place in manifest dictionary
	public void PlaceChildrenInManifest (){
		if (manifest.Count <= 0) {
			for (int i = 0; i < gameObject.transform.childCount; i++) {
				GameObject child = gameObject.transform.GetChild (i).gameObject;
				manifest.Add(child.GetInstanceID(), child);
			}
		}
	}
		
	//TODO Check manifest



	//Return the manifest as an array of gameobjects
	public GameObject[] GetManifestArray () {

		GameObject[] returnedList = new GameObject[manifest.Count];
		int[] keysList = new int[manifest.Keys.Count];
		manifest.Keys.CopyTo (keysList, 0);

		int i = 0;
		foreach (int key in keysList) {
			returnedList [i] = manifest[key];
		}

		return returnedList;
	}

	//Return a gameobject array as a dictionary
	public Dictionary<int, GameObject> ConvertArrayToDictionary (GameObject[] objects) {
		Dictionary<int, GameObject> solution = new Dictionary<int, GameObject> ();

		return solution;
	}
		
	//TODO Add new child into manifest dictionary

	//TODO Remove child from manifest dictionary

	// Create from manifest
	public void CreateFromManifest (){
		foreach (GameObject go in manifest) {
			
		}
	}
}
