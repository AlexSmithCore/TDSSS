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
	private EnemyManager em;

	private Collider coll;


	void Start () {
		animator = GetComponent<Animator>();
		enemy = GetComponent<NavMeshAgent>();
		em = GetComponent<EnemyManager>();
		coll = GetComponent<Collider>();

		NavMeshTriangulation triangulatedNavMesh = NavMesh.CalculateTriangulation();
        vertices = triangulatedNavMesh.vertices;
		point = Random.Range(0, vertices.Length);
		enemy.SetDestination(vertices[point]);

		freq = Random.Range(0.01f, freq);
	}
	
	void Update () {
	
		if (em.health <= 0){
			em.Invoke("Death", 2.1f);
			enemy.isStopped = true;
			coll.enabled = false;
			animator.Play("Death", 0);
		}
		else{
			animator.Play("Base", 0);
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
}
