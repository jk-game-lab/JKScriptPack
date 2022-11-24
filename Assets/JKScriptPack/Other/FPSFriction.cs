/*
 *	FPSFriction.cs
 *
 *	Makes the RigidbodyFPSController follow the movement of the surface it
 *	has collided with (e.g. a moving platform).
 *
 *	Can also be used with ThirdPersonController.
 *
 *	v1.27 -- added to JKScriptPack.
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSFriction : MonoBehaviour {

	private Rigidbody myRigidbody;
	private Vector3 prevPos;

	void Awake () { 
		myRigidbody = this.GetComponentInChildren<Rigidbody>();
		if (myRigidbody) {
			myRigidbody.freezeRotation = true;     // Don't let the Physics Engine rotate this object (i.e. fall over when running)
		}
	}

	void OnCollisionEnter (Collision other) {
		if (other.collider.name != "Terrain") {
			prevPos = other.transform.position;
		}
	}

	void OnCollisionStay (Collision other) {
		if (other.collider.name != "Terrain") {

			// Note: cannot just read the velocity of the other object because script-controlled
			// kinematic rigidbodies have velocity 0.  Hence we have to keep track of previous position
			// in order to calculate the other object's movement.
			Vector3 deltapos = other.collider.transform.position - prevPos;
			prevPos = other.collider.transform.position;

			//Debug.Log ("OnCollisionStay: " + other.collider.name + "other{pos=" + other.rigidbody.position.ToString() + ",rbvel="  + other.rigidbody.velocity.ToString() + ",deltapos=" + deltapos.ToString() + "}, my{pos=" + transform.position.ToString() + "}");

			if (Input.GetButtonDown("Jump")) {
				// Do nothing
			} else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) {
				// Note: AddForce doesn't do anything to an idle RigidbodyFPSController
				// so cannot use myRigidbody.AddForce(velocity);
				if (myRigidbody) {
					myRigidbody.MovePosition (transform.position + deltapos);
				} else {
					transform.Translate (deltapos);
				}
			}

		}
	}

}
