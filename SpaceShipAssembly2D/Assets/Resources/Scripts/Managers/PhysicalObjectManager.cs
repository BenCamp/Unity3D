using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhysicalObjectManager : MonoBehaviour {
	

	float rotation;
	float xVelocity;
	float yVelocity;
	List<Modifier> modifiers;
	public List<Vector2[]> colliderPaths;
	public List <PhysicalObjectData> manifest;



	public float GetXVelocity (){
		return xVelocity;
	}

	public float GetYVelocity (){
		return yVelocity;
	}

	public float GetRotation (){
		return rotation;
	}

	public List<Modifier> GetModifiers () {
		return modifiers;
	}

	public 
}