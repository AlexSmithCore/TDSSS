using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

	#region  Singleton
	public static Inventory instance;

	void Awake(){
		instance = this;
	}

	#endregion

	public delegate void OnItemChanged();
	public OnItemChanged onItemChangedCallBack;

	public Transform owner;

	public int invSpace = 20;

	private int fastItemsSpace = 3;

	public float weight;
	public float maxWeight;
	public List<Slot> items = new List<Slot>();

	public List<Slot> fastItems = new List<Slot>();

	public Transform itemInfoPanel;

	public Transform itemInfoParent;

	public int selectedSlot = 0;

	public bool isRightClick;

	public bool Add(Item item, int iCount){
		if(items.Count >= invSpace){
			Debug.Log("Not enoght space!");
			return false;
		}
		if(items.Count == 0){
			items.Add(new Slot(item, 0));
		} else {
			CheckRepeat(item,iCount);
		}

		ItemInfoPanel(item, iCount);

		if(onItemChangedCallBack != null)
			onItemChangedCallBack.Invoke();
		return true;
	}

	public void Remove(){
		weight -= items[selectedSlot].item.weight;
		GameObject drop = Instantiate(items[selectedSlot].item.prefab,owner.transform.position + transform.forward * 4, Quaternion.identity);
		drop.GetComponent<ItemInfo>().count = 1;
		if(items[selectedSlot].count > 1){
			items[selectedSlot].count--;
		} else {
			items.Remove(items[selectedSlot]);
		}
		isRightClick = false;

		if(onItemChangedCallBack != null)
			onItemChangedCallBack.Invoke();
	}

	public void Wear(){
		isRightClick = false;

		if(fastItems.Count >= fastItemsSpace){
			Debug.Log("Not enoght space!");
			if(onItemChangedCallBack != null)
				onItemChangedCallBack.Invoke();
			return;
		}

		if(items[selectedSlot].count > 1){
			items[selectedSlot].count--;
		} else {
			items.Remove(items[selectedSlot]);
		}

		fastItems.Add(new Slot(items[selectedSlot].item,1));

		if(onItemChangedCallBack != null)
			onItemChangedCallBack.Invoke();
	}

	public void ItemInfoPanel(Item item, int count){
		Transform infoPanel = Instantiate(itemInfoPanel, transform.position, Quaternion.identity);
		infoPanel.GetChild(0).GetComponent<Image>().color = item.itemColor;
		infoPanel.GetChild(1).GetComponent<Image>().sprite = item.icon;
		infoPanel.GetChild(2).GetComponent<Text>().text = item.name;
		infoPanel.SetParent(itemInfoParent);
		infoPanel.localScale = new Vector3(1f, 1f, 1f);	
		if(count > 1){
			infoPanel.GetChild(4).gameObject.SetActive(true);
			infoPanel.GetChild(4).GetChild(0).GetComponent<Text>().text = "x " + count;
		}
	}

	public void CheckRepeat(Item item, int iCount){
		int count = iCount;
		Debug.Log(count);
		for(int i = 0; i < items.Count; i++){
			//items.Add(new Slot(item, 1));
			if(items[i] != null && items[i].item == item && items[i].count < item.stackSize){
				for(int c = count; c > 0; c--){
					weight += item.weight;
					items[i].count ++;
					if(items[i].count >= item.stackSize){
						print("Pasha privet!");
						count = c;
						//count--;
						items.Add(new Slot(item, 0));
						break;
					} 
				}
			}
		}
	}
}
