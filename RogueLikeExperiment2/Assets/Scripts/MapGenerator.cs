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
		solution.roomParents = new List<bool> ();
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
		if (solution.roomParents.Count == 0)
			solution.roomParents.Add (true);
		else 
			solution.roomParents.Add (false);
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
		solution.open.Remove (new Vector2Int (door.x, door.y));
		return solution;
	}
	#endregion
	#region Empty
	static Map GenerateAllEmpty (Map map) {
		Map solution = map;
		for (int i = 0; i < Constants.MAXTRIES * 5; i++) {
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
	static bool EmptyIsGood (Room emptyRoom, Map map) {
		bool solution = true;
		try {
			for (int i = -Constants.EDGEBUFFER; i < emptyRoom.width + Constants.EDGEBUFFER; i++) {
				for (int j = -Constants.EDGEBUFFER; j < emptyRoom.height + Constants.EDGEBUFFER; j++) {
					if (map.sectorMap [i +  emptyRoom.upperLeft.x, j + emptyRoom.upperLeft.y].type == Constants.TYPE_ROOM) {
						solution = false;
					}
				}
			}
		} catch {
			solution = false;
		}
		return solution;
	}
	static Map AddEmptyToMap (Room emptyRoom, Map map) {
		Map solution = map;
		for (int i = 0; i < emptyRoom.width; i++) {
			for (int j = 0; j < emptyRoom.height; j++) {
				solution.open.Remove(new Vector2Int (i + emptyRoom.upperLeft.x, j + emptyRoom.upperLeft.y));
			}
		}
		return solution;
	}
	#endregion
	#region Hallways
	static Map GenerateAllHallways (Map map) {
		Map solution = map;
		int nextInt;
		Vector2Int nextVec;
		bool done = false;
		solution = StartHallways (solution);
		while (!done && solution.open.Count != 0) {
			nextInt = Random.Range (0, solution.open.Count);
			nextVec = solution.open [nextInt];
			solution.open.RemoveAt (nextInt);
			solution.sectorMap [nextVec.x, nextVec.y].type = Constants.TYPE_HALLWAY;
			if (NextToZero (nextVec, solution)) {
				solution = SetConnectedHallwaysToZero (nextVec, solution);
				done = CheckParents (map);
			}
			else
				solution.sectorMap [nextVec.x, nextVec.y].roomNumber = 1;
		}
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
			solution.open.Remove (new Vector2Int (temp.x, temp.y));
		}
		return solution;
	}
	static bool NextToZero (Vector2Int check, Map map){
		bool solution = false;
		if (map.sectorMap [check.x, check.y - 1].roomNumber == 0)
			solution = true;
		if (map.sectorMap [check.x + 1, check.y].roomNumber == 0)
			solution = true;
		if (map.sectorMap [check.x, check.y + 1].roomNumber == 0)
			solution = true;
		if (map.sectorMap [check.x - 1, check.y].roomNumber == 0)
			solution = true;
		return solution;
	}
	static Map SetConnectedHallwaysToZero (Vector2Int nextVec, Map map){
		Map solution = map;
		int temp;
		Vector2Int north = new Vector2Int (nextVec.x, nextVec.y - 1);
		Vector2Int east = new Vector2Int (nextVec.x + 1, nextVec.y);
		Vector2Int south = new Vector2Int (nextVec.x, nextVec.y + 1);
		Vector2Int west = new Vector2Int (nextVec.x - 1, nextVec.y);
		solution.sectorMap [nextVec.x, nextVec.y].roomNumber = 0;
		temp = CheckSector (north, solution);
		if (temp > 0)
			solution = ChangeParent (temp, solution);
		else if (temp == -1)
			solution = SetConnectedHallwaysToZero(north, solution);
		temp = CheckSector (east, solution);
		if (temp > 0)
			solution = ChangeParent (temp, solution);
		else if (temp == -1)
			solution = SetConnectedHallwaysToZero(east, solution);
		temp = CheckSector (south, solution);
		if (temp > 0)
			solution = ChangeParent (temp, solution);
		else if (temp == -1)
			solution = SetConnectedHallwaysToZero(south, solution);
		temp = CheckSector (west, solution);
		if (temp > 0)
			solution = ChangeParent (temp, solution);
		else if (temp == -1)
			solution = SetConnectedHallwaysToZero(west, solution);
		return solution;
	}
	static Map ChangeParent (int change, Map map){
		Map solution = map;
		solution.roomParents [change] = true;
		return solution;
	}
	static bool CheckParents (Map map){
		bool solution = true;
		foreach (bool p in map.roomParents) {
			if (!p)
				solution = false;
		}
		return solution;
	}
	static int CheckSector (Vector2Int check, Map map){
		int solution = 0;
		Sector temp = map.sectorMap [check.x, check.y];
		if (temp.type == Constants.TYPE_DOOR)
			solution = temp.roomNumber;
		else if (temp.type == Constants.TYPE_HALLWAY && temp.roomNumber != 0)
			return -1;
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
