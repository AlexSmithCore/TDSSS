using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour {

	public Transform playerTarget;

	public float timeToWait;

	public GameObject zombie;

	public Transform[] spawnPoints;

	public Gradient randomColor;

	void Start(){
		spawnPoints = new Transform[transform.childCount];
		for(int i = 0; i < this.transform.childCount; i++){
			spawnPoints[i] = transform.GetChild(i);
		}

		StartCoroutine(WaitToSpawn());
	}

	IEnumerator WaitToSpawn(){
		yield return new WaitForSeconds(timeToWait);
		SpawnEnemy();
	}

	void SpawnEnemy(){
		int rand = Random.Range(0,transform.childCount);
		GameObject enemy = Instantiate(zombie, new Vector3(spawnPoints[rand].position.x, 0f, spawnPoints[rand].position.z), Quaternion.identity);
		StartCoroutine(WaitToSpawn());
	}
}
