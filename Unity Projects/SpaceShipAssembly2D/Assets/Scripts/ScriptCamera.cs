using UnityEngine;
using System.Collections;

public class ScriptCamera : MonoBehaviour {
	public float camMovementSpeed = 10f;

	void Update () {
		if (Input.GetKey (KeyCode.LeftArrow)) {
			transform.position = new Vector3 (transform.position.x - camMovementSpeed * Time.deltaTime, transform.position.y, transform.position.z);
		}
		if (Input.GetKey (KeyCode.UpArrow)) {
			transform.position = new Vector3 (transform.position.x, transform.position.y + camMovementSpeed * Time.deltaTime, transform.position.z);
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			transform.position = new Vector3 (transform.position.x + camMovementSpeed * Time.deltaTime, transform.position.y, transform.position.z);
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			transform.position = new Vector3 (transform.position.x, transform.position.y - camMovementSpeed * Time.deltaTime, transform.position.z);
		}
	}
}
