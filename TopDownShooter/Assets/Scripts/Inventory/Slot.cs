using System.Collections;
using UnityEngine;

[System.Serializable]
public class Slot {
	public Item item;
	public int count;

	public Slot(Item newItem, int newCount){
		item = newItem;
		count = newCount;
	}
}
