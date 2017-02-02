using UnityEngine;
using System.Collections;

public class MarkColor : MonoBehaviour {

	public MeshRenderer[] renderers;

	// Use this for initialization
	void Start () {
		var color = GetComponent<Player> ().info.AccentColor;
		foreach (var r in renderers) {
			r.material.color = color;
		}
	}
}
