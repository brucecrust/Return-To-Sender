
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
	public ConfigurableJoint[] slaveTransforms;
	public Transform[] masterTransforms;
	
	// Holds slave / master key-value pairs to later compare rotations.
	public Dictionary<ConfigurableJoint, Transform> armatureDictionary;

	public float defaultSpringValue;
	private JointDrive legJointDrive, torsoJointDrive, armJointDrive;

	//public GameObject deathScreen;

	// Use this for initialization
	void Start () {
		if (Mathf.Abs(Physics.gravity.y) > 9.81f) {
			defaultSpringValue += defaultSpringValue * ((Mathf.Abs(Physics.gravity.y) - 9.81f));
		}

		armatureDictionary = new Dictionary<ConfigurableJoint, Transform>();
		
		// Grab all children with a Transform component, and add them to lists.
		slaveTransforms = rootSlave.GetComponentsInChildren<ConfigurableJoint>();

		masterTransforms = rootMaster.GetComponentsInChildren<Transform>();

		// For every bone in those lists, check if it has a joint. If it does, add it to the dictionary.
		foreach (ConfigurableJoint slaveBone in slaveTransforms) {
			foreach (Transform masterBone in masterTransforms) {
				if (slaveBone.name == masterBone.name) {
					armatureDictionary.Add(slaveBone, masterBone);
				}
			}
		}
		ApplyJointValues();
	}

	void Update () {
		if (DeathController.isDead) {
			foreach(KeyValuePair<ConfigurableJoint, Transform> boneTransforms in armatureDictionary) {
				DropDead();
			}
		}

		if (!DeathController.isDead) {
			foreach(KeyValuePair<ConfigurableJoint, Transform> boneTransforms in armatureDictionary) {
				boneTransforms.Key.SetTargetRotationLocal(boneTransforms.Value.localRotation, boneTransforms.Key.GetComponent<Transform>().localRotation);
			}
		}
	}
	void DropDead () {
		foreach(KeyValuePair<ConfigurableJoint, Transform> boneTransforms in armatureDictionary) {
			if (boneTransforms.Key.GetComponent<ConfigurableJoint>()) {
				legJointDrive = new JointDrive();
				legJointDrive.positionSpring = 0;
				legJointDrive.maximumForce = float.MaxValue;

				boneTransforms.Key.GetComponent<ConfigurableJoint>().slerpDrive = legJointDrive;
			}
		}
		//deathScreen.SetActive(true);
		Destroy(this);
	}

	private void ApplyJointValues () {
			foreach(KeyValuePair<ConfigurableJoint, Transform> boneTransforms in armatureDictionary) {
				if (boneTransforms.Key.GetComponent<ConfigurableJoint>()) {
					legJointDrive = new JointDrive();
					legJointDrive.maximumForce = float.MaxValue;

					torsoJointDrive = new JointDrive();
					torsoJointDrive.maximumForce = float.MaxValue;

					armJointDrive = new JointDrive();
					armJointDrive.maximumForce = float.MaxValue;

					if (boneTransforms.Key.name.ToLower().Contains("upper leg")) {
						legJointDrive.positionSpring = defaultSpringValue;
						boneTransforms.Key.GetComponent<ConfigurableJoint>().slerpDrive = legJointDrive;
					}

					if (boneTransforms.Key.name.ToLower().Contains("lower leg")) {
						legJointDrive.positionSpring = defaultSpringValue;
						boneTransforms.Key.GetComponent<ConfigurableJoint>().slerpDrive = legJointDrive;
					}

					if (boneTransforms.Key.name.ToLower().Contains("spine")) {
						torsoJointDrive.positionSpring = defaultSpringValue / 5f;
						boneTransforms.Key.GetComponent<ConfigurableJoint>().slerpDrive = torsoJointDrive;
					}

					if (boneTransforms.Key.name.ToLower().Contains("head")) {
						torsoJointDrive.positionSpring = defaultSpringValue;
						boneTransforms.Key.GetComponent<ConfigurableJoint>().slerpDrive = torsoJointDrive;
					}

					if (boneTransforms.Key.name.ToLower().Contains("upper arm")) {
						armJointDrive.positionSpring = defaultSpringValue;
						boneTransforms.Key.GetComponent<ConfigurableJoint>().slerpDrive = armJointDrive;
					}

					if (boneTransforms.Key.name.ToLower().Contains("lower arm")) {
						armJointDrive.positionSpring = defaultSpringValue / 15f;
						boneTransforms.Key.GetComponent<ConfigurableJoint>().slerpDrive = armJointDrive;
					}

					if (boneTransforms.Key.name.ToLower().Contains("hand")) {
						armJointDrive.positionSpring = defaultSpringValue / 10f;
						boneTransforms.Key.GetComponent<ConfigurableJoint>().slerpDrive = armJointDrive;
					}
				}
			}
	}
}
