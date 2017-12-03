using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFire : MonoBehaviour {
	public GameObject particle;
	private bool onFire;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (onFire) {
			particle.GetComponent<ParticleSystem>().startLifetime -= 0.0005f;
			if (particle.GetComponent<ParticleSystem>().startLifetime <= 0f) {
				onFire = false;
				particle.SetActive(false);
			} 
		}
	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "OnFire") {
			particle.SetActive(true);
			onFire = true;
		}
	}
}
