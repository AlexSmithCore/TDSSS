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

	public List<Slot> weaponItems = new List<Slot>();

	public Transform itemInfoPanel;

	public Transform itemInfoParent;

	public int selectedSlot = 0;

	public bool isRightClick;

	void Start(){
		if(onItemChangedCallBack != null)
			onItemChangedCallBack.Invoke();
	}

	public bool Add(Item item, int iCount){
		if(items.Count >= invSpace && (weight + item.weight) * iCount < maxWeight){
			Debug.Log("Not enoght space!");
			return false;
		}

		CheckRepeat(item,iCount);

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

	public void RemoveItem(Item item){
		for(int i = items.Count - 1; i >= 0; i--){
			Debug.Log("test!");
			if(items[i].item == item){
				weight -= items[i].item.weight;
				if(items[i].count > 1){
					items[i].count--;
				} else {
					items.Remove(items[i]);
				}
				if(onItemChangedCallBack != null)
					onItemChangedCallBack.Invoke();
				return;
			}
		}
	}

	public void RemoveAllItem(int id){
		weight -= items[id].item.weight * items[id].count;
		items.Remove(items[id]);
	}

	public void RemoveAll(){
		for(int i = 0; i < items[selectedSlot].count; i++){
			weight -= items[selectedSlot].item.weight;
		}
		GameObject drop = Instantiate(items[selectedSlot].item.prefab,owner.transform.position + transform.forward * 4, Quaternion.identity);
		drop.GetComponent<ItemInfo>().count = items[selectedSlot].count;
		items.Remove(items[selectedSlot]);
		isRightClick = false;

		if(onItemChangedCallBack != null)
			onItemChangedCallBack.Invoke();
	}

	public void RemoveWeapon(int id){
		if(weaponItems.Count != 0){
			if(id < weaponItems.Count){
				Debug.Log("Removed weapon!");
				weaponItems[id].item = null;
			}
		}
	}

	public void Arm(){
		isRightClick = false;

/*		if(weaponItems.Count >= fastItemsSpace){
			Debug.Log("Not enoght space!");
			if(onItemChangedCallBack != null)
				onItemChangedCallBack.Invoke();
			return;
		}*/

		if((int)items[selectedSlot].item.type == 3){
			//bool isHaveItem = CheckWeaponItems();
			//if(!isHaveItem){
				weaponItems[0].item = items[selectedSlot].item;
			//}
		}

		if((int)items[selectedSlot].item.type == 4){
			//bool isHaveItem = CheckWeaponItems();
			weaponItems[1].item = items[selectedSlot].item;
		}

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

		bool isHaveItem = CheckFastItems();
		if(!isHaveItem){
			fastItems.Add(new Slot(items[selectedSlot].item,1));
		}

		if(onItemChangedCallBack != null)
			onItemChangedCallBack.Invoke();
	}

	private bool CheckWeaponItems(){
		for(int i = 0; i < weaponItems.Count; i++){
			if(weaponItems.Count > 0){
				return true;
			}
		}
		return false;
	}

	private bool CheckFastItems(){
		for(int i = 0; i < fastItems.Count; i++){
			if(fastItems[i].item == items[selectedSlot].item){
				return true;
			}
		}
		return false;
	}

	public void RemoveFastSlot(int id){
		if(fastItems.Count != 0){
			if(id < fastItems.Count){
				Debug.Log("Remove " + id + " slot!");
				fastItems.Remove(fastItems[id]);
			}
		}
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
		if(items.Count == 0){
			Debug.Log("Stacking!");
			Stacking(iCount ,item);	
			return;
		}
		
		bool isNeedItem = CheckInv(item);
		if(isNeedItem){
			int c = iCount; // 12
			for(int i = 0; i < items.Count; i++){
				if(items[i].item == item && items[i].count < items[i].item.stackSize){
					if(c <= item.stackSize - items[i].count){
						items[i].count += c;
						weight += item.weight * c;
						return;
					} else {
						c-= item.stackSize - items[i].count;
						weight += (item.stackSize - items[i].count) * item.weight;
						items[i].count = item.stackSize;
					}
					if(c <= 0){
						return;
					}
				}

				if(i >= items.Count - 1){
					if(c > 0){
						Debug.Log("Stacking!");
						Stacking(c ,item);	
						return;
					}
				}
			}
		} else {
			Debug.Log("Stacking!");
			Stacking(iCount ,item);	
			return;
		}
	}

	private bool CheckInv(Item item){
		for(int i = 0; i < items.Count; i++){
			if(items[i].item == item && items[i].count < items[i].item.stackSize){
				return true;
			}
		}
		return false;
	}

	private void Stacking(int iCount ,Item item){
	int count = iCount;
		for( int i = 0; i < count / item.stackSize; i++){
			items.Add( new Slot( item, item.stackSize));
			weight+=item.weight * item.stackSize;
		}

		if(count % item.stackSize != 0){
			items.Add(new Slot(item, count % item.stackSize));
			weight+= item.weight * (count % item.stackSize);
		}
	}
}
