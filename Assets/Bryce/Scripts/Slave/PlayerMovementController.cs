using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {
	private RagdollController ragdollController;

	public Transform rootBone;

	// Collision handlers that signify when to add an upward force to the legs; important for walking / running animations.
	public GroundCollisionHandler leftFootCollisionHandler;
	public GroundCollisionHandler rightFootCollisionHandler;

	public float playerMovementSpeed, upwardSpringSpeed, rootBoneRotationDivisor;
	
	// Use this for initialization
	void Start () {
		ragdollController = GetComponent<RagdollController>();
	}

	void FixedUpdate () {
		if (Input.GetKey("w")) {
			foreach(KeyValuePair<Transform, Transform> boneTransforms in ragdollController.armatureDictionary) {
				if (boneTransforms.Key.name.Contains("shin")) {
					// Move the slave's shins forward.
					boneTransforms.Key.GetComponent<Rigidbody>().AddForce(-boneTransforms.Key.forward * (playerMovementSpeed));

					// If one of the slave's feet is touching the ground, spring him upward for the next step.
					if (leftFootCollisionHandler.onGround) {
						if (boneTransforms.Key.name.Contains("shin") && boneTransforms.Key.name.ToLower().Contains("l")) {
							boneTransforms.Key.GetComponent<Rigidbody>().AddForce(boneTransforms.Key.right * playerMovementSpeed * upwardSpringSpeed);
						}
					}

					if (rightFootCollisionHandler.onGround) {
						if (boneTransforms.Key.name.Contains("shin") && boneTransforms.Key.name.ToLower().Contains("r")) {
							boneTransforms.Key.GetComponent<Rigidbody>().AddForce(boneTransforms.Key.right * playerMovementSpeed * upwardSpringSpeed);
						}
					}
				}
			}
		}

		if (Input.GetKey("s")) {
			foreach(KeyValuePair<Transform, Transform> boneTransforms in ragdollController.armatureDictionary) {
				if (boneTransforms.Key.name.Contains("shin")) {
					boneTransforms.Key.GetComponent<Rigidbody>().AddForce(boneTransforms.Key.forward * (playerMovementSpeed));
					if (leftFootCollisionHandler.onGround) {
						if (boneTransforms.Key.name.Contains("shin") && boneTransforms.Key.name.ToLower().Contains("l")) {
							boneTransforms.Key.GetComponent<Rigidbody>().AddForce(boneTransforms.Key.right * playerMovementSpeed * upwardSpringSpeed);
							rootBone.GetComponent<Rigidbody>().AddForce(boneTransforms.Key.right * playerMovementSpeed * upwardSpringSpeed);
						}
					}

					if (rightFootCollisionHandler.onGround) {
						if (boneTransforms.Key.name.Contains("shin") && boneTransforms.Key.name.ToLower().Contains("r")) {
							boneTransforms.Key.GetComponent<Rigidbody>().AddForce(boneTransforms.Key.right * playerMovementSpeed * upwardSpringSpeed);
							rootBone.GetComponent<Rigidbody>().AddForce(boneTransforms.Key.right * playerMovementSpeed * upwardSpringSpeed);
						}
					}
				}
			}
		}

		if (Input.GetKey("a")) {
			rootBone.Rotate(transform.right * (playerMovementSpeed / rootBoneRotationDivisor) * Time.fixedDeltaTime);
		}

		if (Input.GetKey("d")) {

			rootBone.Rotate(-transform.right * (playerMovementSpeed / rootBoneRotationDivisor) * Time.fixedDeltaTime);
		} 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
