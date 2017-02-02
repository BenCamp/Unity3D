using UnityEngine;
using System.Collections;

public class ButtonTest : MonoBehaviour {
	public void OnClick(){
		Debug.Log ("Clicked");
	}

	public void OnOver(){
		Debug.Log ("OnOver");
	}

	public void OnExit(){
		Debug.Log ("OnExit");
	}
}
