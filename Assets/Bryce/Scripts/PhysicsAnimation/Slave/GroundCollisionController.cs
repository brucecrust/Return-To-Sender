using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollisionController : MonoBehaviour {
	public static bool onGround;
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Ground" || other.gameObject.transform.parent.tag == "Ground") {
			onGround = true;
		}
		RagdollController.isFalling = false;
		RagdollController.standupCounter = RagdollController.standupTimer;
	}

	void OnTriggerStay(Collider other) {
		if (other.gameObject.tag == "Ground" || other.gameObject.transform.parent.tag == "Ground") {
			onGround = true;
		}
		RagdollController.isFalling = false;
	}
    
	void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Ground" || other.gameObject.transform.parent.tag == "Ground") {
            onGround = false;
		}
		RagdollController.isFalling = true;
	}
}
