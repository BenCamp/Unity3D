using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputKeys {
	public 	KeyCode		primary	{ get; set; }	
	public 	KeyCode		fire 	{ get; set; }
	//TODO Add other keys

	public InputKeys () {
		
		primary = KeyCode.Mouse0;
		fire 	= KeyCode.Space;
		//TODO Add other key defaults
	}

	private KeyCode MyGetKey (KeyCode previous){
		bool done = false;
		while (!done){
			foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode))) {
				if (Input.GetKey (vKey)) {
					return vKey;
				}
			}
		}
		return previous;
	}

}
