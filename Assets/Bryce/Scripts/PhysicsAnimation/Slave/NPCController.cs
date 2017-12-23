using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour {
	public static int removedBodyParts;

	// Declare root slave and master models.
	private Transform NPCSlave;
	public Transform NPCMaster;
	public Rigidbody rootCOG;

	// Lists used to store transforms from their respective slave / master model.
	public ConfigurableJoint[] slaveTransforms;
	public Transform[] masterTransforms;
	
	// Holds slave / master key-value pairs to later compare rotations.
	public static Dictionary<ConfigurableJoint, Transform> armatureDictionary;

	public float defaultSpringValue, defaultDampenValue, standUpDelay;

	public JointDrive jointDrive;
	
	public static bool standUp, isFalling, isDead;

	private Quaternion previousRotation;

	// Use this for initialization
	void Start () {
		previousRotation = rootCOG.transform.localRotation;
		NPCSlave = transform;

		if (Mathf.Abs(Physics.gravity.y) > 9.81f) {
			defaultSpringValue += defaultSpringValue * ((Mathf.Abs(Physics.gravity.y) - 9.81f));
		}

		armatureDictionary = new Dictionary<ConfigurableJoint, Transform>();
		
		// Grab all children with a Transform component, and add them to lists.
		slaveTransforms = NPCSlave.GetComponentsInChildren<ConfigurableJoint>();

		masterTransforms = NPCMaster.GetComponentsInChildren<Transform>();

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
		if (removedBodyParts > 2 && !isDead) {
			isDead = true;
			DeathController.KillObject(gameObject);
		}

		if (standUp) {
			rootCOG.transform.localRotation = Quaternion.Slerp(rootCOG.transform.localRotation, previousRotation, Time.deltaTime * standUpDelay);
			rootCOG.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			ApplyJointValues(false);
		} 

		if (rootCOG.transform.localRotation == previousRotation) {
			standUp = false;
		}

		if (!NPCGroundCollisionController.onGround) {
			standUp = false;
			ApplyJointValues(true);
			rootCOG.constraints = RigidbodyConstraints.None;
		} else {
			isFalling = false;
			standUp = true;
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
						if (boneTransforms.Key.name.ToLower().Contains("arm_r") && PlayerController.attacking) {
							jointDrive.positionSpring = defaultSpringValue;
							boneTransforms.Key.GetComponent<ConfigurableJoint>().slerpDrive = jointDrive;
						} else {
							jointDrive.positionSpring = 0;
							boneTransforms.Key.GetComponent<ConfigurableJoint>().slerpDrive = jointDrive;
						}
					}
				}
			}
	}
}
