﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveableObject {
	
	public bool canBeParent;

	public List<MyComponent> components;
}