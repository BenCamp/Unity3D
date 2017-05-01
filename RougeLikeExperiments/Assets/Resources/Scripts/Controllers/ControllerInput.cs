using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : MonoBehaviour {
	public static ControllerInput controllerInput;

	public delegate void UserInput (Message message);

	public static event UserInput EventInput;
	void Awake () {
		if (controllerInput == null) {
			DontDestroyOnLoad (gameObject);
			controllerInput = this;
		} else {
			Destroy (gameObject);
		}

	}
}
