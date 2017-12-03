using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

	public static Animator masterAnimator;
	public float attackTimer;
	private float attackCounter;
	private bool hasAttacked, hasStabbed, hasHacked, hasSlashed;


	// Use this for initialization
	void Start () {
		attackCounter = attackTimer;
		masterAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (hasAttacked) {
			attackCounter -= Time.deltaTime;
			if (attackCounter < 0) {
				hasAttacked = false;
				if (hasStabbed) {
					masterAnimator.SetBool("isStabbing", false);
					hasStabbed = false;
				}
				
				if (hasHacked) {
					masterAnimator.SetBool("isHacking", false);
					hasHacked = false;
				}

				if (hasSlashed) {
					masterAnimator.SetBool("isSlashing", false);
					hasSlashed = false;
				}
				attackCounter = attackTimer;
			}
		}

		if (Input.GetKey("w") || Input.GetKey("s")) {
			masterAnimator.SetBool("isWalking", true);
		}

		if (Input.GetKeyUp("w") || Input.GetKey("s")) {
			masterAnimator.SetBool("isWalking", false);
		}

		if (Input.GetMouseButtonDown(0)) {
			masterAnimator.SetBool("isStabbing", true);
			hasStabbed = true;
			hasAttacked = true;
		}

		if (Input.GetMouseButtonDown(1)) {
			masterAnimator.SetBool("isHacking", true);
			hasHacked = true;
			hasAttacked = true;
		}

		if (Input.GetMouseButtonDown(2)) {
			masterAnimator.SetBool("isSlashing", true);
			hasSlashed = true;
			hasAttacked = true;
		}
	}
}
