/*
 * 	Grabber.cs
 *
 * 	Allows a player to "grab" objects.
 *
 *	Create a trigger zone and attach it to the player as a kind of 'invisible hand'.
 *	A grabbable object that comes within this zone may be grabbed by pressing a key.
 *
 *	To make an object grabbable, change its tag to "grabbable".  (You may need to add a new tag to the list.)
 * 
 * 	Apply this scrip to the player's 'invisible hand' trigger zone.
 * 
 *	v1.33 -- added to JKScriptPack.
 *	
 *	
 *	NEED UPDATING
 *	
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour {

	[Tooltip("Which key grabs things?")]
	public KeyCode grabKey = KeyCode.G;

	[Tooltip("Press key to grab and again to release?")]
	public bool toggle = true;

	[Tooltip("Does the grabbable get pulled to me?")]
	public bool moveToMe = true;
	public float moveSpeed = 10.0f;
//	public bool followMe = false;

	private GameObject triggered = null;
	private GameObject grabbed = null;
	private bool gravity;
	private bool trapped;

	private Vector3 relativePosition;

	void Start () {
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag.ToLower () == "grabbable") {
			triggered = other.gameObject;
		}
	}

	void OnTriggerExit(Collider other)
	{
		triggered = null;
	}

	void Grab(GameObject grabbable) {
		grabbed = grabbable;
		relativePosition = grabbed.transform.position - this.transform.position;
		Rigidbody rb = grabbed.GetComponentInChildren<Rigidbody>();
		if (rb) {
			gravity = rb.useGravity;
			rb.useGravity = false;
		}
		trapped = false;
	}

	void Release() {
		if (grabbed) {
			Rigidbody rb = grabbed.GetComponentInChildren<Rigidbody> ();
			if (rb) {
				rb.useGravity = gravity;
			}
			trapped = false;
			grabbed = null;
		}
	}

	void Update () {

		if (Input.GetKeyDown(grabKey)) {
			if (triggered && !grabbed) {
				Grab (triggered);
			} else {
				if (toggle) {
					Release ();
				}
			}
		} else if (Input.GetKeyUp(grabKey) && !toggle) {
			Release ();
		}

		if (grabbed) {
			if (moveToMe && !trapped) {
				if (relativePosition.magnitude > 0.1f) {
					grabbed.transform.position = Vector3.Lerp (grabbed.transform.position, this.transform.position, Time.deltaTime * moveSpeed);
					relativePosition = grabbed.transform.position - this.transform.position; 
				} else {
					grabbed.transform.position = this.transform.position;
					//if (!followMe) {
						trapped = true;
					//}
				}
			}
			grabbed.transform.position = this.transform.position + relativePosition;
		}
	}
}
