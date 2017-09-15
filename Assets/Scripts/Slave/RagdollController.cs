
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
	public float springGravityMultiplier = 1000f;
	public float defaultLegSpringValue = 18000f;
	public float defaultTorsoSpringValue = 15000f;
	public float defaultArmSpringValue = 10000f;
	private JointDrive legJointDrive, torsoJointDrive, armJointDrive;

	// Use this for initialization
	void Start () {
		if (Mathf.Abs(Physics.gravity.y) > 9.81f) {
			defaultLegSpringValue += springGravityMultiplier * (Mathf.Abs(Physics.gravity.y) - 9.81f);
			defaultTorsoSpringValue += springGravityMultiplier * (Mathf.Abs(Physics.gravity.y) - 9.81f);
			defaultArmSpringValue += springGravityMultiplier * (Mathf.Abs(Physics.gravity.y) - 9.81f);
		}
		legJointDrive = new JointDrive();
		legJointDrive.positionSpring = defaultLegSpringValue;
		legJointDrive.maximumForce = float.MaxValue;

		torsoJointDrive = new JointDrive();
		torsoJointDrive.positionSpring = defaultLegSpringValue;
		torsoJointDrive.maximumForce = float.MaxValue;

		armJointDrive = new JointDrive();
		armJointDrive.positionSpring = defaultLegSpringValue;
		armJointDrive.maximumForce = float.MaxValue;

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

		ApplyJointValues();
	}

	void LateUpdate () {
		foreach(KeyValuePair<Transform, Transform> boneTransforms in armatureDictionary) {
			boneTransforms.Key.GetComponent<ConfigurableJoint>().SetTargetRotationLocal(boneTransforms.Value.localRotation, boneTransforms.Key.localRotation);
		}
	}

	private void ApplyJointValues () {
		foreach(KeyValuePair<Transform, Transform> boneTransforms in armatureDictionary) {
			if (boneTransforms.Key.GetComponent<ConfigurableJoint>()) {
				if (boneTransforms.Key.name.ToLower().Contains("shin") || boneTransforms.Key.name.ToLower().Contains("thigh")) {
					boneTransforms.Key.GetComponent<ConfigurableJoint>().slerpDrive = legJointDrive;
				}

				if (boneTransforms.Key.name.ToLower().Contains("chest") || boneTransforms.Key.name.ToLower().Contains("head")) {
					boneTransforms.Key.GetComponent<ConfigurableJoint>().slerpDrive = torsoJointDrive;
				}

				if (boneTransforms.Key.name.ToLower().Contains("arm")) {
					boneTransforms.Key.GetComponent<ConfigurableJoint>().slerpDrive = armJointDrive;
				}
			}
		}
	}
}
