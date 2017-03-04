using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class WeaponManager : ObjectManager {
	public Transform maxRange;
	public float damage = 10f;
	public bool continuous = false;
	public float coolDown = 1f;
	protected bool active = false;
	protected Collider2D colliderId;
	protected int layerID;
	public int layerMask;

	// Use this for initialization
	void Start () {
		IsSelectable = true;
		maxRange = transform.FindChild ("MaxRange");
		colliderId = gameObject.GetComponent<Collider2D> ();

		layerID = LayerMask.NameToLayer ("Default");
		layerMask = 1 << layerID;
		Setup ();
	}

	public void SetShip (ShipManager given){
		shipManager = given;
	}


	public override void StateChange (){
		shipManager.WeaponStateChange ();
	}

	abstract public void Fire ();
	abstract public void Stop ();
	abstract public void Setup ();
}
