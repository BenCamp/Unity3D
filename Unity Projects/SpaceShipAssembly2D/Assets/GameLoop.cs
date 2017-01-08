using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLoop : MonoBehaviour {
	Dictionary<int, Structure> structures = new Dictionary<int, Structure>();
	Dictionary<int, Wall> walls = new Dictionary<int, Wall>();
	Dictionary<int, Item> items = new Dictionary<int, Item>();

	public void addStructure (Structure structure){
		structures.Add (structure.GetInstanceID(), structure);
	}

	public Structure getStructure (int id){
		return structures [id];
	}

	public void addWall (Wall wall){
		walls.Add (wall.GetInstanceID(), wall);
	}

	public Wall getWall(int id){
		return walls [id];
	}

	public void addItems (Item item){
		items.Add (item.GetInstanceID(), item);
	}

	public Item getItem (int id){
		return items [id];
	}


}
