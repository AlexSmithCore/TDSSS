using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour {

	public Animator anim;
	public float moveSpeed;

	public float distanceToTarget;

	public bool isStun;

	public bool canAttack;
	public bool isAttacking;

	public int damage;

	private float dist;

	PlayerController target;

	public Rigidbody rb;

	public NavMeshAgent zombie;

	ZombieManager zm;

	public Collider zCollider;

	void Start(){
		zombie.speed = moveSpeed;
		canAttack = true;

		anim = GetComponent<Animator>();

		rb = GetComponent<Rigidbody>();
		target = FindObjectOfType<PlayerController>();
		zm = GetComponent<ZombieManager>();
	}

	void FixedUpdate(){
		if(!zm.isDead){
		anim.SetBool("attack", isAttacking);

		if(target != null){
			transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
			if(!isStun){
				zombie.speed = moveSpeed;
				dist = Vector3.Distance(target.transform.position, transform.position);
				if(dist >= distanceToTarget){
					zombie.SetDestination(target.transform.position);
				} else {
					zombie.speed = 0;
				}
				if(dist <= distanceToTarget && !isAttacking && canAttack && !isStun){
					canAttack = false;
					StartCoroutine(Attack());
				}else if(canAttack && isAttacking){
					isAttacking = false;
				}
			} else {
				isAttacking = false;
				canAttack = false;
				zombie.speed = 0;
			}
			}
		} else {
			zombie.speed = 0;
			zCollider.enabled = false;
		}
	}

	IEnumerator Attack(){
		isAttacking = true;
		yield return new WaitForSeconds(1f);
		canAttack = true;
	}

	IEnumerator WaitToNextAttack(){
		yield return new WaitForSeconds(1f);
		canAttack = true;
	}

	public IEnumerator WaitToStun(){
		isStun = true;
		anim.SetTrigger("stun");
		yield return new WaitForSeconds(1f);
		isStun = false;
		canAttack = true;
	}
}
