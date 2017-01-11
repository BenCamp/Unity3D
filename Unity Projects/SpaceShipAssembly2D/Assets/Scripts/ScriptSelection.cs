/*
 * This script will be for anything relating to an already selected object
 * In the future I want this thing to pause itself when nothing is selected
 * and be started by scripts that select objects such as ScriptMouse
 */

using UnityEngine;
using System.Collections;

public class ScriptSelection : MonoBehaviour {
	Transform selected;
	bool connecting = false;

	public void setSelected (Transform selection){
		if (connecting) {
			//Need to add logic that checks if there is an available connection
			if (selection != selected) {
				connect (selection); 
			}
		} else {
			selected = selection;
			Debug.Log (Camera.main.ScreenToWorldPoint(Input.mousePosition));
		}
	}

	public Transform getSelected (){
		return selected;
	}

	public void connect (Transform connectee){
		//TODO ScriptSelection.connect
	}

	void Update(){
		if (Input.GetKey (KeyCode.C)){
			if (connecting)
				connecting = false;
			else {
				if (selected != null)
					connecting = true;
			}

		}
	}
}