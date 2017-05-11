﻿/*First! Beelzabee :P
 * 
 * Creating this class to clean up my code a bit visually.
 * Also gives me a centralized place to insure all my error code is 
 * uniformally formated.
 * Using a switch statement since its all based on strings.
 * Supposedly this is significantly faster with large numbers of strings.
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Static Class*/
public static class ErrorStrings {
	public static string GetError(string code){
		switch (code) {
		case "CG0001":
				return "ControllerGame -> Update -> isMenuOpen -> TRUE -> SCENE_ProgramLaunched -> ControllerBuilder message: Builder shouldn't be sending this kind of data";
			break;
		case "CG0002":
			return "ControllerGame -> Update -> isMenuOpen -> TRUE -> SCENE_ProgramLaunched -> ContorllerBuilder message: Builder shouldn't be sending this kind of scene";
			break;
		case "CG0003":
			return "ControllerGame.Update: Something exploded in the GUI: ";
			break;
		case "CG0004":
			return "ControllerGame.Update: Something exploded in the Cinematic: ";
			break;
		case "CG0005":
			return "ControllerGame.Update: Something exploded in the Input: ";
			break;
		case "CG0006":
			return "ControllerGame.Update: Something exploded in the Builder: ";
			break;
		case "CG0007": 
			return "ControllerGame -> Update -> isMenuOpen -> TRUE: There shouldn't be any circumstance that isMenuOpen is true if you aren't paused";
			break;
		case "CG0008":
			return "ControllerGame -> Update -> isMenuOpen -> FALSE -> PlayingGame: There shouldn't be any circumstance that isMenuOpen is false if you aren't in SCENE_PlayingGame";
			break;
		case "CI0001":
			return "ControllerInput -> Update -> ControllerGame message: currentScene does not match provided Scene name";
			break;
		default:
			return "Uhhh an error with the error system?";
		}
	}
}