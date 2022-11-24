/*
 *	BetterOnTriggerReveal.cs
 * 
 *	Reveals an object (usually a UI object, like instructions
 *	for the player) whenever the first person controller
 *	enters a trigger zone.
 *
 *	On exit, another object can be activated, like a door trigger
 *	or a trigger for another puzzle clue.
 *
 *	Attach this script to the trigger zone.
 *
 *	v1.11 -- re-written in C#; both showing & hiding now supported.
 *	v1.29 -- verified that this works correctly with RigidbodyFirstPersonController
 *			 and ThirdPersonController.
 *	v1.30 -- added option to disable auto-hide on trigger exit.
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterOnTriggerReveal : MonoBehaviour {

	public KeyCode keypressNeeded = KeyCode.None;
	public GameObject revealObject;				// for next version call this "tempReveal"
	public GameObject hideObject;
	public AudioClip playSound;
	public bool hideOnExit = true;
	public bool permanent = false;
	public float timeout = 0.0f;
	public AudioClip exitSound;
	public GameObject onExitEnable;				// for next version call this "permanentReveal"
	public GameObject onExitDisable;

	private bool withinTriggerZone = false;
	private bool activated;
	private float countdown;
	private AudioSource audiosource;

	void Start () {

		// Initialise objects
		if (revealObject) revealObject.SetActive(false);
		if (hideObject) hideObject.SetActive(true);

		withinTriggerZone = false;
		activated = false;

		// Initialise audio
		audiosource = gameObject.AddComponent<AudioSource>();
	}

	void OnTriggerEnter (Collider other) {
		withinTriggerZone = true;
		if (keypressNeeded == KeyCode.None) {
			Activate();
		}
	}
	void Activate() {
		if (!activated) {
			activated = true;
			if (revealObject) revealObject.SetActive(true);
			if (hideObject) hideObject.SetActive(false);
			audiosource.PlayOneShot(playSound);
			countdown = timeout;
		}
	}

	void OnTriggerExit (Collider other) {
		withinTriggerZone = false;
		if (hideOnExit) {
			Deactivate ();
		}
	}
	void Deactivate() {
		if (activated && !permanent) {
			activated = false;
			if (revealObject) revealObject.SetActive(false);
			if (hideObject) hideObject.SetActive(true);
		}
		audiosource.PlayOneShot(exitSound);
		if (onExitEnable) onExitEnable.SetActive(true);
		if (onExitDisable) onExitDisable.SetActive(false);
		countdown = 0.0f;
	}

	void Update () {
		if (withinTriggerZone && Input.GetKeyDown(keypressNeeded)) {
			if (activated) {
				Deactivate();
			} else {
				Activate();
			}
		}
		if (activated && countdown > 0) {
			countdown -= Time.deltaTime;
			if (countdown <= 0) {
				Deactivate();
			}
		}
	}

}
	