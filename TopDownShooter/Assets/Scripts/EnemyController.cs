using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

	public bool isDetected = false;
	public bool isDead;

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

	[Header("Field Of View")]
	[Space]
	public float viewRadius;
	[Range(0,360)]
	public float viewAngle;
	public LayerMask targetMask;
	public LayerMask obstacleMask;

	[HideInInspector]
	public List<Transform> visibleTargets = new List<Transform>();

	public Transform curTarget;

	public Transform enemyTarget;

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
		StartCoroutine ("FindTargetsWithDelay", .2f);
	}
	
	void Update () {
		if(isDetected){
			curTarget = enemyTarget;
			RotateTowards(curTarget);
		}

		if (em.health <= 0){
			isDead = true;
			em.Invoke("Death", 2.1f);
			enemy.isStopped = true;
			coll.enabled = false;
			animator.Play("Death", 0);
		} else {
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

		//enemy.SetDestination(curTarget.transform.position);
	}

	/*public Transform VToTransform(Vector3 point){
				//tPoint.transform.position = point;
		//return Transform.TransformVector(point);
	}*/

	IEnumerator FindTargetsWithDelay(float delay) {
		while (true) {
			yield return new WaitForSeconds (delay);
			FindVisibleTargets ();
		}
	}

	public void FindVisibleTargets() {
		visibleTargets.Clear ();
		Collider[] targetsInViewRadius = Physics.OverlapSphere (transform.position, viewRadius, targetMask);
		int count = 0;
		for (int i = 0; i < targetsInViewRadius.Length; i++) {
			Transform target = targetsInViewRadius [i].transform;
			Vector3 dirToTarget = (target.position - transform.position).normalized;
			if (Vector3.Angle (transform.forward, dirToTarget) < viewAngle / 2) {
				float dstToTarget = Vector3.Distance (transform.position, target.position);
				if (!Physics.Raycast (transform.position, dirToTarget, dstToTarget, obstacleMask)) {
					count++;
					visibleTargets.Add (target);
					enemyTarget = GetClosestGO(transform.position,targetsInViewRadius);
					isDetected = true;
				}
			}
		}

		if(count == 0){
			isDetected = false;
		}
	}

	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
		if (!angleIsGlobal) {
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}

	Transform GetClosestGO(Vector3 _pos, Collider[] _targets) {
 	int m = 0;
 		for (int i = 1; i < _targets.Length; i++){
  			if (((_pos - _targets[i].transform.position).magnitude < (_pos-_targets[m].transform.position).magnitude)){
  				m = i;
			}
		}
	 	return _targets[m].transform;
	}

	private void RotateTowards (Transform target) {
		if(enemyTarget != null){
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 12f);
		}
    }
}
