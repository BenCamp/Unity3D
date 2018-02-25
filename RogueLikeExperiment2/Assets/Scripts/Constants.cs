public static class Constants {
	#region GeneralGame
	public const int PLAYERHEIGHT = 64;

	public const int RESOLUTION = 4;

	public const int EDGEBUFFER = 3;

	public const int MAXLEVELHEIGHT = 50;
	public const int MINLEVELHEIGHT = 20;
	public const int MAXLEVELWIDTH = 50;
	public const int MINLEVELWIDTH = 20;

	public const int MAXROOMWIDTH = 6;
	public const int MINROOMWIDTH = 3;
	public const int MAXROOMHEIGHT  = 6;
	public const int MINROOMHEIGHT = 3;
	public const int MAXTRIES = 100;
	public const int MAXROOMS = 20;
	public const int MAXROOMENTRANCES = 3;
	#endregion
	#region Strings
	public const string TYPE_EMPTY = "EMPTY";
	public const string TYPE_ROOM = "ROOM";
	public const string TYPE_HALLWAY = "HALLWAY";
	public const string TYPE_DOOR = "DOOR";
	public const string TYPE_EDGE = "EDGE";
	#endregion
	#region Directions
	public const int DIR_NONE = 0;
	public const int DIR_NORTH = 1;
	public const int DIR_EAST = 2;
	public const int DIR_SOUTH = 3;
	public const int DIR_WEST = 4;
	#endregion
}
