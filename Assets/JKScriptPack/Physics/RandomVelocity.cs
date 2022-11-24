/*
 *	RandomVelocity.js
 *
 *	Attach to a rigidbody gameobject to set its initial velocity,
 *	randomly chosen around the Y axis.
 *
 *	v1.32 -- added to JKScriptPack.
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomVelocity : MonoBehaviour {

	public float minSpeed = 10;
	public float maxSpeed = 10;
	public float angleRange = 360;

	private Rigidbody myRigidbody;

	void Start () {
		
		myRigidbody = this.GetComponentInChildren<Rigidbody>();
		if (!myRigidbody) {
			myRigidbody = new Rigidbody();
		}

		float angle = Random.Range (-angleRange/2, angleRange/2);
		myRigidbody.rotation = Quaternion.AngleAxis(angle, Vector3.up);

		Vector3 localForward = myRigidbody.rotation * Vector3.forward;
		myRigidbody.velocity  = localForward * Random.Range (minSpeed, maxSpeed);

	}

}
