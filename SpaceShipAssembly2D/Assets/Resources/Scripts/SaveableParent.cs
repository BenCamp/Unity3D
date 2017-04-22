using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveableParent : SaveableObject {

	public List <SaveableObject> manifest;

	SaveableParent () {
		canBeParent = true;
	}
}
