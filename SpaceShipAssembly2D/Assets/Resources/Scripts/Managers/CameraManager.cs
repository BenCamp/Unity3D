using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
	public static CameraManager cam;

	private Camera 				activeCamera;
	private Vector3 			pos;
	public 	GameObject 			follow;

	//Makes sure there is only one CameraManager at a time
	void Awake () {
		if (cam == null) {
			DontDestroyOnLoad (gameObject);
			cam = this;
		} else if (cam != this) {
			Destroy (gameObject);
		}
	}

	void Start () {
		activeCamera = Camera.main;
	}

	void Update () {
		pos = activeCamera.transform.position;
		if (follow != null)
			activeCamera.transform.position = new Vector3 (follow.transform.position.x, follow.transform.position.y, follow.transform.position.z);	
	}
		

	//Basic functions
	public void up () {
		activeCamera.transform.position = new Vector3 (pos.x, pos.y + 1, pos.z);
	}

	public void down () {
		activeCamera.transform.position = new Vector3 (pos.x, pos.y - 1, pos.z);
	}

	public void right () {
		activeCamera.transform.position = new Vector3 (pos.x + 1, pos.y, pos.z);
	}

	public void left () {
		activeCamera.transform.position = new Vector3 (pos.x - 1, pos.y, pos.z);
	}


	//
	//Utility
	//

	public void SetActiveCamera (Camera cam) {
		activeCamera = cam;
	}

	public RaycastHit2D[] MousePoint (){
		return Physics2D.RaycastAll (new Vector2 (activeCamera.ScreenToWorldPoint (Input.mousePosition).x, activeCamera.ScreenToWorldPoint (Input.mousePosition).y), Vector2.zero, Mathf.Infinity);
	}

	public string CameraStatus () {
		if (activeCamera == null) {
			return "No active Camera!";
		}

		if (follow != null) {
			return "Following " + follow.gameObject.name + " at [" + pos.x + ", " + pos.y + "]";
		}

		return "Manual Mode at [" + pos.x + ", " + pos.y + "]";
	}
}
