using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

	private Animator masterAnimator;

	// Use this for initialization
	void Start () {
		masterAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetMouseButtonDown(0)) {
			masterAnimator.SetTrigger("isAttacking");
		}

		if (Input.GetKey("w")) {
			 masterAnimator.SetBool("isWalking", true);
		}

		if (Input.GetKeyUp("w")) {
			masterAnimator.SetBool("isWalking", false);
		}
	}
}
