using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyWeapon : FireControl {



	#region implemented abstract members of FireControl

	public override void FireCommandGiven ()
	{
		 
	}

	public override void CeaseFireCommandGiven ()
	{
		throw new System.NotImplementedException ();
	}

	public override void Setup ()
	{
		type = "Energy";
	}
	#endregion

}
