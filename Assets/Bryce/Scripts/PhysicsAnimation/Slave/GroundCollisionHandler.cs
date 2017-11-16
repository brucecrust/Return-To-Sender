using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollisionHandler : MonoBehaviour {
	public bool onGround;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter (Collision other) { 
		if (other.gameObject.tag == "GroundType" || other.gameObject.GetComponent<Terrain>()) {
			onGround = true;
		}
	}

	void OnCollisionStay(Collision other) {
		if (other.gameObject.tag == "GroundType" || other.gameObject.GetComponent<Terrain>()) {
			onGround = true;
		}
	}
	
	void OnCollisionExit (Collision other) {
		onGround = false;
	}

	void OnTriggerEnter (Collider other) { 
		if (other.gameObject.tag == "GroundType" || other.gameObject.GetComponent<Terrain>()) {
			onGround = true;
		} 
	}

	void OnTriggerStay(Collider other) {
		if (other.gameObject.tag == "GroundType" || other.gameObject.GetComponent<Terrain>()) {
			onGround = true;
		}
	}
	
	void OnTriggerExit (Collider other) {
		onGround = false;
	}
}
