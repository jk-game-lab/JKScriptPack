/*
 * 	Bucket.cs
 * 
 * 	Detects whether a set of rigidbody objects have moved into a trigger zone.
 * 
 * 	When all objects have been collected, another gameobject (such as a barrier or prize)
 * 	will be revealed (or hidden).
 * 
 * 	v1.35 -- added to JKScriptPack
 *
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bucket : MonoBehaviour {

	[System.Serializable]
	public class Collectable {
		public GameObject gameObject;
		public bool collected = false;
		//public int points = 10;
	}
	public Collectable[] collectables;

	public AudioClip collectionSound;
	public AudioClip finishedSound;
	public GameObject finishedReveal;
	public GameObject finishedHide;

	private AudioSource audiosource;

	void Start () {

		// Reset rewards
		if (finishedReveal) {
			finishedReveal.gameObject.SetActive(false);
		}
		if (finishedHide) {
			finishedHide.gameObject.SetActive(true);
		}

		// Initialise audio
		audiosource = gameObject.AddComponent<AudioSource>();

	}

	void OnTriggerEnter(Collider other) {
		CheckCollectable (other.gameObject, true);
	}

	void OnTriggerExit(Collider other) {
		CheckCollectable (other.gameObject, false);
	}

	void CheckCollectable(GameObject other, bool state) {

		// Is the item in our list?
		foreach (Collectable collectable in collectables) {
			if (other.Equals(collectable.gameObject)) {

				// Collect it
				collectable.collected = state;
				if (state) audiosource.PlayOneShot(collectionSound);

				// Have all objects been collected?
				if (AllCollected()) {
					if (finishedReveal) {
						finishedReveal.gameObject.SetActive(true);
					}
					if (finishedHide) {
						finishedHide.gameObject.SetActive(false);
					}
					audiosource.PlayOneShot(finishedSound);
				}

				break;
			}
		}
	}

	public bool AllCollected() {
		bool all = true;
		foreach (Collectable item in collectables) {
			if (!item.collected) {
				all = false;
				break;
			}
		}
		return all;
	}

}
