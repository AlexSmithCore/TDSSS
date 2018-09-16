using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemperaturesController : MonoBehaviour {

	public bool isHot;

	[Header("Main options")]
	[Space]
	public float maxTemperature;

	public float minTemperature;

	public float temperature;

	private float temperatureTransition;

	[SerializeField]
	public float maxTransition;

	[Header("UI Elements")]
	[Space]
	public Image coldHeart;
	public Text temperatureText;

	private float test;

	public Image warmIndicator;

	public Text transitionTimeText;

	public Gradient tempGardient;

	void Start(){
		temperature = 35;
	}

	void Update(){
		//float t = (temperature - minTemperature) / ( maxTemperature - minTemperature );
		float t = 1 - ((temperature - minTemperature) / ( maxTemperature - minTemperature ));
		coldHeart.color = new Color(coldHeart.color.r,coldHeart.color.g,coldHeart.color.b,t);

		temperatureText.text = temperature.ToString("F1") + " C";

		float tt = temperatureTransition + (test * Time.deltaTime);
		temperatureTransition = Mathf.Clamp(tt,-maxTransition,maxTransition); 

		if(temperatureTransition >= 1){
			isHot = true;
		} else if(temperatureTransition <= -1){
			isHot = false;
		}
		if(!isHot){
			if(temperature >= minTemperature){
				temperature -= 0.025f * Time.deltaTime;
			}
		}

		float ttc = (temperatureTransition + 3) / 6;
		warmIndicator.color = tempGardient.Evaluate(ttc);
		float ttt = Mathf.Abs((int)temperatureTransition);
		if(ttt == maxTransition || ttt == 0) { transitionTimeText.enabled = false; } else { transitionTimeText.enabled = true; }
		transitionTimeText.text = ttt + " sec";

	}	

	void OnTriggerStay(Collider other)
	{
		if(other.tag == "HotObject"){
			test = 1f;
			if(temperature < maxTemperature && isHot){
				temperature += 0.03f * Time.deltaTime;
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag == "HotObject"){
			test = -1;
		}
	}
}
