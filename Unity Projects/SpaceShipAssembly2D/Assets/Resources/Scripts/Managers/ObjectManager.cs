using UnityEngine;
using System.Collections;

abstract public class ObjectManager : MonoBehaviour {
	private bool isSelectable;
	public bool IsSelectable { get { return isSelectable; } set { isSelectable = value; } }

	public void SpawnPlaceable () {
		GameObject spawn = Instantiate (Resources.Load ("Prefabs/PlacementClonePrefabs/PlacementClone"), Vector3.zero, Quaternion.identity) as GameObject;
		spawn.GetComponent <PlacementCloneManager> ().Spawner = gameObject;
	}
}
