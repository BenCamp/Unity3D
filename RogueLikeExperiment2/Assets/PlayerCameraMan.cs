using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraMan : MonoBehaviour {


	Rigidbody2D rig2D;
	Rigidbody2D playerRig2D;
	GameObject player;
	Vector2 velocity;
	Vector2 position;

	public float positionTolerance;
	public LayerMask worldBounds;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("PlayerSprite");
		playerRig2D = player.GetComponent<Rigidbody2D> ();	
		rig2D = gameObject.GetComponent<Rigidbody2D> ();
		gameObject.transform.position = player.transform.position;
		position = new Vector2 ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		velocity = playerRig2D.velocity;
		if (rig2D.position != playerRig2D.position) {
			if (rig2D.position.x != playerRig2D.position.x) {
				velocity.x = (playerRig2D.position.x - rig2D.position.x) * positionTolerance;
			}
			if (rig2D.position.y != playerRig2D.position.y) {
				velocity.y = (playerRig2D.position.y - rig2D.position.y) * positionTolerance;
			}
		}	

		rig2D.velocity = velocity;

	}
	//Functions

}