using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISelected : MonoBehaviour {
	public void UpdateText(string text) {
		gameObject.GetComponent<Text> ().text = text;
	}

	public string GetText () {
		return gameObject.GetComponent<Text> ().text.ToString();
	}
}
