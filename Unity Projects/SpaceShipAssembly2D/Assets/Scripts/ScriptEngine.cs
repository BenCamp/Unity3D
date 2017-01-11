using UnityEngine;
using System.Collections;

public class ScriptEngine : MonoBehaviour {
	public Rigidbody2D rb;
	public float thrust = 10f;
	public float torque = 20f;

	// Use this for initialization
	void Start () {
		rb = transform.parent.GetComponent <Rigidbody2D> ();
	}

	void FixedUpdate () {
		if (Input.GetKey (KeyCode.Q))
			rb.AddTorque (1 * torque);
		if (Input.GetKey (KeyCode.E))
			rb.AddTorque (-1 * torque);
		if (Input.GetKey (KeyCode.W))
			rb.AddForce (transform.right * thrust);
		if (Input.GetKey (KeyCode.S))
			rb.AddForce (transform.right * -thrust);
		if (Input.GetKey (KeyCode.A))
			rb.AddForce (transform.up * thrust);
		if (Input.GetKey (KeyCode.D))
			rb.AddForce (transform.up * -thrust);

	}
}
