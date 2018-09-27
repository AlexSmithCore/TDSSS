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

	public int invSpace = 20;

	public float weight;
	public float maxWeight;

	public List<Item> items = new List<Item>();

	public Transform itemInfoPanel;

	public Transform itemInfoParent;

	void Start(){
		if(onItemChangedCallBack != null)
			onItemChangedCallBack.Invoke();
	}

	public bool Add(Item item){
		if(items.Count >= invSpace){
			Debug.Log("Not enoght space!");
			return false;
		}
		items.Add(item);
		weight += item.weight;

		ItemInfoPanel(item);
		if(onItemChangedCallBack != null)
			onItemChangedCallBack.Invoke();
		return true;
	}

	public void Remove(Item item){
		weight -= item.weight;
		items.Remove(item);

		if(onItemChangedCallBack != null)
			onItemChangedCallBack.Invoke();
	}

	public void ItemInfoPanel(Item item){
		Transform infoPanel = Instantiate(itemInfoPanel, transform.position, Quaternion.identity);
		infoPanel.GetChild(1).GetComponent<Image>().sprite = item.icon;
		infoPanel.GetChild(2).GetComponent<Text>().text = item.name;
		infoPanel.SetParent(itemInfoParent);	
	}
}
