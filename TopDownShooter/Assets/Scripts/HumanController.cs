using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

public class HumanController : MonoBehaviour {

	public bool isDetected = false;

	public Transform mainTarget;

	public Transform curTarget;

	private float distToFollow;

	private NavMeshAgent human;

	private Animator animator;
	public float Speed = 10f;

	[Header("Field Of View")]
	[Space]
	public float viewRadius;
	[Range(0,360)]
	public float viewAngle;

	public LayerMask targetMask;
	public LayerMask obstacleMask;

	[HideInInspector]
	public List<Transform> visibleTargets = new List<Transform>();


	void Start(){
		animator = GetComponent<Animator>();
		human = GetComponent<NavMeshAgent>();
		mainTarget = GameObject.FindGameObjectWithTag("Player").transform;
		curTarget = mainTarget;
		StartCoroutine ("FindTargetsWithDelay", .2f);
	}

	
	void FixedUpdate(){
		/*if(isDetected){
			distToEnemy = Vector3.Distance(transform.position, target.transform.position);
		} */

		if(!isDetected){
			human.SetDestination(mainTarget.transform.position);
		}

		transform.LookAt(curTarget.transform.position);

		// Animator
		if (human.velocity.magnitude >= Speed || human.velocity.magnitude < 0){
			animator.SetFloat("Move", 1f);
		}
		else{
			animator.SetFloat("Move", human.velocity.magnitude/Speed);
		}
	}

		IEnumerator FindTargetsWithDelay(float delay) {
		while (true) {
			yield return new WaitForSeconds (delay);
			FindVisibleTargets ();
		}
	}

	void FindVisibleTargets() {
		visibleTargets.Clear ();
		Collider[] targetsInViewRadius = Physics.OverlapSphere (transform.position, viewRadius, targetMask);
		for (int i = 0; i < targetsInViewRadius.Length; i++) {
			Transform target = targetsInViewRadius [i].transform;
			Vector3 dirToTarget = (target.position - transform.position).normalized;
			if (Vector3.Angle (transform.forward, dirToTarget) < viewAngle / 2) {
				float dstToTarget = Vector3.Distance (transform.position, target.position);

				if (!Physics.Raycast (transform.position, dirToTarget, dstToTarget, obstacleMask) && target.tag == "Enemy") {
					visibleTargets.Add (target);
					curTarget = GetClosestGO(transform.position,targetsInViewRadius);
				}
			}
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
  			if (((_pos - _targets[i].transform.position).magnitude < (_pos-_targets[m].transform.position).magnitude) && _targets[i].tag == "Enemy"){
  				m = i;
			}
		}
	 	return _targets[m].transform;
	}

}
