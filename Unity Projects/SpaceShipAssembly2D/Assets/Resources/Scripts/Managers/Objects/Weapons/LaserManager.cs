using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : WeaponManager {

	LineRenderer line;
	bool hitSomething;


	void FixedUpdate () {
		 
		if (active) {
			hitSomething = false;
			Vector2 start = new Vector2 (transform.position.x, transform.position.y);
			Vector2 end = new Vector2 (maxRange.position.x, maxRange.position.y);
			line.SetPosition (0, new Vector3(start.x, start.y, 0));
			var hits = Physics2D.LinecastAll (start, end, layerMask);
			foreach (RaycastHit2D hit in hits) {
				if (hit.collider.isTrigger != true && hit.collider.GetInstanceID() != colliderId.GetInstanceID()) {
					line.SetPosition (1, hit.point);
					if (hit.collider.gameObject.GetComponent<DefenseManager> () != null) {
						hit.collider.gameObject.GetComponent<DefenseManager> ().TakeDamage (damage * Time.deltaTime);
					}
					hitSomething = true;
					break;
				}
			}

			if (!hitSomething){
				line.SetPosition (1, end);
			}

			line.enabled = true;
		} else {
			line.enabled = false;
		}
	}

	public override void Setup (){
		placement = "Exterior";
		line = gameObject.GetComponentInChildren<LineRenderer> ();
	}

	public override void Fire (){
		active = true;
	}

	public override void Stop () {
		active = false;
	}
}
