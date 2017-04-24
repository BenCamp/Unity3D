using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Deflator {


	//TODO
	//TODO Hmmm.. kinda messy
	public static SaveableObject Deflate (GameObject go){

		//go doesn't exist
		if (go == null) {
			return null;
		}

		// go is not a SaveableObject
		if (go.GetComponent<SaveableObject>() == null) {
			return null;
		} 

		// go is a SaveableObject
		else {

			// go is a SaveableParent Object, has a manifest
			if (go.GetComponent<SaveableObject> ().canBeParent) {
				SaveableParent soParent = (SaveableParent)go.GetComponent <SaveableObject> ();
				soParent.manifest = GetManifest (go);

				soParent.components = GetMyComponents (go);

				return soParent;
			} 

			// go is a SaveableNotParent Object
			else {
				SaveableNotParent soNotParent = (SaveableNotParent)go.GetComponent <SaveableObject> ();

				soNotParent.components = GetMyComponents (go);

				return soNotParent;
			}
		}
	}


	//
	//Utility
	//

	private static List<MyComponent> GetMyComponents (GameObject go){
		
		List<MyComponent> solution = new List<MyComponent> ();

		Component[] thisObjectsComponents = go.GetComponents(typeof(Component));
		MyComponent myComponent;

		for (int i = 0; i < thisObjectsComponents.Length; i++) {
			myComponent = new MyComponent ();
			myComponent.SetFieldsAndProperties (thisObjectsComponents [i]);
			solution.Add (myComponent);
		}

		return solution;
	}


	private static List <SaveableObject> GetManifest (GameObject go){

		List <SaveableObject> solution = new List <SaveableObject> ();

		for (int i = 0; i < go.transform.childCount; i++) {

			solution.Add (Deflate (go.transform.GetChild (i).gameObject));

		}

		return solution;
	}
}
