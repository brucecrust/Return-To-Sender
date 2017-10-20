using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathController : MonoBehaviour {
	public PlayerController playerController;
	public SpringUpOnCollision spring;
	public static bool isDead;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.GetComponent<Rigidbody>()) {
			if (other.gameObject.GetComponent<Rigidbody>().mass > 100f) {
				if (other.relativeVelocity.magnitude > 10f) {
					isDead = true;
					spring.rootBone.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
					Destroy(playerController);
					Destroy(spring);
				}
			}
		}
	}
}
