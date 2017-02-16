using UnityEngine;
using System.Collections;

abstract public class ObjectManager : MonoBehaviour {
	protected ShipManager shipManager;
	private bool isSelectable = false;
	public bool IsSelectable { get { return isSelectable; } set { isSelectable = value; } }

	private bool isPlaceable = false;
	public bool IsPlaceable { get { return isPlaceable; } set { isPlaceable = value; } }

	protected string objectName;


	public GameObject SpawnPlaceable (){
		GameObject spawn = 
			Instantiate (
				Resources.Load (
					"Prefabs/PlacementClonePrefabs/"
					+ objectName
					+ "Clone"), 
				Vector3.zero, 
				Quaternion.identity) 
			as GameObject;
		spawn.GetComponent <CloneManager> ().Spawner = gameObject;
		return spawn;
	}

	public void SetShip (ShipManager given){
		shipManager = given;
	}

	public virtual bool CanThisBePlaced () {
		return true;
	}
	abstract public void StateChange ();
}
