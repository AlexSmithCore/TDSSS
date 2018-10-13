using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropSystem : MonoBehaviour {

	public int itemsDropCount;

	public int[] simple;

	public Item[] usualItems;

	public Item[] posibleItems;

	public void RandomDrop(){
		for(int u = 0; u < usualItems.Length; u++){
			//DropItem(usualItems[u], Random.Range(1, usualItems[u].maxDropCount));
		}

		for(int c = 0; c < itemsDropCount; c++){
			float result = 0;
			int greatest = 0;
			float rand = (float)Random.Range(11, 53);
			for(int i = 0; i < posibleItems.Length; i++){
				result = rand / simple[i]; 
				if (result - Mathf.Floor(result) == 0) 
				{ 
					greatest = i; 
				}
			}

			if(greatest > 0){
				Debug.Log("Выпало: " + posibleItems[greatest].name);
				//DropItem(posibleItems[greatest], Random.Range(1, posibleItems[greatest].maxDropCount));
			} else {
				Debug.Log("Ничего не выпало!");
			}
		}
	}

	private void DropItem(Item item, int count){
		GameObject drop = Instantiate(item.prefab,transform.position + new Vector3(Random.Range(0,2f),0,Random.Range(0,2f)), Quaternion.identity);
		drop.GetComponent<ItemInfo>().count = count;
	}
}
