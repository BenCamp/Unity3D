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
		//Map
		Map solution = BlankMap ();

		//Basic level structure
		solution = GenerateAllRooms (solution);
		solution = GenerateAllDoors (solution);
		solution = GenerateAllHallways (solution);

		//TODO Environmentals

		//TODO Objects

		//TODO Neutrals

		//TODO Hostiles

		//TODO Friendlies

		Debug.Log (DebugMapString (solution.sectorMap));
		return solution;
	}
	#endregion
	#region Map
	static Map BlankMap () {
		Map solution = new Map ();
		int width = Random.Range (Constants.MINLEVELWIDTH, Constants.MAXLEVELWIDTH);
		int height = Random.Range (Constants.MINLEVELHEIGHT, Constants.MAXLEVELHEIGHT);
		solution.current = 0;
		solution.rooms = new List<Room> ();
		solution.doors = new List<Vector2Int> ();
		solution.hallways = new List<Vector2Int> ();
		solution.sectorMap = new Sector[width, height];
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				solution.sectorMap [i, j] = new Sector ();
				if (i == 0 || i == width - 1 || j == 0 || j == height - 1)
					solution.sectorMap [i, j].type = Constants.TYPE_EDGE;
				else 
					solution.sectorMap [i,j].type = Constants.TYPE_EMPTY;
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
		for (int i = -Constants.EDGEBUFFER; i < room.width + Constants.EDGEBUFFER; i++) {
			for (int j = -Constants.EDGEBUFFER; j < room.height + Constants.EDGEBUFFER; j++) {
				if (map.sectorMap [i +  room.upperLeft.x, j + room.upperLeft.y].type == Constants.TYPE_ROOM) {
					solution = false;
				}
			}
		}
		return solution;
	}
	static Map AddRoomToMap (Room room, Map map) {
		Map solution = map;
		for (int i = 0; i < room.width; i++) {
			for (int j = 0; j < room.height; j++) {
				solution.sectorMap [i + room.upperLeft.x, j + room.upperLeft.y].type = Constants.TYPE_ROOM; // Sets each sector covered by the room to the type "room"
				solution.sectorMap [i + room.upperLeft.x, j + room.upperLeft.y].roomNumber = solution.rooms.Count; // Sets each sector covered by the room's roomNumber to the current room
			}
		}
		room.parent = solution.rooms.Count; // Sets the room as a whole's parent to itself
		solution.rooms.Add (room); // Adds the room to the map's list of rooms
		return solution;
	}
	#endregion
	#region Doorways
	static Map GenerateAllDoors (Map map) {
		Map solution = map;
		for (int i = 0; i < map.rooms.Count; i++) { //Go through each room and ensure each has at least 1 door
			solution = GenerateDoorsForRoom (solution, i);
		}
		return solution;
	}
	static Map GenerateDoorsForRoom (Map map, int count) {
		Map solution = map;
		int numberOfEntrances = Random.Range (1, Constants.MAXROOMENTRANCES + 1); // +1 because the max is not inclusive
		for (int i = 0; i < numberOfEntrances; i++) {
			solution = GenerateDoor (solution, count);
		}
		return solution;
	}
	static Map GenerateDoor (Map map, int count) {
		Map solution = map;
		Vector2Int door = new Vector2Int ();
		bool done = false;
		while (!done) {
			done = true;
			int wall = Random.Range (0, 4); //0 - Roof, 1 - Right, 2 - Floor, 3 - Left
			if (wall == 0) {
				door = GenerateRoofDoor (solution, count);
			}
			else if (wall == 1) {
				door = GenerateRightDoor (solution, count);
			}
			else if (wall == 2) {
				door = GenerateFloorDoor (solution, count);
			}
			else if (wall == 3) {
				door = GenerateLeftDoor (solution, count);
			}
			if (door.x == 0)
				done = false;
		}
		solution = AddDoorToMap (solution, door);
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
	static Map AddDoorToMap (Map map, Vector2Int door){
		Map solution = map;
		solution.sectorMap [door.x, door.y].type = Constants.TYPE_DOOR;
		return solution;
	}
	#endregion
	#region Hallways
	static Map GenerateAllHallways (Map map) {
		Map solution = map;
		bool done = false;
		int mapHallwayCount = 0;
		Vector2Int hallway = new Vector2Int ();
		solution = StartHallways (solution);
		while (!done) {
			hallway = GenerateHallway (solution);
			solution.sectorMap [hallway.x, hallway.y].type = Constants.TYPE_HALLWAY;
			solution.hallways.Add (hallway);
			solution = SetConnectionToOtherHallway (solution, hallway);
			solution = SetParentConnections (solution, hallway);
			done = CheckIfAllParentsConnected (solution);
		}
		return solution;
	}
	static Map StartHallways (Map map) {
		Map solution = map;
		Vector2Int temp = new Vector2Int ();
		int room = new int ();
		foreach (Vector2Int door in solution.doors) {
			temp = door;
			room = solution.sectorMap [temp.x, temp.y].roomNumber;
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
			solution.hallways.Add (temp);
		}
		return solution;
	}
	static Vector2Int GenerateHallway (Map map){
		Vector2Int solution = new Vector2Int ();
		//Test if a new hallway can be created there
		int hallway = new int (), direction = new int ();
		bool found = false;
		while (!found) {
			found = false;
			hallway = Random.Range (0, map.hallways.Count); //Select Random Hallway
			direction = Random.Range(0, 4);	//Choose a random direction from that hallway
			solution.x = map.hallways [hallway].x;
			solution.y = map.hallways [hallway].y; 
			if (direction == 0) {
				found = TestIfValidHallwayPosition (map, new Vector2Int (solution.x, solution.y - 1));
				if (found)
					solution.y--;
			}
			else if (direction == 1) {
				found = TestIfValidHallwayPosition (map, new Vector2Int (solution.x + 1, solution.y));
				if (found)
					solution.x++;
			}
			else if (direction == 2) {
				found = TestIfValidHallwayPosition (map, new Vector2Int (solution.x, solution.y + 1));
				if (found)
					solution.y++;
			}
			else if (direction == 3) {
				found = TestIfValidHallwayPosition (map, new Vector2Int (solution.x - 1, solution.y));
				if (found)
					solution.x--;
			}
		}
		return solution;
  	}
	static bool TestIfValidHallwayPosition (Map map, Vector2Int test){
		bool solution = true;
		if (map.sectorMap [test.x, test.y].type != Constants.TYPE_EMPTY)
			solution = false;
		else if (map.sectorMap [test.x - 1, test.y - 1].type == Constants.TYPE_ROOM)
			solution = false;
		else if (map.sectorMap [test.x, test.y - 1].type == Constants.TYPE_ROOM) 
			solution = false;
		else if (map.sectorMap [test.x + 1, test.y - 1].type == Constants.TYPE_ROOM) 
			solution = false;
		else if	(map.sectorMap [test.x - 1, test.y].type == Constants.TYPE_ROOM) 
			solution = false;
		else if	(map.sectorMap [test.x + 1, test.y].type == Constants.TYPE_ROOM) 
			solution = false;
		else if	(map.sectorMap [test.x - 1, test.y + 1].type == Constants.TYPE_ROOM) 
			solution = false;
		else if	(map.sectorMap [test.x, test.y + 1].type == Constants.TYPE_ROOM) 
			solution = false;
		else if	(map.sectorMap [test.x + 1, test.y + 1].type == Constants.TYPE_ROOM) 
			solution = false;
		return solution;
	}
	static Map SetConnectionToOtherHallway (Map map, Vector2Int test) {
		Map solution = map;
		if (solution.sectorMap [test.x, test.y - 1].type == Constants.TYPE_HALLWAY) {
			solution.sectorMap [test.x, test.y].roomNumber = solution.sectorMap [test.x, test.y - 1].roomNumber;
		}
		else if (solution.sectorMap [test.x + 1, test.y].type == Constants.TYPE_HALLWAY) {
			solution.sectorMap [test.x, test.y].roomNumber = solution.sectorMap [test.x + 1, test.y].roomNumber;
		}
		else if (solution.sectorMap [test.x, test.y + 1].type == Constants.TYPE_HALLWAY) {
			solution.sectorMap [test.x, test.y].roomNumber = solution.sectorMap [test.x, test.y + 1].roomNumber;
		}
		else if (solution.sectorMap [test.x - 1, test.y].type == Constants.TYPE_HALLWAY) {
			solution.sectorMap [test.x, test.y].roomNumber = solution.sectorMap [test.x - 1, test.y].roomNumber;
		}
		return solution;
	}
	static Map SetParentConnections (Map map, Vector2Int test) {
		Map solution = map;
		if (solution.sectorMap [test.x, test.y - 1].type == Constants.TYPE_HALLWAY) {
			if (solution.sectorMap [test.x, test.y - 1].roomNumber != solution.sectorMap [test.x, test.y].roomNumber) {
				solution = CheckParentConnections (solution, test, new Vector2Int (test.x, test.y - 1));
			}
		}
		else if (solution.sectorMap [test.x + 1, test.y].type == Constants.TYPE_HALLWAY) {
			if (solution.sectorMap [test.x + 1, test.y].roomNumber != solution.sectorMap [test.x, test.y].roomNumber) {
				solution = CheckParentConnections (solution, test, new Vector2Int (test.x + 1, test.y));
			}
		}
		else if (solution.sectorMap [test.x, test.y + 1].type == Constants.TYPE_HALLWAY) {
			if (solution.sectorMap [test.x, test.y + 1].roomNumber != solution.sectorMap [test.x, test.y].roomNumber) {
				solution = CheckParentConnections (solution, test, new Vector2Int (test.x, test.y + 1));
			}
		}
		else if (solution.sectorMap [test.x - 1, test.y].type == Constants.TYPE_HALLWAY) {
			if (solution.sectorMap [test.x - 1, test.y].roomNumber != solution.sectorMap [test.x, test.y].roomNumber) {
				solution = CheckParentConnections (solution, test, new Vector2Int (test.x - 1, test.y));
			}
		}
		return solution;
	}
	static Map CheckParentConnections (Map map, Vector2Int test1, Vector2Int test2){
		Map solution = map;
		int num1 = solution.sectorMap [test1.x, test1.y].roomNumber;
		int num2 = solution.sectorMap [test2.x, test2.y].roomNumber;
		if (solution.rooms [num1].parent > solution.rooms [num2].parent)
			solution.rooms [num1].parent = solution.rooms [num2].parent;
		else
			solution.rooms [num2].parent = solution.rooms [num1].parent;
		return solution;
	}
	static bool CheckIfAllParentsConnected (Map map) {
		bool solution = true;
		foreach (Room room in map.rooms) {
			if (room.parent != 0) {
				solution = false;
			}
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
	static string DebugMapString (Sector[,] map){
		string solution = "";

		for (int j = 0; j < map.GetLength (1); j++) {
			for (int i = 0; i < map.GetLength (0); i++) {
				if (map [i, j].type == Constants.TYPE_EMPTY) {
					solution += "0";
				}
				else if (map [i, j].type == Constants.TYPE_ROOM) {
					solution += "1";
				}
				else if (map [i, j].type == Constants.TYPE_DOOR) {
					solution += "2";
				}
				else if (map [i, j].type == Constants.TYPE_HALLWAY) {
					solution += "3";
				}
				else if (map [i, j].type == Constants.TYPE_EDGE) {
					solution += "#";
				}
			}
			solution += "\n";

		}

		return solution;
	}
	#endregion
}
