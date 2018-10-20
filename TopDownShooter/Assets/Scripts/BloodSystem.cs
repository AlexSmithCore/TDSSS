using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodSystem : MonoBehaviour {

	public bool isPlayer;
	public float bloodCount;

	public int maxBloodCount;

	public float multiplier;

	public bool isBleeding;

	public bool timerStart;

	public int bleedingCount;

	public float bleedingTime;

	public GameObject bloodPuddle;

	public Image heart;

	public Text bloodText;

	public Image bleedingIndicator;
	public Text bleedingCountText;

	public Gradient bleedingGardient;

	public Texture[] bloodTextures;

	HumanController hc;

	public Collider col;

	void Start(){
		bloodCount = maxBloodCount;
		col = GetComponent<Collider>();
		if(!isPlayer){
			hc = GetComponent<HumanController>();
		}
	}

	void LateUpdate(){
		if(isBleeding){
			bloodCount -= (multiplier * bleedingCount) * Time.deltaTime;
			if(!timerStart){
				StartCoroutine(BloodyRiver());
			}
		} else { 
			timerStart = false;
			StopAllCoroutines();
		}

		if(bleedingCount > 0){
			isBleeding = true;
			if(isPlayer){
				bleedingCountText.enabled = true;
				bleedingCountText.text = "X " + bleedingCount;
			}
		} else {
			if(isPlayer){
				bleedingCountText.enabled = false;
			}
			isBleeding = false;
			bleedingCount = 0;
		}

		bleedingTime = (10 - bleedingCount) / 2;
		
		float test = bleedingCount / 10f;

		if(isPlayer){
			bleedingIndicator.color = bleedingGardient.Evaluate(test);
			heart.fillAmount = bloodCount / maxBloodCount;
			bloodText.text = (int)bloodCount + " ml";
		} else {
			if (bloodCount <= 0){
				BloodFloor(4);
				hc.isDead = true;
				Invoke("Death", 4f);
				hc.human.isStopped = true;
				col.enabled = false;
				hc.animator.Play("Death", 0);
			}
		}
	}


	public IEnumerator BloodyRiver(){
		if(isBleeding){
			timerStart = true;
			GameObject puddle = Instantiate(bloodPuddle, new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f),0.01f,transform.position.z + Random.Range(-0.5f, 0.5f)), transform.rotation);
			int rand = Random.Range(0, bloodTextures.Length);
			puddle.GetComponent<Renderer>().material.SetTexture("_MainTex", bloodTextures[rand]);
			yield return new WaitForSeconds(Random.Range(0f,1.5f));
			StartCoroutine(BloodyRiver());
		}
	}

	public void BloodFloor(int count){
		for(int i = 0; i < count; i++){
			GameObject puddle = Instantiate(bloodPuddle, new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f),0.01f,transform.position.z + Random.Range(-0.5f, 0.5f)), transform.rotation);
			int rand = Random.Range(0, bloodTextures.Length);
			puddle.GetComponent<Renderer>().material.SetTexture("_MainTex", bloodTextures[rand]);
		}
	}

	public void Death(){
		Destroy(this.gameObject);
	}
}
