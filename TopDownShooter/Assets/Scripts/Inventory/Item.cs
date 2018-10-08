﻿using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {
	
	[Header("Item Settings")]
	new public string name = "New Item";

	public enum Type{
		other = 0,
		medical = 1,
		ammo = 2,
		weapon = 3,
	}

	public Type type;
	public Sprite icon = null; 

	public float weight;

	public int stackSize;

	public int timeToUse;

	public int maxDropCount;

	public Color itemColor;


	public GameObject prefab;

	[Space]
	[TextArea]
	public string description;

	[Space]
	public bool canWear;
	public bool canUse;

	Inventory inv;

	public virtual void Use(){
		Debug.Log("Using " + name);

		if(name == "Bandage"){
			Inventory.instance.RemoveItem(this);
			Inventory.instance.owner.GetComponent<BloodSystem>().bleedingCount--;
		}
	}
	
}
