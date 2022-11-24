/*
 * 	Hopper.cs
 * 
 * 	Attach this to a gameobject.  Pressing set keys will make the object
 * 	move to a waypoint position.
 * 
 * 	v1.37 -- added to JKScriptPack
 *
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hopper : MonoBehaviour {

	[System.Serializable]
	public class Waypoint {
		public GameObject gameObject;
		public KeyCode directKey = KeyCode.None;
	}
	public Waypoint[] waypoints;
	public int currentWaypoint = 0;

	public float speed = 1.0f;

	public KeyCode nextKey = KeyCode.None;
	public KeyCode prevKey = KeyCode.None;

	void Start () {
		
	}
	
	void Update () {
	
		// Prev/Next keys
		if (Input.GetKeyDown(prevKey)) {
			currentWaypoint--;
			if (currentWaypoint < 0) {
				currentWaypoint = 0;
			}
		}
		if (Input.GetKeyDown(nextKey)) {
			currentWaypoint++;
			if (currentWaypoint > waypoints.Length) {
				currentWaypoint = waypoints.Length;
			}
		}

		// Direct key
		for (int i = 0; i < waypoints.Length; i++ ) {
			if (Input.GetKeyDown(waypoints[i].directKey)) {
				currentWaypoint = i;
				break;
			}
		}

		// Do we need to move?
		Vector3 current = this.transform.position;
		Vector3 target = waypoints[currentWaypoint].gameObject.transform.position;
		Vector3 movement = target - current;
		if (movement.magnitude < 0.1) {
			this.transform.position = target;
		} else {
			this.transform.position = Vector3.Lerp(current, target, speed * Time.deltaTime);
		}

	}
}
