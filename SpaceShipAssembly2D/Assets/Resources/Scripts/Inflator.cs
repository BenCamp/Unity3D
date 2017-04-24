using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
public static class Inflator {
	public static GameObject Inflate (SaveableObject so){
		GameObject solution = new GameObject ();

		//Add the components to the gameObject
		for(int i = 0; i < so.components.Count; i++){
			solution.AddComponent (so.components [i].type);
		}
		//The so is a SaveableParent
			//For each element in the manifest
				//Create a new gameObject
				//Apply Inflator to corresponding SaveableObject in the manifest

		return solution;
	}
}