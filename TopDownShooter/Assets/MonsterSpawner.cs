using UnityEngine;
using System.Collections;

public class MonsterSpawner : MonoBehaviour {
	
	public Transform basePoint; 
    public GameObject enemy;
    public float dist;
    public float angle = 360;
    public int count = 15;

	public float timeToSpawn;

    void Start ()
    {
        angle = Mathf.PI * 2;
		StartCoroutine(WaitToSpawn());
    }

	IEnumerator WaitToSpawn(){
		yield return new WaitForSeconds(timeToSpawn);

		print("Spawned!");
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
		StartCoroutine(WaitToSpawn());
	}
}