using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

	public bool isDetected = false;
	public bool isDead;

	public bool isStun;

	public bool isAttacking;

	public float distToAttack;

	public float stunTime;
	public float stunCounter;

	public float enemySpeed;

	public float attackInterval;
	private float attackCounter;

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

	public GameObject hitPoint;
	RaycastHit hit;

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
		StartCoroutine ("FindTargetsWithDelay", .5f);
	}
	
	void FixedUpdate () {
		if(isStun){
			stunCounter -= Time.deltaTime;
			if(stunCounter <= 0){
				isStun = false;
				enemySpeed = speed;
				animator.SetFloat("isStun", 0f);
			}
		}

		if(isDetected && !isDead){
			curTarget = enemyTarget;
			RotateTowards(curTarget);
			enemy.SetDestination(curTarget.transform.position);
			float dist = (transform.position - enemyTarget.transform.position).magnitude;
			if(dist <= distToAttack && !isStun){
				if(!isAttacking){
					animator.Play("Attack");
					attackCounter = attackInterval;
				}
				isAttacking = true;
			}
		}

		if(isAttacking && !isStun){
			attackCounter -= Time.deltaTime;
			if(attackCounter <= 0){
				animator.Play("Attack");
				attackCounter = attackInterval;
				isAttacking = false;
			}
		} else {
			isAttacking = false;
		}

		enemy.isStopped = isAttacking;

		if (em.health <= 0){
			isDead = true;
			em.Invoke("Death", 2.1f);
			enemy.isStopped = true;
			coll.enabled = false;
			animator.Play("Death", 0);
		} else {
			if(!isAttacking){
				animator.Play("Base", 0);
				if (Mathf.Sin(2 * Mathf.PI * freq * Time.time + point) > 0.999f && !isDetected){
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

		enemy.speed = enemySpeed;
	}

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

	void AttackEnemy(){
  		Vector3 fwd = hitPoint.transform.TransformDirection(Vector3.forward);
  		if(Physics.Raycast(hitPoint.transform.position, fwd, out hit, 1.25f,targetMask))
  		{
			hit.collider.GetComponent<BloodSystem>().bloodCount -= 1000f;
			if (hit.collider.tag != "Player"){
				hit.collider.GetComponent<HumanController>().HitReaction(0.5f);
				//hit.collider.GetComponent<HumanController>().Invoke("HitReaction(0)", 1f);
			}
			float rand = Random.Range(0f,100f);
			if(rand >= 50f){
				hit.collider.GetComponent<BloodSystem>().bleedingCount++;
			}
			hit.collider.GetComponent<BloodSystem>().BloodFloor(2);
  		}
	}
}
