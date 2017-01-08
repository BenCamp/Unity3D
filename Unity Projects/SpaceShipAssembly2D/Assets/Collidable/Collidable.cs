using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Collidable : MonoBehaviour {
	//The basic mass of this collidable
	int baseMass;

	//Whether or not this object is attachable
	bool canBeAttached;

	//The list of all objects attached to this object
	List <Collidable> collidables = new List<Collidable> ();

	//Basic Getters and Setters
	public void setBaseMass(int mass){ this.baseMass = mass; }
	public int getBaseMass(){ return this.baseMass; }
	public void setCanBeAttached (bool isAttachable){ this.canBeAttached = isAttachable; }
	public bool getCanBeAttached(){	return canBeAttached; }

	public void attachToOtherCollidable(){ 
		//TODO attach the collidables together
	}
}