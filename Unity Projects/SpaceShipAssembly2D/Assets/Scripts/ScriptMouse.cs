using UnityEngine;
using System.Collections;

public class ScriptMouse : MonoBehaviour {
	Transform selected;
	bool connecting = false;

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)){
			Vector2 ray = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast (ray, Vector2.zero);
			if (connecting) {
				if (hit) {
					Debug.Log ("ScriptMouse.Update.connecting.hit");
					if (hit.transform.localPosition != hit.transform.parent.localPosition) {
						connect (hit.transform);
					}
				}
			}
			else if (hit) {
				if (hit.transform.parent != null)
					setSelected (hit.transform.parent);
				else
					setSelected (hit.transform);
			} else {
				setSelected (null);
			}
		}

		if (Input.GetKey (KeyCode.C)){
			if (connecting) 
				connecting = false;
			else {
				if (selected != null)
					connecting = true;
			}

		}


	}

	public void setSelected (Transform selection){
		if (connecting) {
			//Need to add logic that checks if there is an available connection
			if (selection != selected) {
				connect (selection); 
			}
		} else {
			selected = selection;
			//Debug.Log (Camera.main.ScreenToWorldPoint(Input.mousePosition));
		}
	}

	public Transform getSelected (){
		return selected;
	}

	public void connect (Transform connectee){
		Debug.Log ("ScriptMouse.connect: " + connectee.parent.name);
		selected.Translate(
	}
}