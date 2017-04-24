using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public class MyComponent {
	public Type type;

	public FieldInfo[] fieldsInfo;
	public PropertyInfo[] propertiesInfo;
	public object[] fields;
	public object[] properties;


	public void SetFieldsAndProperties (Component comp){

		const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

		type = comp.GetType ();

		fieldsInfo = comp.GetType ().GetFields (flags);
		fields = new object[fieldsInfo.Length];

		propertiesInfo = comp.GetType ().GetProperties (flags);
		properties = new object[propertiesInfo.Length];


		for (int i = 0; i < propertiesInfo.Length; i++) {
			bool notObsolete = true;
			PropertyInfo propertyInfo = propertiesInfo [i];
			Dictionary <string, object> checkForObsolete = new Dictionary <string, object> ();

			foreach (Attribute attribute in propertyInfo.GetCustomAttributes (false)){
				checkForObsolete.Add (attribute.GetType ().Name, attribute);
			}

			foreach (string k in checkForObsolete.Keys)
			{
				if (checkForObsolete[k] is ObsoleteAttribute)
				{
					notObsolete = false;
				}
			}
			if (comp.GetType().GetProperty(propertiesInfo[i].Name) != null && notObsolete){
				properties [i] = (object) comp.GetType().GetProperty(propertiesInfo[i].Name).GetValue(comp, null);
			}
		}

		for (int i = 0; i < fieldsInfo.Length; i++){
			bool notObsolete = true;
			FieldInfo fieldInfo = fieldsInfo[i];
			Dictionary <string, object> checkForObsolete = new Dictionary <string, object> ();

			foreach (Attribute attribute in fieldInfo.GetCustomAttributes (false)){
				checkForObsolete.Add (attribute.GetType ().Name, attribute);
			}

			foreach (string k in checkForObsolete.Keys)
			{
				if (checkForObsolete[k] is ObsoleteAttribute)
				{
					notObsolete = false;
				}
			}
			if (comp.GetType().GetField(fieldsInfo[i].Name) != null && notObsolete){
				fields [i] = (object) comp.GetType().GetField(fieldsInfo[i].Name).GetValue(comp);
			}			
		}

	}


}
