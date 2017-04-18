using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	public static UIManager ui;
	GameObject displayText;

	void Awake (){
		if (ui == null) {
			DontDestroyOnLoad (gameObject);
			ui = this;
		} else if (ui != this) {
			Destroy (gameObject);
		}

	}

	void Start (){
		foreach (Transform child in transform){
			if (child.name == "DisplayScreen") {
				displayText = child.Find ("DisplayText").gameObject;
			}
		}
	}

	public void UpdateText(string text) {
		displayText.GetComponent<Text> ().text = text;
	}

	public string GetText () {
		return displayText.GetComponent<Text> ().text.ToString();
	}
}