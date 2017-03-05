using UnityEngine;
using System.Collections;

public class EngineManager : ObjectManager {
	private Rigidbody2D body;

	void Start (){
		IsSelectable = true;
		placement = "Exterior";
		objectName = "Engine";
	}

	void Update (){

	}
	public override void StateChange (){
		SetStructure (gameObject.transform.parent.gameObject);
		shipManager.EngineStateChange ();

	}

	public void SetStructure (GameObject body){
		this.body = body.GetComponent <Rigidbody2D> ();
	}

	public void Forward(){
		body.AddForce (body.transform.up * shipManager.Thrust);
	}

	public void Backward () {
		body.AddForce (transform.forward * -shipManager.Thrust);
	}

	public void TurnRight(){
		body.AddTorque (-shipManager.Torque);
	}

	public void TurnLeft(){
		body.AddTorque (shipManager.Torque);
	}

	public GameObject GetParent (){
		return gameObject;
	}
}
