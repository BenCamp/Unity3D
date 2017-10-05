using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectGround : MonoBehaviour {

	PlayerActions pA;

	//Variables
	public float radius = 0.08f;
	public LayerMask solidLayer;

	void Start (){
		pA = gameObject.GetComponentInParent<PlayerActions> ();
	}

	void FixedUpdate () {
		pA.OnSolid(Physics2D.OverlapCircle (gameObject.transform.position, radius, solidLayer));
	}
}
