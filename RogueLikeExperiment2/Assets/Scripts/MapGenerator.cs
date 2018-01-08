#region Using
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion
public static class  MapGenerator {
	#region MapGenerator Structs and Variables
	#endregion
	#region Main
	public static Map MakeMap (){
		World world = GameObject.Find ("World").GetComponent<World> ();

		//Map
		Map solution = BlankMap ();

		//Basic level structure
		solution = GenerateAllRooms (solution);
		solution = GenerateAllDoors (solution);
		solution = GenerateAllEmpty (solution);
		solution = GenerateAllHallways (solution);

		//TODO Environmentals

		//TODO Objects

		//TODO Neutrals

		//TODO Hostiles

		//TODO Friendlies

		//Debugging
		world.MapDebug(solution);

		//Finish
		return solution;
	}

	#endregion
	#region Map
	static Map BlankMap () {
		Map solution = new Map ();
		int width = Random.Range (Constants.MINLEVELWIDTH, Constants.MAXLEVELWIDTH);
		int height = Random.Range (Constants.MINLEVELHEIGHT, Constants.MAXLEVELHEIGHT);
		solution.rooms = new List<Room> ();
		solution.roomParents = new List<RoomParent> ();
		solution.doors = new List<Vector2Int> ();
		solution.open = new List<Vector2Int> ();
		solution.sectorMap = new Sector[width, height];
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				solution.sectorMap [i, j] = new Sector ();
				solution.sectorMap [i, j].roomNumber = -1;
				if (i == 0 || i == width - 1 || j == 0 || j == height - 1) 
					solution.sectorMap [i, j].type = Constants.TYPE_EDGE;
				else {
					solution.sectorMap [i, j].type = Constants.TYPE_EMPTY;
					solution.open.Add (new Vector2Int (i, j));
				}
			}
		}
		return solution;
	}
	#endregion
	#region Rooms
	static Map GenerateAllRooms (Map map) {
		Map solution = map;
		for (int i = 0; i < Constants.MAXTRIES && map.rooms.Count < Constants.MAXROOMS; i++) {
			Room tempRoom = GenerateRoom (solution.sectorMap.GetLength(0), solution.sectorMap.GetLength(1)); // Make a room to TRY to put into map
			if (RoomIsGood (tempRoom, solution)) { // Check that the room is compatible with the map as it stands
				solution = AddRoomToMap (tempRoom, solution); // Add the room to the map
			}
		}	
		return map;
	}
	static Room GenerateRoom (int width, int height){
		Room solution = new Room ();
		solution.width = Random.Range (Constants.MINROOMWIDTH, Constants.MAXROOMWIDTH);
		solution.height = Random.Range (Constants.MINROOMHEIGHT, Constants.MAXROOMHEIGHT);
		solution.upperLeft = new Vector2Int	 (Random.Range(0 + Constants.EDGEBUFFER, width - Constants.EDGEBUFFER - solution.width + 1), Random.Range(0 + Constants.EDGEBUFFER, height - Constants.EDGEBUFFER - solution.height + 1));
		return solution;
	}
	static bool RoomIsGood (Room room, Map map) {
		bool solution = true;
		try {
			for (int i = -Constants.EDGEBUFFER; i < room.width + Constants.EDGEBUFFER; i++) {
				for (int j = -Constants.EDGEBUFFER; j < room.height + Constants.EDGEBUFFER; j++) {
					if (map.sectorMap [i +  room.upperLeft.x, j + room.upperLeft.y].type == Constants.TYPE_ROOM) {
						solution = false;
					}
				}
			}
		} catch {
			solution = false;
		}
		return solution;
	}
	static Map AddRoomToMap (Room room, Map map) {
		Map solution = map;
		RoomParent tempParent = new RoomParent ();
		for (int i = -1; i < room.width + 1; i++) {
			for (int j = -1; j < room.height + 1; j++) {
				if (i >= 0 && i < room.width && j >= 0 && j < room.height){
					solution.sectorMap [i + room.upperLeft.x, j + room.upperLeft.y].type = Constants.TYPE_ROOM; // Sets each sector covered by the room to the type "room"
					solution.sectorMap [i + room.upperLeft.x, j + room.upperLeft.y].roomNumber = solution.rooms.Count; // Sets each sector covered by the room's roomNumber to the current room
				}
				solution.open.Remove(new Vector2Int (i + room.upperLeft.x, j + room.upperLeft.y));
			}
		}
		room.number = solution.rooms.Count; // Sets the room's number
		solution.rooms.Add (room); // Adds the room to the map's list of rooms
		tempParent.rNumber = room.number;
		tempParent.parent = room.number;
		solution.roomParents.Add (tempParent);
		return solution;
	}
	#endregion
	#region Doorways
	static Map GenerateAllDoors (Map map) {
		Map solution = map;
		for (int roomNumber = 0; roomNumber < solution.rooms.Count; roomNumber++) { //Go through each room and ensure each has at least 1 door
			solution = GenerateDoorsForRoom (solution, roomNumber);
		}
		return solution;
	}
	static Map GenerateDoorsForRoom (Map map, int roomNumber) {
		Map solution = map;
		int numberOfEntrances = Random.Range (1, Constants.MAXROOMENTRANCES + 1); // +1 because the max is not inclusive
		for (int entranceNumber = 0; entranceNumber < numberOfEntrances; entranceNumber++) {
			
			solution = GenerateDoor (solution, roomNumber);
		}
		return solution;
	}
	static Map GenerateDoor (Map map, int roomNumber) {
		Map solution = map;
		Vector2Int door = new Vector2Int ();
		bool done = false;
		while (!done) {
			done = true;
			int wall = Random.Range (0, 4); //0 - Roof, 1 - Right, 2 - Floor, 3 - Left
			if (wall == 0) {
				door = GenerateRoofDoor (solution, roomNumber);
			}
			else if (wall == 1) {
				door = GenerateRightDoor (solution, roomNumber);
			}
			else if (wall == 2) {
				door = GenerateFloorDoor (solution, roomNumber);
			}
			else if (wall == 3) {
				door = GenerateLeftDoor (solution, roomNumber);
			}
			if (door.x == 0)
				done = false;
		}
		solution = AddDoorToMap (solution, door, roomNumber);
		solution.doors.Add (door);
		return solution;
	}
	static Vector2Int GenerateRoofDoor (Map map, int count){
		Vector2Int solution = new Vector2Int ();
		solution.y = map.rooms [count].upperLeft.y;
		solution.x = Random.Range (map.rooms [count].upperLeft.x + 1, map.rooms [count].upperLeft.x + map.rooms [count].width - 1);
		if (map.sectorMap [solution.x, solution.y].type == Constants.TYPE_DOOR || map.sectorMap[ solution.x, solution.y].type == Constants.TYPE_EMPTY) {
			solution = new Vector2Int (0, 0);
		}
		if (solution.x != 0) {
			if (map.sectorMap [solution.x -1, solution.y].type == Constants.TYPE_DOOR ||map.sectorMap [solution.x + 1, solution.y].type == Constants.TYPE_DOOR) {
				solution = new Vector2Int (0, 0);
			}
		}
		return solution;
	}
	static Vector2Int GenerateRightDoor (Map map, int count){
		Vector2Int solution = new Vector2Int ();
		solution.y = Random.Range (map.rooms [count].upperLeft.y + 1, map.rooms [count].upperLeft.y + map.rooms [count].height - 1);
		solution.x = map.rooms [count].upperLeft.x + map.rooms[count].width - 1;
		if (map.sectorMap [solution.x, solution.y].type == Constants.TYPE_DOOR || map.sectorMap[ solution.x, solution.y].type == Constants.TYPE_EMPTY) {
			solution = new Vector2Int (0, 0);
		}
		if (solution.x != 0) {
			if (map.sectorMap [solution.x, solution.y - 1].type == Constants.TYPE_DOOR ||map.sectorMap [solution.x, solution.y + 1].type == Constants.TYPE_DOOR) {
				solution = new Vector2Int (0, 0);
			}
		}
		return solution;
	}
	static Vector2Int GenerateLeftDoor (Map map, int count){
		Vector2Int solution = new Vector2Int ();
		solution.y = Random.Range (map.rooms [count].upperLeft.y + 1, map.rooms [count].upperLeft.y + map.rooms [count].height - 1);
		solution.x = map.rooms [count].upperLeft.x;
		if (map.sectorMap [solution.x, solution.y].type == Constants.TYPE_DOOR || map.sectorMap[ solution.x, solution.y].type == Constants.TYPE_EMPTY) {
			solution = new Vector2Int (0, 0);
		}
		if (solution.x != 0) {
			if (map.sectorMap [solution.x, solution.y - 1].type == Constants.TYPE_DOOR || map.sectorMap [solution.x, solution.y + 1].type == Constants.TYPE_DOOR) {
				solution = new Vector2Int (0, 0);
			}
		}
		return solution;
	}
	static Vector2Int GenerateFloorDoor (Map map, int count){
		Vector2Int solution = new Vector2Int ();
		solution.y = map.rooms [count].upperLeft.y + map.rooms[count].height - 1;
		solution.x = Random.Range (map.rooms [count].upperLeft.x + 1, map.rooms [count].upperLeft.x + map.rooms [count].width - 1);
		if (map.sectorMap [solution.x, solution.y].type == Constants.TYPE_DOOR || map.sectorMap[ solution.x, solution.y].type == Constants.TYPE_EMPTY) {
			solution = new Vector2Int (0, 0);
		}
		if (solution.x != 0) {
			if (map.sectorMap [solution.x -1, solution.y].type == Constants.TYPE_DOOR ||map.sectorMap [solution.x + 1, solution.y].type == Constants.TYPE_DOOR) {
				solution = new Vector2Int (0, 0);
			}
		}
		return solution;
	}
	static Map AddDoorToMap (Map map, Vector2Int door, int roomNumber){
		Map solution = map;
		solution.sectorMap [door.x, door.y].type = Constants.TYPE_DOOR;
		solution.sectorMap [door.x, door.y].roomNumber = roomNumber;
		return solution;
	}
	#endregion
	#region Empty
	static Map GenerateAllEmpty (Map map) {
		Map solution = map;
		for (int i = 0; i < Constants.MAXTRIES * 2; i++) {
			Room tempEmptySpace = GenerateEmpty (solution.sectorMap.GetLength(0), solution.sectorMap.GetLength(1)); // Make a negative-room to TRY to put into map
			if (EmptyIsGood (tempEmptySpace, solution)) { // Check that the negative-room is compatible with the map as it stands
				solution = AddEmptyToMap (tempEmptySpace, solution); // Add the negative-room to the map
			}
		}	
		return map;
	}
	static Room GenerateEmpty (int width, int height){
		Room solution = new Room ();
		solution.width = Random.Range (2, Constants.MAXROOMWIDTH);
		solution.height = Random.Range (2, Constants.MAXROOMHEIGHT);
		solution.upperLeft = new Vector2Int	 (Random.Range(0 + Constants.EDGEBUFFER, width - Constants.EDGEBUFFER - solution.width + 1), Random.Range(0 + Constants.EDGEBUFFER, height - Constants.EDGEBUFFER - solution.height + 1));
		return solution;
	}
	static bool EmptyIsGood (Room negRoom, Map map) {
		bool solution = true;
		try {
			for (int i = -Constants.EDGEBUFFER; i < negRoom.width + Constants.EDGEBUFFER; i++) {
				for (int j = -Constants.EDGEBUFFER; j < negRoom.height + Constants.EDGEBUFFER; j++) {
					if (map.sectorMap [i +  negRoom.upperLeft.x, j + negRoom.upperLeft.y].type == Constants.TYPE_ROOM) {
						solution = false;
					}
				}
			}
		} catch {
			solution = false;
		}
		return solution;
	}
	static Map AddEmptyToMap (Room negRoom, Map map) {
		Map solution = map;
		for (int i = 0; i < negRoom.width; i++) {
			for (int j = 0; j < negRoom.height; j++) {
				solution.open.Remove(new Vector2Int (i + negRoom.upperLeft.x, j + negRoom.upperLeft.y));
			}
		}
		return solution;
	}
	#endregion
	#region Hallways
	static Map AddHallway (Map map, int count){//Selects a random open sector and turns it into a hallway
		Map solution = map;
		int openSector = Random.Range (0, solution.open.Count);
		Vector2Int newHallway = solution.open[openSector];
		solution.open.RemoveAt (openSector);
		if (IsHallwayNorth(solution, newHallway))
			solution.sectorMap [newHallway.x, newHallway.y].roomNumber = solution.sectorMap [newHallway.x, newHallway.y - 1].roomNumber;
		else if (IsHallwayEast(solution, newHallway))
			solution.sectorMap [newHallway.x, newHallway.y].roomNumber = solution.sectorMap [newHallway.x + 1, newHallway.y].roomNumber;
		else if (IsHallwaySouth (solution, newHallway))
			solution.sectorMap [newHallway.x, newHallway.y].roomNumber = solution.sectorMap [newHallway.x, newHallway.y + 1].roomNumber;
		else if (IsHallwayWest(solution, newHallway))
			solution.sectorMap [newHallway.x, newHallway.y].roomNumber = solution.sectorMap [newHallway.x - 1, newHallway.y].roomNumber;
		
		if (IsHallway(solution, newHallway))//Has already been combined with another room
			solution = CombineHallways (solution, newHallway);
		else {
			RoomParent tempParent = new RoomParent ();
			tempParent.rNumber = solution.roomParents.Count;
			tempParent.parent = solution.roomParents.Count;
			solution.sectorMap[newHallway.x, newHallway.y].type = Constants.TYPE_HALLWAY;
			solution.sectorMap [newHallway.x, newHallway.y].roomNumber = solution.roomParents.Count;
			solution.roomParents.Add (tempParent);
		}
		solution.sectorMap [newHallway.x, newHallway.y].debugCount = count;
		return solution;
	}
	static bool CheckIfAllRoomsConnected (Map map) {
		bool solution = true;
		for (int i = 0; i < map.rCount; i++) {
			if (map.roomParents[i].parent != 0)
				solution = false;
		}
		return solution;
	}
	static Map CombineHallways (Map map, Vector2Int newHallway) {
		/*
		 * The program will look at each neighbor of the new hallway one at a time.
		 * Recursion: 
		 * 		If the neighbor's room isn't its own parent, then it will jump to the parent
		 * 		Once a room that points to itself is found, it will return that rooms number
		 * 
		 * Recursion:
		 * 		Recieve the List of children
		 * 		If the room has children apply this to each child
		 * 			After each child has copied their numbers to the list, clear the child list
		 * 
		 * 		If the room does not have children copy this rooms parent number to the list
		 * 		return the list
		 * 	
		 */
		Map solution = map;
		List <int> parents = new List<int> ();
		int tempTopParent = -1;
		//North
		if (IsHallwayNorth (solution, newHallway)) {
			tempTopParent = FindTopParent (solution, solution.sectorMap [newHallway.x, newHallway.y - 1].roomNumber);
			parents = FindAllParents (solution, parents, tempTopParent);
		}
		//East
		if (IsHallwayEast (solution, newHallway)) {
			tempTopParent = FindTopParent (solution, solution.sectorMap [newHallway.x + 1, newHallway.y].roomNumber);
			parents = FindAllParents (solution, parents, tempTopParent);
		}
		//South
		if (IsHallwaySouth (solution, newHallway)) {
			tempTopParent = FindTopParent (solution, solution.sectorMap [newHallway.x, newHallway.y + 1].roomNumber);
			parents = FindAllParents (solution, parents, tempTopParent);
		}
		//West
		if (IsHallwayWest (solution, newHallway)) {
			tempTopParent = FindTopParent (solution, solution.sectorMap [newHallway.x - 1, newHallway.y].roomNumber);
			parents = FindAllParents (solution, parents, tempTopParent);
		}
		return solution;
	}
	static List <int> FindAllParents (Map map, List <int> parents, int here){
		List <int> solution = parents;
		if (map.roomParents [here].children.Count > 0) {
			foreach (int p in solution) {
				solution = FindAllParents (map, solution, p);
			}
			map.roomParents [here].children.Clear ();
		}
		solution.Add (here);
		return solution;
	}
	static int FindLowestNumberedParent (List<int> parents){
		int solution = -1;

		return solution;
	}
	static int FindTopParent (Map map, int num){
		int solution = num;
		if (map.roomParents [num].parent != num)
			solution = FindTopParent (map, map.roomParents [num].parent);
		return solution;
	}
	static Map GenerateAllHallways (Map map) {
		Map solution = map;
		bool done = false;
		solution = StartHallways (solution);
		int count = 1;
		while (!done && solution.open.Count != 0 && count % 900 != 0) {
			solution = AddHallway (solution, count);
			done = CheckIfAllRoomsConnected (solution);
			count++;
		}
		return solution;
	}
	static bool IsHallway (Map map, Vector2Int check){
		bool solution = false;
		if (map.sectorMap [check.x, check.y].type == Constants.TYPE_HALLWAY)
			solution = true;
		return solution;
	}
	static bool IsHallwayNorth (Map map, Vector2Int check){
		bool solution = false;
		if (map.sectorMap [check.x, check.y - 1].type == Constants.TYPE_HALLWAY)
			solution = true;
		return solution;
	}
	static bool IsHallwayEast (Map map, Vector2Int check){
		bool solution = false;
		if (map.sectorMap [check.x + 1, check.y].type == Constants.TYPE_HALLWAY)
			solution = true;
		return solution;
	}
	static bool IsHallwaySouth (Map map, Vector2Int check){
		bool solution = false;
		if (map.sectorMap [check.x, check.y + 1].type == Constants.TYPE_HALLWAY)
			solution = true;
		return solution;
	}
	static bool IsHallwayWest (Map map, Vector2Int check){
		bool solution = false;
		if (map.sectorMap [check.x - 1, check.y].type == Constants.TYPE_HALLWAY)
			solution = true;
		return solution;
	}
	static Map StartHallways (Map map) {// Adds a hallway in front of each door to ensure that the hallway system can be built properly
		Map solution = map;
		Vector2Int temp = new Vector2Int ();
		int room = -1;
		foreach (Vector2Int door in solution.doors) {
			temp = door;
			room = solution.sectorMap [door.x, door.y].roomNumber;
			if (door.y == solution.rooms [room].upperLeft.y) {
				temp.y--;
					
			}
			else if (door.x == solution.rooms [room].upperLeft.x + solution.rooms [room].width - 1) {	
				temp.x++;
			}
			else if (door.y == solution.rooms [room].upperLeft.y + solution.rooms [room].height - 1) {
				temp.y++;
			}
			else if (door.x == solution.rooms [room].upperLeft.x) {
				temp.x--;
			}
			solution.sectorMap [temp.x, temp.y].type = Constants.TYPE_HALLWAY;
			solution.sectorMap [temp.x, temp.y].roomNumber = room;

		}
		return solution;
	}
	#endregion
	#region Environmentals
	#endregion
	#region Objects
	#endregion
	#region Neutrals
	#endregion
	#region Hostiles
	#endregion
	#region Friendlies
	#endregion
	#region Debugging
	#endregion
}
