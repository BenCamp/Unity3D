using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour {
	/***GameObjects***/
	Rigidbody2D rig2D;
	PhysicsMaterial2D mat;
	SpriteRenderer sprite;
	Animator animator;

	/***Variables***/
	public int xAxisForce = 1000;
	public int xAxisMaxVelocity = 7;

	public int yAxisForce = 20000;
	public int yAxisMaxVelocity = 10;

	public int jumpDelay = 20;
	int jumpDelayNow = 0;

	public float friction = 10f;

	short playerMoveX, playerMoveY;

	bool onSolid = false;


	/***MonoBehaviour Functions***/
	void Start () {
		rig2D = gameObject.GetComponent<Rigidbody2D> ();
		mat = rig2D.sharedMaterial;
		mat.friction = 0f;
		sprite = gameObject.GetComponent<SpriteRenderer> ();
		animator = gameObject.GetComponent<Animator> ();
	}

	void FixedUpdate (){
		//Getting current state of velocity
		playerMoveX = GameManager.gameManager.playerMoveX;
		playerMoveY = GameManager.gameManager.playerMoveY;

		//Adding force to object
		//X-axis forces
		if (playerMoveX != 0) {
			rig2D.AddForce (new Vector2 (playerMoveX * xAxisForce, 0));
			if (playerMoveX > 0) {
				if (rig2D.velocity.x < 0) {
					rig2D.AddForce (FrictionMove());
				}

				sprite.flipX = false;
			}
			else {
				if (rig2D.velocity.x > 0) {
					rig2D.AddForce (FrictionMove());
				}
				sprite.flipX = true;
			}
			GameManager.gameManager.PlayerMoveX (0);
		}
		else {
			rig2D.AddForce(FrictionStop ());
		}

		//Y-axis forces
		if (playerMoveY > 0) {
			if (onSolid && jumpDelayNow <= 0) {
				rig2D.AddForce (new Vector2 (0, yAxisForce)); 
				jumpDelayNow = jumpDelay;
			}
		}
		if (playerMoveY < 0) {

		}

		if (jumpDelayNow > 0) {
			jumpDelayNow--;
		}

		GameManager.gameManager.PlayerMoveY (0);


		//Setting the animation state
		if (onSolid) {
			if (playerMoveX != 0) {
				animator.SetInteger ("state", 1);
			}
			else {
				animator.SetInteger ("state", 0);
			}
		}
		else {
			animator.SetInteger ("state", 2);
		}


		//Constraing the velocity of the object on the x axis to the xAxisMaxVelocity
		if (rig2D.velocity.x < -1 * xAxisMaxVelocity) {
			rig2D.velocity = new Vector2 (-1 * xAxisMaxVelocity, rig2D.velocity.y);
		}
		if (rig2D.velocity.x > xAxisMaxVelocity){
			rig2D.velocity = new Vector2 (xAxisMaxVelocity, rig2D.velocity.y);
		}

		//Constraing the velocity of the object on the y axis to the yAxisMaxVelocity
		if (rig2D.velocity.y < -1 * yAxisMaxVelocity) {
			rig2D.velocity = new Vector2 (rig2D.velocity.x, -1 * yAxisMaxVelocity);
		}
		if (rig2D.velocity.y > yAxisMaxVelocity){
			rig2D.velocity = new Vector2 (rig2D.velocity.x, yAxisMaxVelocity);
		}
	}


	/***Functions***/
	public void OnSolid (bool value) {
		onSolid = value;
	}

	Vector2 FrictionMove () {
		return new Vector2 (- (rig2D.velocity.x * friction)/* x */ , 0 /* y */);
	}

	Vector2 FrictionStop () {
		if (Mathf.Abs (rig2D.velocity.x) < 1f) {
			rig2D.velocity = new Vector2 (0, rig2D.velocity.y);
			return new Vector2 (0, 0);
		}
		else {
			return new Vector2 (- (rig2D.velocity.x * friction)/* x */ , 0 /* y */);
		}

	}


	///END OF CLASS
}