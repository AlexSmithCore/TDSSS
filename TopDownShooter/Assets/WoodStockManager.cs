using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodStockManager : MonoBehaviour {

	public int logCount = 0;
	private int maxLog = 6;

	Transform logsParent;

	public List<Transform> logs = new List<Transform>();

	void Start(){
		logsParent = transform.Find("LogsParent");

		for(int l = 0; l < maxLog; l++){
			logs.Add(logsParent.GetChild(l));
		}

		UpdateLogs();
	}

	public void UpdateLogs(){
		for(int i = 0; i < maxLog; i++){
			if(i < logCount){
				logs[i].gameObject.SetActive(true);
			} else {
				logs[i].gameObject.SetActive(false);
			}
		}
	}
}