
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

	// Use this for initialization
	void Start () {
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
	}

	void LateUpdate () {
		foreach(KeyValuePair<Transform, Transform> boneTransforms in armatureDictionary) {
			boneTransforms.Key.GetComponent<ConfigurableJoint>().SetTargetRotationLocal(boneTransforms.Value.localRotation, boneTransforms.Key.localRotation);
		}
	}
}
