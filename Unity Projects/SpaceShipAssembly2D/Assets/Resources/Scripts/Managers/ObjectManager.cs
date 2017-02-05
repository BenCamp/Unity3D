using UnityEngine;
using System.Collections;

abstract public class ObjectManager : MonoBehaviour {
	private bool isSelectable;
	public bool IsSelectable { get { return isSelectable; } set { isSelectable = value; } }
}
