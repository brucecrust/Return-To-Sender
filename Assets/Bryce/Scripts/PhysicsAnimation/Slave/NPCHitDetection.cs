using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHitDetection : MonoBehaviour {
	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "Weapon") {
			if (gameObject.name.ToLower().Contains("cog")) {
				GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
			}
			GetComponent<Rigidbody>().AddForce(-Vector3.right * 100f);
			NPCController.removedBodyParts += 1;
			NPCController.armatureDictionary.Remove(GetComponent<ConfigurableJoint>());
			Destroy(GetComponent<ConfigurableJoint>());		
		}
	}
}
