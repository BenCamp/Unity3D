using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextTest : MonoBehaviour {

	public string message;
	private Text textInstance;

	// Use this for initialization
	void Start () {
		textInstance = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		textInstance.text = message;
	}
}
