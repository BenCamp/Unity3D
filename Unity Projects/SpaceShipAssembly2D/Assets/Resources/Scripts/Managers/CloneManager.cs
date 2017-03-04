using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CloneManager : MonoBehaviour {
	protected bool canPlace = false;
	public bool CanPlace { get { return canPlace; } }

	protected GameObject spawner;
	public GameObject Spawner { get { return spawner; } set { spawner = value; } }

	protected GameState gameState;
	protected float rotationModifier = 0f;
	protected CircleCollider2D circCollider;


	protected void CommonCloneSetup (){
		//Waits for the spawning gameobject to finish preparing the clone
		while (spawner == null);

		gameState = GameObject.Find ("GameManager").GetComponent<GameState> ();
		circCollider = gameObject.GetComponent<CircleCollider2D> ();
		circCollider.isTrigger = true;
	}


	public void KillClone(){
		spawner.GetComponent<ObjectManager> ().IsPlaceable = false;
		Destroy (gameObject);
	}
		

	abstract public void AttemptToAttachClone ();
	abstract public void ConnectorRotationOffset (int change);
}
