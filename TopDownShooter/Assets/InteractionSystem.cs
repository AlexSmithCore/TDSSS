using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour {
	
	public bool interacted;

	public delegate void OnDropChanged();
	public OnDropChanged onDropChangedCallBack;

	public Transform interactionPoint;

	public GameObject materialsBody;

	[SerializeField]
	private Material _material;

	public Color _interactColor;
	public Color _disableColor;

	public List<Slot> dropItems = new List<Slot>();

	public Inventory Inventory;

	
	public int itemsSpace;

	public float weight;
	public float maxWeight;


	void Start(){
		_material = materialsBody.GetComponent<Renderer>().material;
		Inventory = Inventory.instance;
	}

	public bool Add(Item item, int iCount){
		if(dropItems.Count >= itemsSpace && (weight + item.weight) * iCount < maxWeight){
			Debug.Log("Not enoght space!");
			return false;
		}

		dropItems.Add(new Slot(item, iCount));

		if(onDropChangedCallBack != null)
			onDropChangedCallBack.Invoke();
		return true;
	}


	/*private void Stacking(int iCount ,Item item){
	int count = iCount;
		for( int i = 0; i < count / item.stackSize; i++){
			dropItems.Add( new Slot( item, item.stackSize));
			weight+=item.weight * item.stackSize;
		}

		if(count % item.stackSize != 0){
			dropItems.Add(new Slot(item, count % item.stackSize));
			weight+= item.weight * (count % item.stackSize);
		}
	}*/

	public void Interact(){
		if(interacted){
			_material.SetColor("_OutlineColor", _interactColor);
		} else { 
			_material.SetColor("_OutlineColor", _disableColor);
		}
	}
}
