/*
 *================================================================================
 *
 *  CameraRelative.cs
 *
 *================================================================================
 *
 *	Attach to a camera (one which is NOT parented to another object).
 *	Specify the object you wish to follow.
 *	The camera will then follow this object at a relative position.
 *
 *	When you attach this script it will automatically set itself to follow the
 *	first person controller (FPC).  If you wish to do this manually, you will need to set:
 *		FPSController > FirstPersonController > Camera = disabled
 *	
 *	v1.26 -- added to JKScriptPack.
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRelative : MonoBehaviour {

	public GameObject objectToFollow;

	private Vector3 offset;
	private Quaternion viewingAngle;
	private GameObject fpc;

	void Reset () {

		// If there is a first person controller, follow it automatically
		fpc = GameObject.Find ("FPSController");
		if (fpc) {
			objectToFollow = fpc;
		}
	}

	void Start () {
		if (objectToFollow) {
			offset = this.transform.position - objectToFollow.transform.position;
			viewingAngle = this.transform.rotation;

			// If we're following the First Person Controller, adjust its settings
			if (objectToFollow == fpc) {
			
				// Disable the FPC's native camera
				Camera fpcamera = fpc.GetComponentInChildren<Camera>();
				if (fpcamera) {
					fpcamera.enabled = false;
				}

			}

		}
	}
	
	void Update () {
		if (objectToFollow) {
			this.transform.position = objectToFollow.transform.position + offset;
			this.transform.rotation = viewingAngle;
		}
	}

}
