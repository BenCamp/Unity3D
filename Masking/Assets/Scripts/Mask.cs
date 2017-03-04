using UnityEngine;
using System.Collections;

public class Mask : MonoBehaviour
{
	void Start ()
	{
		iTween.MoveBy(gameObject, iTween.Hash("x", 116, "time", 2, "looptype", "loop", "easetype", iTween.EaseType.easeInOutCubic));
	}
}

