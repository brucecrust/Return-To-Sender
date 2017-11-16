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
		if (isDead) {
			Die();
		}
	}

	void OnCollisionEnter(Collision other) {
		if (!isDead) {
			if (other.gameObject.GetComponent<Rigidbody>()) {
				if (other.gameObject.GetComponent<Rigidbody>().mass > 100f) {
					if (other.relativeVelocity.magnitude > 100f) {
						isDead = true;
						print ("THIS KILLED YOU " + " | " + other.relativeVelocity.magnitude + " with a mass of " + other.gameObject.GetComponent<Rigidbody>().mass);
					}
				}
			}

			if (other.gameObject.tag == "Weapon") {
				isDead = true;
			}	
		}
	}

	public void Die() {
		spring.rootBone.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		Destroy(playerController);
		Destroy(spring);
	}
}
