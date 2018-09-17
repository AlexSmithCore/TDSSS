using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllAgentsManager : MonoBehaviour {

	public GameObject[] enemies;

	public GameObject[] allies;

	public int enemiesSize;

	public int alliesSize;

	void Awake(){
		enemies = new GameObject[enemiesSize];
		allies = new GameObject[alliesSize];
	}
}
