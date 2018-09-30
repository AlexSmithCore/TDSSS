using UnityEngine;
using System.Collections;

public class MonsterSpawner : MonoBehaviour {
	
	public Transform basePoint; 
    public GameObject enemy;
    public float dist;
    private float angle;
    public int count = 15;

    public float timeToSpawn;
	private float spawnCounter;

    void Start ()
    {
        angle = Mathf.PI * 2;
    }

    void Update(){
        spawnCounter -= Time.deltaTime;
        if(spawnCounter <= 0){
            spawnCounter = timeToSpawn;
            SpawnEnemy();
        }
    }
    void SpawnEnemy(){
		Vector3 point = basePoint.position;

        for(int i = 1; i <= count; i++)
        {
            //Рассчитываем координату Z для врага
            float _z = basePoint.transform.position.z + Mathf.Cos(angle/count*i)*dist;
            //Рассчитываем координату X для врага
            float _x = basePoint.transform.position.x + Mathf.Sin(angle/count*i)*dist;
            point.x = _x;
            point.z = _z;
            //Создаём врага
            Instantiate(enemy, point, Quaternion.identity);
        }
    }
}