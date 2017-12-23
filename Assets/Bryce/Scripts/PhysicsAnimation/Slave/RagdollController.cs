
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
	public Rigidbody rootCOG;

	// Lists used to store transforms from their respective slave / master model.
	public ConfigurableJoint[] slaveTransforms;
	public Transform[] masterTransforms;
	
	// Holds slave / master key-value pairs to later compare rotations.
	public static Dictionary<ConfigurableJoint, Transform> armatureDictionary;

	public float defaultSpringValue, defaultDampenValue, standUpDelay;

	private JointDrive jointDrive;
	
	private bool standUp;

	private Quaternion previousRotation;

	// Use this for initialization
	void Start () {
		previousRotation = rootCOG.transform.localRotation;
		print(previousRotation);
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
		if (standUp) {
			rootCOG.transform.localRotation = Quaternion.Slerp(rootCOG.transform.localRotation, previousRotation, Time.deltaTime * standUpDelay);
			rootCOG.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			ApplyJointValues(false);
		}

		if (rootCOG.transform.localRotation == previousRotation) {
			standUp = false;
		} else {
			standUp = true;
		}

		if (!GroundCollisionController.onGround) {
			ApplyJointValues(true);
			rootCOG.constraints = RigidbodyConstraints.None;
		}
	}

	void FixedUpdate () {
			foreach(KeyValuePair<ConfigurableJoint, Transform> boneTransforms in armatureDictionary) {
				boneTransforms.Key.SetTargetRotationLocal(boneTransforms.Value.localRotation, boneTransforms.Key.GetComponent<Transform>().localRotation);
			}
	}

	private void ApplyJointValues (bool isRagdoll) {
			foreach(KeyValuePair<ConfigurableJoint, Transform> boneTransforms in armatureDictionary) {
				if (boneTransforms.Key.GetComponent<ConfigurableJoint>()) {
					jointDrive = new JointDrive();
					jointDrive.maximumForce = float.MaxValue;
					if (!isRagdoll) {
						jointDrive.positionDamper = defaultDampenValue;
						if (boneTransforms.Key.name.ToLower().Contains("lower") || !boneTransforms.Key.name.ToLower().Contains("_r")) {
							jointDrive.positionSpring = defaultSpringValue / 2f;
						} else {
							jointDrive.positionSpring = defaultSpringValue;
						}
						boneTransforms.Key.GetComponent<ConfigurableJoint>().slerpDrive = jointDrive;
					} else {
						jointDrive.positionSpring = 0;
						if (!boneTransforms.Key.name.ToLower().Contains("arm_r")) {
							boneTransforms.Key.GetComponent<ConfigurableJoint>().slerpDrive = jointDrive;
						}
					}
				}
			}
	}
}
