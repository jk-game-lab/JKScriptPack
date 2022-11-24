/*
 * 	Latcher.cs
 * 
 * 	When the First Person Controller (FPC) wanders into a trigger area
 * 	then the FPC will latch to this object.  Pressing any key will make
 * 	the FPC un-latch.
 * 
 * 	Apply this script to the trigger area.
 * 
 *	v1.29 -- added to JKScriptPack.
 */

using UnityEngine;
using System.Collections;

public class Latcher : MonoBehaviour {

	public KeyCode keyToHold = KeyCode.L;

	private GameObject fpc;					// Set to FPC if latched; null if not latched

	private Quaternion fpcOriginalRotation;

	void Start () {
		fpcOriginalRotation = this.transform.rotation;		// Set a default just in case
	}

	void OnTriggerEnter(Collider other) {
		if (!fpc && keyToHold == KeyCode.None) {
			Latch (other);
		}
	}

	void OnTriggerStay(Collider other) {
		
		if (!fpc && keyToHold != KeyCode.None && Input.GetKeyDown (keyToHold)) {
			Latch (other);
		}

		// Move with trigger zone
		if (fpc) {
			fpc.transform.position = this.transform.position;
			fpc.transform.rotation = this.transform.rotation;
		}

		// Unlatch?
		if (fpc && ((keyToHold == KeyCode.None && Input.anyKeyDown) || Input.GetKeyUp(keyToHold)) ) {
			fpc.transform.rotation = fpcOriginalRotation;
			fpc = null;
		}

	}

	void Latch(Collider other) {
		if (other.gameObject.name == "FPSController" || other.gameObject.name == "RigidbodyFPSController" || other.gameObject.name == "ThirdPersonController") {
			fpc = other.gameObject;
			fpcOriginalRotation = fpc.transform.localRotation;
		}
	}

}
