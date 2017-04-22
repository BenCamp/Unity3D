using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : MonoBehaviour {

	public List<Attribute> GetAttributes (){
		List <Attribute> attributes = new List<Attribute> ();

		Component[] hasAttributes = GetComponents (typeof(IHasAttribute));

		foreach (IHasAttribute att in hasAttributes) {
			attributes.Add (att.GetAttribute ());
		}
	}

}