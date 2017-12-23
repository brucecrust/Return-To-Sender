using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGroundCollisionController : MonoBehaviour {
	public static bool onGround;
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Ground" || other.gameObject.transform.parent.tag == "Ground") {
			onGround = true;
		}
		NPCController.isFalling = false;
	}

	void OnTriggerStay(Collider other) {
		if (other.gameObject.tag == "Ground" || other.gameObject.transform.parent.tag == "Ground") {
			onGround = true;
		}
		NPCController.isFalling = false;
	}
    
	void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Ground" || other.gameObject.transform.parent.tag == "Ground") {
            onGround = false;
		}
		NPCController.isFalling = true;
	}
}
