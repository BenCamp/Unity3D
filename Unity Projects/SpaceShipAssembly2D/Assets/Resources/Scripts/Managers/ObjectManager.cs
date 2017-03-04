using UnityEngine;
using System.Collections;

abstract public class ObjectManager : MonoBehaviour {
	public GameObject spawn;
	protected string placement;
	protected ShipManager shipManager;
	private bool isSelectable = false;
	public bool IsSelectable { get { return isSelectable; } set { isSelectable = value; } }

	private bool isPlaceable = false;
	public bool IsPlaceable { get { return isPlaceable; } set { isPlaceable = value; } }

	protected string objectName;


	public GameObject SpawnPlaceable (){
		spawn = 
			Instantiate (
				gameObject, 
				Vector3.zero, 
				Quaternion.identity) 
			as GameObject;
		spawn.gameObject.GetComponent<ObjectManager> ().RemoveAllButVisual();
		spawn.gameObject.AddComponent<CircleCollider2D>().isTrigger = true;

		switch (placement){
		case "Exterior":
			spawn.AddComponent <ExteriorPartCloneManager> ();
			break;

		case "Interior":
			spawn.AddComponent <InteriorPartCloneManager> ();
			break;

		case "Structure":
			spawn.AddComponent <StructureCloneManager> ();
			break;

		default:
			break;
		}


		foreach (Transform child in spawn.transform) {
			if (child.gameObject.GetComponent<ObjectManager>() != null){
				child.gameObject.GetComponent<ObjectManager> ().RemoveAllButVisual ();
			}
			if (child.gameObject.name == "NorthConnector") {
				child.gameObject.name = "NorthCloneConnector";
				spawn.GetComponent<StructureCloneManager> ().SetClone (0, child.gameObject);
			}
			if (child.gameObject.name == "EastConnector") {
				child.gameObject.name = "EastCloneConnector";
				spawn.GetComponent<StructureCloneManager> ().SetClone (1, child.gameObject);
			}
			if (child.gameObject.name == "SouthConnector") {
				child.gameObject.name = "SouthCloneConnector";
				spawn.GetComponent<StructureCloneManager> ().SetClone (2, child.gameObject);
			}
			if (child.gameObject.name == "WestConnector") {
				child.gameObject.name = "WestCloneConnector";
				spawn.GetComponent<StructureCloneManager> ().SetClone (3, child.gameObject);
			}
		}

		//Informs the clone it is ready to take over
		spawn.GetComponent<CloneManager> ().Spawner = gameObject;

		return spawn;
	}

	public void RemoveAllButVisual () {
		var components = gameObject.GetComponents<Component>();	
		foreach (var component in components) {
			System.Type componentType = component.GetType ();
			if (componentType.Name != "Transform" && componentType.Name != "SpriteRenderer" && componentType.Name != "Animator"){
				gameObject.GetComponent<ObjectManager>().RemoveComponent(componentType.Name);
			}
		}
	}

	public void SetShip (ShipManager given){
		shipManager = given;
	}

	public virtual bool CanThisBePlaced () {
		return true;
	}

	public void RemoveComponent (string compoName){
		Destroy (gameObject.GetComponent (compoName));
	}

	abstract public void StateChange ();
}
