
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

	public float defaultSpringValue, defaultDampenValue, standUpDelay, sleepOnFallTimer, maxAllowedFallLength;

	private JointDrive jointDrive;
	
	public static bool standUp, isFalling;

	private Quaternion previousRotation;

	public static float standupCounter, fallingTime, standupTimer;

	// Use this for initialization
	void Start () {
		standupCounter = sleepOnFallTimer;
		standupTimer = sleepOnFallTimer;
		previousRotation = rootCOG.transform.localRotation;
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
			if (standupCounter < 0 || fallingTime < maxAllowedFallLength) {
				fallingTime = 0;
				standupCounter = sleepOnFallTimer;
				rootCOG.transform.localRotation = Quaternion.Slerp(rootCOG.transform.localRotation, previousRotation, Time.deltaTime * standUpDelay);
				rootCOG.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
				ApplyJointValues(false);
			} 

			if (standupCounter > 0) {
				standupCounter -= Time.deltaTime;
			}
		}

		if (PlayerController.hasJumped && fallingTime < maxAllowedFallLength) {
			standupCounter = standupCounter / 4f;
		}

		if (rootCOG.transform.localRotation == previousRotation) {
			standUp = false;
		}

		if (!GroundCollisionController.onGround) {
			standUp = false;
			standupCounter = sleepOnFallTimer;
			ApplyJointValues(true);
			rootCOG.constraints = RigidbodyConstraints.None;
			if (!PlayerController.hasJumped) {
				isFalling = true;
			}
		} else {
			isFalling = false;
		}

		if (isFalling) {
			if (fallingTime < 5f) {
				fallingTime += Time.deltaTime;	
			}
			
			if (fallingTime > maxAllowedFallLength) {
				isFalling = true;
			}

		} else if (!isFalling && !standUp) {
			standupCounter += fallingTime;
			//fallingTime = 0;
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
						if (boneTransforms.Key.name.ToLower().Contains("lower") && !boneTransforms.Key.name.ToLower().Contains("_r")) {
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
