using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockManager : MonoBehaviour {

	public enum StockType{
		general = 0,
		wood = 1,
	}

	public StockType stockType;

	public int resourcesCount;

	public void AddToStock(Transform worker, int count){
		Debug.Log("Items added to stock!");
		resourcesCount+=count;
		worker.GetComponent<HumanManager>().DeleteItems(1, count);
	}
}
