using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

	[SerializeField]	
	private int point;
	[SerializeField]
	private float freq = 0.2f;
	public float speed;
	private NavMeshAgent enemy;
	private Animator animator;
	private Vector3[] vertices;


	void Start () {
		animator = GetComponent<Animator>();
		enemy = GetComponent<NavMeshAgent>();

		NavMeshTriangulation triangulatedNavMesh = NavMesh.CalculateTriangulation();
        vertices = triangulatedNavMesh.vertices;
		point = Random.Range(0, vertices.Length);
		enemy.SetDestination(vertices[point]);

		freq = Random.Range(0.01f, freq);
	}
	
	void Update () {
		if (Mathf.Sin(2 * Mathf.PI * freq * Time.time + point) > 0.999f){
			point = Random.Range(0, vertices.Length);
			enemy.SetDestination(vertices[point]);
		}
		if (enemy.velocity.magnitude >= speed || enemy.velocity.magnitude < 0){
			animator.SetFloat("Move", 1f);
		}
		else{
			animator.SetFloat("Move", enemy.velocity.magnitude/speed);
		}
	}
}
