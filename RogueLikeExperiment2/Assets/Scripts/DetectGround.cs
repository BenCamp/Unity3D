using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectGround : MonoBehaviour {

	PlayerActions pA;

	//Variables
	public LayerMask solidLayer;
	public CircleCollider2D cc;
	void Start (){
		pA = gameObject.GetComponentInParent<PlayerActions> ();
		cc = gameObject.GetComponentInParent<CircleCollider2D> ();
	}

	void FixedUpdate () {
		pA.OnSolid(cc.IsTouchingLayers (solidLayer));
		//Physics2D.OverlapCircle (gameObject.transform.position, radius, solidLayer)
	}
}
