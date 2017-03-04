using UnityEngine;
using System.Collections;

public class Gear : MonoBehaviour
{
	void Start ()
	{
		iTween.RotateBy(gameObject, iTween.Hash("y",1,"time",4,"looptype","loop","easetype","linear"));
	}
}

