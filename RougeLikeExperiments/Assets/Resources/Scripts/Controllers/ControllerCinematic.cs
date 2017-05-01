using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCinematic : MonoBehaviour {

	public static ControllerCinematic controllerCinematic;

	public delegate void Status (Message message);

	public static event Status EventCinematic;

	void Awake () {
		if (controllerCinematic == null) {
			DontDestroyOnLoad (gameObject);
			controllerCinematic = this;
		} else {
			Destroy (gameObject);
		}
	}
}
