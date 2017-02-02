using UnityEngine;
using System.Collections;

public class EngineThrust : MonoBehaviour {
	public int thrust = 200;
	public int torque = 10;

	private Rigidbody2D body;

	void Start (){
		body = gameObject.GetComponentInChildren <Rigidbody2D> ();
	}
	public void forward(){
		body.AddForce (body.transform.up * thrust);
	}
	public void backward () {
		body.AddForce (transform.forward * -thrust);
	}

	public void turnRight(){
		body.AddTorque (-torque);
	}

	public void turnLeft(){
		body.AddTorque (torque);
	}
}
