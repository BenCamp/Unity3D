using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public class MyComponent {

	public FieldInfo[] fields;
	public PropertyInfo[] properties;

	public void SetFieldsAndProperties (Component comp){

		const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

		fields = comp.GetType ().GetFields (flags);

		foreach (FieldInfo fieldInfo in fields) {
			Debug.Log ("Component: " + comp.GetType () + ", Field: " + fieldInfo.Name);
		}

		properties = comp.GetType().GetProperties ();

		foreach (PropertyInfo propertyInfo in properties) {
			Debug.Log ("Component: " + comp.GetType () + ", Property: " + propertyInfo.Name);
		}
	}

}
