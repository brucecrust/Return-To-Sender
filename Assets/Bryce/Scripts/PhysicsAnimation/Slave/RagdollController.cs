
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	This class strips bone information from a master and slave model,
	compares their rotations, and uses the `ConfigurableJointExtensions` 
	extension to move the slave bones to match their master's rotation.
*/
public class RagdollController : MonoBehaviour {
	public static int removedBodyParts;

	// Declare root slave and master models.
	private Transform rootSlave;
	public Transform rootMaster;
	public Rigidbody rootCoG;

	// Lists used to store transforms from their respective slave / master model.
	public ConfigurableJoint[] slaveTransforms;
	public Transform[] masterTransforms;
	
	// Holds slave / master key-value pairs to later compare rotations.
	public static Dictionary<ConfigurableJoint, Transform> armatureDictionary;

	public float defaultSpringValue;

	private JointDrive jointDrive;
	
	private bool onGround;

	// Use this for initialization
	void Start () {
		rootSlave = transform;

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

		ApplyJointValues(false);
	}

	void Update() {
		if (!GroundCollisionController.onGround) {
			ApplyJointValues(true);
		} else {
			ApplyJointValues(false);
		}
	}

	void FixedUpdate () {
			foreach(KeyValuePair<ConfigurableJoint, Transform> boneTransforms in armatureDictionary) {
				boneTransforms.Key.SetTargetRotationLocal(boneTransforms.Value.localRotation, boneTransforms.Key.GetComponent<Transform>().localRotation);
			}
	}

	private void ApplyJointValues (bool ragdoll) {
			foreach(KeyValuePair<ConfigurableJoint, Transform> boneTransforms in armatureDictionary) {
				if (boneTransforms.Key.GetComponent<ConfigurableJoint>()) {
					jointDrive = new JointDrive();
					jointDrive.maximumForce = float.MaxValue;
					if (!ragdoll) {
						if (boneTransforms.Key.name.ToLower().Contains("lower")) {
							jointDrive.positionSpring = defaultSpringValue / 2f;
						} else {
							jointDrive.positionSpring = defaultSpringValue;
						}
						
					} else {
						jointDrive.positionSpring = 0;
					}
					boneTransforms.Key.GetComponent<ConfigurableJoint>().slerpDrive = jointDrive;
				}
			}
	}
}
