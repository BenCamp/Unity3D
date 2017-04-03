using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	public GameObject follow;
	public GameManager gameManager;

	void Start (){
		gameManager = GameObject.Find ("Game").GetComponent<GameManager> ();
	}

	void FixedUpdate () {
		if (gameManager.CameraFollowSelected)
			transform.position = new Vector3 (follow.transform.position.x, follow.transform.position.y, gameObject.transform.position.z);	
	}

	void SetShip (GameObject ship){
		follow = ship;
	}

	public void up () {
		transform.position = new Vector3 (transform.position.x, transform.position.y + 1, transform.position.z);
	}
	public void down () {
		transform.position = new Vector3 (transform.position.x, transform.position.y - 1, transform.position.z);
	}
	public void right () {
		transform.position = new Vector3 (transform.position.x + 1, transform.position.y, transform.position.z);
	}
	public void left () {
		transform.position = new Vector3 (transform.position.x - 1, transform.position.y, transform.position.z);
	}
}
