/*
 * This class handles all functions for generating a new level.
 * 
 * It uses information given it from the world to build an appropriate level.
 * 
 * Right now I'm going to use a simple 2 dim int array to build the map.
 * Based on the numbers in the array, I will then send this back to the world which
 * will build the colliders and textures based on the map.
 * 
 */
#region Using
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion
public static class LevelGenerator {
	public static Level MakeLevel () {
		Level solution = new Level ();
		MapGenerator.MakeMap ();
		return solution;
	}
}