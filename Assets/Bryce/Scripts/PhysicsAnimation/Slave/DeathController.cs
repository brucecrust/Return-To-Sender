using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour {
	public static void KillObject(GameObject killTarget) {
		if (killTarget.GetComponent<NPCController>()) {
			killTarget.GetComponent<NPCController>().rootCOG.constraints = RigidbodyConstraints.None;
			killTarget.GetComponent<NPCController>().defaultSpringValue = 0;
			killTarget.GetComponent<NPCController>().jointDrive.positionSpring = killTarget.GetComponent<NPCController>().defaultSpringValue;
			foreach(KeyValuePair<ConfigurableJoint, Transform> boneTransforms in NPCController.armatureDictionary) {
				boneTransforms.Key.slerpDrive = killTarget.GetComponent<NPCController>().jointDrive;
			}
		} else if (killTarget.GetComponent<RagdollController>()) {
			killTarget.GetComponent<RagdollController>().rootCOG.constraints = RigidbodyConstraints.None;
			killTarget.GetComponent<RagdollController>().defaultSpringValue = 0;
			killTarget.GetComponent<RagdollController>().jointDrive.positionSpring = killTarget.GetComponent<RagdollController>().defaultSpringValue;
			foreach(KeyValuePair<ConfigurableJoint, Transform> boneTransforms in RagdollController.armatureDictionary) {
				boneTransforms.Key.slerpDrive = killTarget.GetComponent<RagdollController>().jointDrive;
			}
		}
	}
}
