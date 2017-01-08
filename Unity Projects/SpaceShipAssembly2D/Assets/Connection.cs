//To make my life simpler, one connection will only be able to connect to one other connection

using UnityEngine;
using System.Collections;

public class Connection : MonoBehaviour {
	//The maximum tech level of connection for this connection
	private int connectionLevel;

	//The Level of the collidable this connection is connected to
	private int parentLevel;

	//The maximum amount of power that can safely pass through this connection
	//If zero, no power can pass through
	private int power;

	//The connection this connection is connected to
	private Connection connectedTo;

	//Basic getters and setters
	public int getConnectionLevel (){ return connectionLevel; }
	public void setConnectionLevel (int connectionLevel){ this.connectionLevel = connectionLevel; }
	public int getParentLevel(){ return parentLevel; }
	public void setParentLevel(int parentLevel){ this.parentLevel = parentLevel; } 
	public int getPower () { return power; }
	public void setPower (int power) { this.power = power; }


	public void connect (Connection connection){
		//TODO Attach the parents of the connectors together
	}
}
