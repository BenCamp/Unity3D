using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public GameObject target;
	public float scale = 4f;

	private Transform trans;

	void Awake(){
		var cam = GetComponent <Camera> ();
		cam.orthographicSize = (Screen.height / 2f) / scale;
	}

	// Use this for initialization
	void Start () {
		trans = target.transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null) {
			transform.position = new Vector3 (trans.position.x, trans.position.y, transform.position.z);
		}
	}
}
