using UnityEngine;
using System.Collections;

public class DefenseManager : MonoBehaviour {
	public float maxHitPoints = 1;
	public float MaxHitPoints { get { return maxHitPoints; }  set { maxHitPoints = value; } }

	private float currentHitPoints;
	public float CurrentHitPoints { get { return currentHitPoints; } set { currentHitPoints = value; } }

	public float maxArmor = 1;
	public float MaxArmor { get { return maxArmor; } set { maxArmor = value; } }

	private float currentArmor;
	public float CurrentArmor { get { return currentArmor; } set { currentArmor = value; } }

	void Start (){
		currentHitPoints = maxHitPoints;
		currentArmor = maxArmor;
	}

	public void TakeDamage(float damage){
		float mitigation = CurrentArmor / maxArmor;
		currentArmor -= mitigation * damage;
		currentHitPoints -= (1 - mitigation) * damage;
		Debug.Log ("Hit Points = " + currentHitPoints + ", Armor = " + currentArmor);
		if (currentHitPoints <= 0) {
			Destroy (gameObject);
		}
	}
}
