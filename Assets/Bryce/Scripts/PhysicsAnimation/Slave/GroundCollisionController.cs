using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollisionController : MonoBehaviour {
	public static bool onGround;
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Ground") {
			onGround = true;
		}
	}
    
	void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Ground")
            onGround = false;
	}
}
