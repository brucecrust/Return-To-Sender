using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DismembermentHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "Weapon") {
			print (transform.name);
			if (GetComponent<ConfigurableJoint>()) {
				NPCController.armatureDictionary.Remove(GetComponent<ConfigurableJoint>());
				Destroy(GetComponent<ConfigurableJoint>());
				NPCController.removedBodyParts += 1;
			}
		}
	}
}
