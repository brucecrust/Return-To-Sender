using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringUpOnCollision : MonoBehaviour {
	private RagdollController ragdollController;

	public Transform rootBone;
	public Rigidbody leftThigh;
	public Rigidbody rightThigh;
	public Rigidbody leftShin;
	public Rigidbody rightShin;
	public GroundCollisionHandler leftFootCollisionHandler;
	public GroundCollisionHandler rightFootCollisionHandler;

	public float upwardSpringSpeed;
	public static bool isMoving;
	
	private Component[] childrenList;
	
	// Use this for initialization
	void Start () {
		ragdollController = GetComponent<RagdollController>();

		childrenList = GetComponentsInChildren(typeof(Transform));
	}

	void FixedUpdate () {
		if (Input.GetKey("w")) {
			isMoving = true;
			// If one of the slave's feet is touching the ground, spring him upward for the next step.
			if (leftFootCollisionHandler.onGround) {
				leftThigh.AddForce(-leftThigh.transform.up * upwardSpringSpeed);
				leftShin.AddForce(-leftShin.transform.up * upwardSpringSpeed);
			}
			if (rightFootCollisionHandler.onGround) {
				rightThigh.AddForce(-leftShin.transform.up * upwardSpringSpeed);
				rightShin.AddForce(-leftShin.transform.up * upwardSpringSpeed);
			}
		}

		/*if (Input.GetKey("s")) {
			isMoving = true;
			if (leftFootCollisionHandler.onGround) {
				leftShin.AddForce(leftShin.transform.forward * (upwardSpringSpeed / 3f), ForceMode.Impulse);
				leftShin.AddForce(leftShin.transform.right * upwardSpringSpeed, ForceMode.Impulse);
			}
			if (rightFootCollisionHandler.onGround) {
				rightShin.AddForce(rightShin.transform.forward * (upwardSpringSpeed / 3f), ForceMode.Impulse);
				leftShin.AddForce(rightShin.transform.right * upwardSpringSpeed, ForceMode.Impulse);
			}
		}*/

		if (Input.GetKeyUp("w") || Input.GetKeyUp("s")) {
			isMoving = false;
		}
	}
}
