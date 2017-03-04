using UnityEngine;
using System.Collections;

public class ConnecterConnector : MonoBehaviour {
	private GameObject parent;
	private GameObject connected;
	public GameObject Connected { get { return connected; } set { connected = value; } }

	void Start (){
		parent = gameObject.transform.parent.gameObject;
	}

	public void Connect (GameObject connectee){
		Debug.Log ("Acceptable Connector");
		parent.gameObject.AddComponent <FixedJoint2D> ();
		parent.GetComponent <FixedJoint2D> ().connectedBody = connectee.transform.parent.GetComponent<Rigidbody2D> ();
	}
}
