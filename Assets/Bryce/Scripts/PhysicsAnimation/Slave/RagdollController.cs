
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	This class strips bone information from a master and slave model,
	compares their rotations, and uses the `ConfigurableJointExtensions` 
	extension to move the slave bones to match their master's rotation.
*/
public class RagdollController : MonoBehaviour {
	// Declare root slave and master models.
	public Transform rootSlave;
	public Transform rootMaster;

	// Declare root bone and the slave model's respective sword model.


	// Lists used to store transforms from their respective slave / master model.
	public Transform[] slaveTransforms;
	public Transform[] masterTransforms;
	
	// Holds slave / master key-value pairs to later compare rotations.
	public Dictionary<Transform, Transform> armatureDictionary;

	public float defaultLegSpringValue;
	public float defaultTorsoSpringValue;
	public float defaultArmSpringValue;
	private JointDrive legJointDrive, torsoJointDrive, armJointDrive;

	public GameObject deathScreen;

	// Use this for initialization
	void Start () {
		if (Mathf.Abs(Physics.gravity.y) > 9.81f) {
			defaultLegSpringValue += defaultLegSpringValue * ((Mathf.Abs(Physics.gravity.y) - 9.81f));
			defaultTorsoSpringValue += defaultTorsoSpringValue * ((Mathf.Abs(Physics.gravity.y) - 9.81f));
			defaultArmSpringValue += defaultArmSpringValue * ((Mathf.Abs(Physics.gravity.y) - 9.81f));
		}
		//attackingCounter = attackingTimer;

		armatureDictionary = new Dictionary<Transform, Transform>();

		// Grab all children with a Transform component, and add them to lists.
		slaveTransforms = rootSlave.GetComponentsInChildren<Transform>();

		masterTransforms = rootMaster.GetComponentsInChildren<Transform>();

		// For every bone in those lists, check if it has a joint. If it does, add it to the dictionary.
		foreach (Transform slaveBone in slaveTransforms) {
			foreach (Transform masterBone in masterTransforms) {
				if (slaveBone.name == masterBone.name) {
					if (slaveBone.GetComponent<ConfigurableJoint>()) {
						armatureDictionary.Add(slaveBone, masterBone);
					}
				}
			}
		}

		ApplyJointValues(true);
	}

	void Update () {
		if (DeathController.isDead) {
			foreach(KeyValuePair<Transform, Transform> boneTransforms in armatureDictionary) {
				DropDead();
			}
		}

		if (!DeathController.isDead) {
			foreach(KeyValuePair<Transform, Transform> boneTransforms in armatureDictionary) {
				if (!SpringUpOnCollision.isMoving) {
					ApplyJointValues(false);
					if (!boneTransforms.Key.name.ToLower().Contains("arm")) {
						boneTransforms.Key.GetComponent<ConfigurableJoint>().SetTargetRotationLocal(boneTransforms.Value.localRotation, boneTransforms.Key.localRotation);
					}
				} else {
					ApplyJointValues(true);
					boneTransforms.Key.GetComponent<ConfigurableJoint>().SetTargetRotationLocal(boneTransforms.Value.localRotation, boneTransforms.Key.localRotation);
				}
			}
		}
	}
	void DropDead () {
		foreach(KeyValuePair<Transform, Transform> boneTransforms in armatureDictionary) {
			if (boneTransforms.Key.GetComponent<ConfigurableJoint>()) {
				legJointDrive = new JointDrive();
				legJointDrive.positionSpring = 0;
				legJointDrive.maximumForce = float.MaxValue;

				boneTransforms.Key.GetComponent<ConfigurableJoint>().slerpDrive = legJointDrive;
			}
		}
		deathScreen.SetActive(true);
		Destroy(this);
	}

	private void ApplyJointValues (bool enableSlerp) {
		if (enableSlerp) {
			foreach(KeyValuePair<Transform, Transform> boneTransforms in armatureDictionary) {
				if (boneTransforms.Key.GetComponent<ConfigurableJoint>()) {
					legJointDrive = new JointDrive();
					legJointDrive.maximumForce = float.MaxValue;

					torsoJointDrive = new JointDrive();
					torsoJointDrive.maximumForce = float.MaxValue;

					armJointDrive = new JointDrive();
					armJointDrive.maximumForce = float.MaxValue;

					if (boneTransforms.Key.name.ToLower().Contains("thigh")) {
						legJointDrive.positionSpring = defaultLegSpringValue;
						boneTransforms.Key.GetComponent<ConfigurableJoint>().slerpDrive = legJointDrive;
					}

					if (boneTransforms.Key.name.ToLower().Contains("shin")) {
						legJointDrive.positionSpring = defaultLegSpringValue  / 6f;
						boneTransforms.Key.GetComponent<ConfigurableJoint>().slerpDrive = legJointDrive;
					}

					if (boneTransforms.Key.name.ToLower().Contains("chest")) {
						torsoJointDrive.positionSpring = defaultTorsoSpringValue;
						boneTransforms.Key.GetComponent<ConfigurableJoint>().slerpDrive = torsoJointDrive;
					}

					if (boneTransforms.Key.name.ToLower().Contains("head")) {
						torsoJointDrive.positionSpring = defaultTorsoSpringValue;
						boneTransforms.Key.GetComponent<ConfigurableJoint>().slerpDrive = torsoJointDrive;
					}

					if (boneTransforms.Key.name.ToLower().Contains("upper_arm")) {
						armJointDrive.positionSpring = defaultArmSpringValue;
						boneTransforms.Key.GetComponent<ConfigurableJoint>().slerpDrive = armJointDrive;
					}

					if (boneTransforms.Key.name.ToLower().Contains("forearm")) {
						armJointDrive.positionSpring = defaultArmSpringValue / 5f;
						boneTransforms.Key.GetComponent<ConfigurableJoint>().slerpDrive = armJointDrive;
					}

					if (boneTransforms.Key.name.ToLower().Contains("hand")) {
						armJointDrive.positionSpring = defaultArmSpringValue;
						boneTransforms.Key.GetComponent<ConfigurableJoint>().slerpDrive = armJointDrive;
					}
				}
			}
		} else {
			foreach(KeyValuePair<Transform, Transform> boneTransforms in armatureDictionary) {
				armJointDrive = new JointDrive();
				armJointDrive.positionSpring = defaultArmSpringValue / 5f;
				armJointDrive.maximumForce = float.MaxValue;
				if (boneTransforms.Key.GetComponent<ConfigurableJoint>()) {
					if (boneTransforms.Key.name.ToLower().Contains("arm")) {
						boneTransforms.Key.GetComponent<ConfigurableJoint>().slerpDrive = armJointDrive;
					}
				}
			}
		}
	}
}
