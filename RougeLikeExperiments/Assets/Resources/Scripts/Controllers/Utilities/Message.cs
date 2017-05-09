/* FIRST! Beelzabee :P
 * 
 * I'll probably change this class to abstract and create different styles of 
 * messages for each type of gameObject that needs to communicate with the 
 * ControllerGame class. 
 * e.g.: MessageController for controllers and MessageActor for gameObjects such
 * as the Player and monsters.
 * For now this will do as I'm figuring out what data I need to communicate.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message {

	/***Variables***/
	public string scene { get; set; }
	public string data { get; set; }

	/***Constructors***/
	public Message (){
		scene = "";
		data = "";
	}
	public Message (string scene, string data){
		this.scene = scene;
		this.data = data;
	}
}