using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {

	public Transform itemsParent;

	Inventory inventory;

	InventorySlot[] slots;

	public Transform weight;

	void Start(){
		inventory = Inventory.instance;
		inventory.onItemChangedCallBack += UpdateUI;

		slots = itemsParent.GetComponentsInChildren<InventorySlot>();
	}

	void UpdateUI(){
		weight.GetChild(0).GetComponent<Text>().text = inventory.weight + " kg";
		weight.GetChild(1).GetComponent<Text>().text = inventory.maxWeight + " kg";
		for (int i = 0; i < slots.Length; i++)
		{
			if(i < inventory.items.Count)
			{
				slots[i].AddItem(inventory.items[i]);
			} else {
				slots[i].ClearItem();
			}
		}
	}
}
