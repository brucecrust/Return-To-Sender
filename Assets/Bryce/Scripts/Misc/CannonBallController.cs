using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallController : MonoBehaviour {
	public float cannonBallForce;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision other) {
		GetComponent<Rigidbody>().AddForce(transform.forward * cannonBallForce, ForceMode.Impulse);
	}
}
