using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodSystem : MonoBehaviour {

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

	void Start(){
		bloodCount = maxBloodCount;
	}

	void Update(){
		/*if(Input.GetKeyDown(KeyCode.B)){
			StopAllCoroutines();
			bleedingCount++;
			StartCoroutine(BloodyRiver());
		}

		if(Input.GetKeyDown(KeyCode.V)){
			bleedingCount--;
		}*/

		if(isBleeding){
			bloodCount -= (multiplier * bleedingCount) * Time.deltaTime;
			if(!timerStart){
				StartCoroutine(BloodyRiver());
			}
		} else { 
			timerStart = false;
			StopAllCoroutines();
		}

		if(bleedingCount < 0 ){
			bleedingCount = 0;
		}

		if(bleedingCount > 0){
			isBleeding = true;
		} else {
			isBleeding = false;
		}

		bleedingTime = (10 - bleedingCount) / 2;

		if(bleedingCount > 0){
			bleedingCountText.enabled = true;
			bleedingCountText.text = "X " + bleedingCount;
		} else { 
			bleedingCountText.enabled = false;
		}
		
		float test = bleedingCount / 10f;

		bleedingIndicator.color = bleedingGardient.Evaluate(test);

		heart.fillAmount = bloodCount / maxBloodCount;
		bloodText.text = (int)bloodCount + " ml";
	}


	public IEnumerator BloodyRiver(){
		if(isBleeding){
			timerStart = true;
			GameObject puddle = Instantiate(bloodPuddle, new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f),0.01f,transform.position.z + Random.Range(-0.5f, 0.5f)), transform.rotation);
			int rand = Random.Range(0, bloodTextures.Length);
			puddle.GetComponent<Renderer>().material.SetTexture("_MainTex", bloodTextures[rand]);
			yield return new WaitForSeconds(Random.Range(0f,TimerSet()));
			StartCoroutine(BloodyRiver());
		}
	}

	float TimerSet(){
		switch(bleedingCount)
		{
			case 1:
				return 3f;
				break;
			case 2: 
				return 2.5f;
				break;
			case 3:
				return 2f;
				break;
			case 4:
				return 1f;
				break;
			default:
				return 0.5f;
				break;
		}
	}
}
