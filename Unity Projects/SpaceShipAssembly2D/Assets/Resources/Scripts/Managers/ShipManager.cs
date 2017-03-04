using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ShipManager : MonoBehaviour {
	private bool hasBridge = false;
	public bool HasBridge { get { return hasBridge; } set { hasBridge = value; } }

	private Dictionary<int, EngineManager> engineList = new Dictionary<int, EngineManager>();
	private Dictionary<int, PowerManager> powerList = new Dictionary<int, PowerManager> ();
	private Dictionary<int, WeaponManager> weaponList = new Dictionary<int, WeaponManager> ();

	private Dictionary<int, GameObject> cargoList = new Dictionary<int, GameObject> ();

	private int thrust = 100;
	public int Thrust { get { return thrust; } set { thrust = value; } }

	private int torque = 100;
	public int Torque { get { return torque; } set { torque = value; } }

	public void ForwardCommandGiven () {
		int[] keyList = engineList.Keys.ToArray();
		foreach(int key in keyList)
			engineList[key].Forward ();
	}

	public void LeftCommandGiven () {
		int[] keyList = engineList.Keys.ToArray();
		foreach(int key in keyList)
			engineList[key].TurnLeft ();
	}

	public void RightCommandGiven () {
		int[] keyList = engineList.Keys.ToArray();
		foreach(int key in keyList)
			engineList[key].TurnRight ();
	}

	public void ConnectionStateChange () {
		//TODO
	}

	public void EngineStateChange () {
		Debug.Log ("EngineStateChange()");
		engineList.Clear ();
		EngineManager[] foundEngines = gameObject.GetComponentsInChildren<EngineManager> ();
		foreach (EngineManager engine in foundEngines) {
			//Check if the engineList already contains the engine
			if (!engineList.ContainsKey(engine.GetInstanceID())){
				Debug.Log ("Not in the engine list");
				engineList.Add (engine.GetInstanceID (), engine);
				engineList [engine.GetInstanceID ()].SetShip (this);
			}
		}
	}

	public void PowerStateChange () {
		powerList.Clear ();
		PowerManager[] foundPowers = gameObject.GetComponentsInChildren<PowerManager> ();
		foreach (PowerManager power in foundPowers) {

			//Check if the engineList already contains the engine
			if (!powerList.ContainsKey(power.GetInstanceID())){
				Debug.Log ("Not in the power list");
				powerList.Add (power.GetInstanceID (), power);
				powerList [power.GetInstanceID ()].SetShip (this);
			}
		}
	}

	public void WeaponStateChange () {
		weaponList.Clear ();
		WeaponManager[] foundWeapons = gameObject.GetComponentsInChildren<WeaponManager> ();
		foreach (WeaponManager weapon in foundWeapons) {

			//Check if the engineList already contains the engine
			if (!weaponList.ContainsKey(weapon.GetInstanceID())){
				Debug.Log ("Not in the weapon list");
				weaponList.Add (weapon.GetInstanceID (), weapon);
				weaponList [weapon.GetInstanceID ()].SetShip (this);
			}
		}
	}

	//For Experimental purposes, final game should have selectable weapons
	public void FireWeapons () {
		var weapons = gameObject.GetComponentsInChildren<WeaponManager> ();
		foreach (WeaponManager wm in weapons) {
			wm.Fire ();
		}
	}

	public void CeaseFire () {
		var weapons = gameObject.GetComponentsInChildren<WeaponManager> ();
		foreach (WeaponManager wm in weapons) {
			wm.Stop ();
		}
	}
}
