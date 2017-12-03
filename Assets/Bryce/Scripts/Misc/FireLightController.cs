using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLightController : MonoBehaviour {
	private Light light;
	public float lightTimer, lightChangeConstant, lightIntensityRange;
	private float lightCounter;
	private float lightTransition;
	private bool lightUp;

	// Use this for initialization
	void Start () {
		light = transform.GetComponent<Light>();	
		lightCounter = lightTimer;
		lightTransition = lightTimer;
	}
	
	// Update is called once per frame
	void Update () {
		if (lightCounter > 0) {
			lightCounter -= Time.deltaTime;
			if (lightUp) {
				if (lightTransition > 0) {
					light.intensity += lightChangeConstant;
					lightTransition -= Time.deltaTime;
				} else {
					lightTransition = lightTimer;
					lightUp = false;
				}
			} else {
				if (lightTransition > 0) {
					light.intensity -= lightChangeConstant;
					lightTransition -= Time.deltaTime;
				} else {
					lightTransition = lightTimer;
					lightUp = true;
				}
			}
		} else {
			lightCounter = lightTimer + Random.Range(-lightIntensityRange, lightIntensityRange);
		}
	}
}
