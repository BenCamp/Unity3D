using UnityEngine;
using System.Collections;

public class PowerManager : ObjectManager {

	void Start () {
		IsSelectable = true;
		placement = "Interior";
		objectName = "Power";
	}
	public void SetShip (ShipManager given){
		shipManager = given;
	}

	public override void StateChange () {
		shipManager.PowerStateChange ();
	}
}
