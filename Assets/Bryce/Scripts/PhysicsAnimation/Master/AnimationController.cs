using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

	public static Animator masterAnimator;

	// Use this for initialization
	void Start () {
		masterAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//if (Input.GetMouseButtonDown(0)) {
		//	masterAnimator.SetTrigger("isAttacking");
		//}

		if (Input.GetKey("w") || Input.GetKey("s")) {
			masterAnimator.SetBool("isWalking", true);
		}

		if (Input.GetKeyUp("w") || Input.GetKey("s")) {
			masterAnimator.SetBool("isWalking", false);
		}

		if (Input.GetKey(KeyCode.LeftShift)) {
			masterAnimator.SetBool("isSprinting", true);
		}

		if (Input.GetKeyUp(KeyCode.LeftShift)) {
			masterAnimator.SetBool("isSprinting", false);
		}
	}
}
