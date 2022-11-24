/*
 * 	PersonalHealth.js
 * 
 * 	Attach this to a trigger zone around an object.  It will detect
 * 	rigidbodies that enter the trigger zone (such as bullets or 
 * 	the FirstPersonController) and reduce the health accordingly.
 * 
 * 	Health is scaled from 0 to 1.
 *	
 * 	v1.39 -- added to JKScriptPack
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersonalHealth : MonoBehaviour {

	public float health = 1;

	public GameObject healthBar;
	private Vector3 originalScale;

	[System.Serializable]
	public class Threat {
		public GameObject gameObject;
		public float damage = 0.1f;
		public bool vanishOnImpact = false;
		public AudioClip collisionSound;
	}
	public Threat[] threats;

	public string deathScene;

	private AudioSource audiosource;

	void Start () {

		// Initialise audio
		audiosource = gameObject.AddComponent<AudioSource>();

		if (healthBar) {
			originalScale = healthBar.transform.localScale;
		}
	}
	
	void Update () {
		if (healthBar) {
			healthBar.transform.localScale = new Vector3(originalScale.x * health, originalScale.y, originalScale.z);
		}
	}

	void OnTriggerEnter(Collider other) {
		//Debug.Log("OnTrigger");
		HitBy(other.gameObject);
	}

	void OnCollisionEnter(Collision other) {
		//Debug.Log("OnCollision");
		HitBy(other.gameObject);
	}

	void OnControllerColliderHit(ControllerColliderHit other) {
		//Debug.Log("OnCollision");
		HitBy(other.gameObject);
	}

	void HitBy(GameObject other) {
		foreach(Threat threat in threats) {

			if (other.name == threat.gameObject.name || other.name == (threat.gameObject.name + "(Clone)")) {
				audiosource.PlayOneShot(threat.collisionSound);
				health -= threat.damage;
				if (threat.vanishOnImpact) {
					Destroy(other);	
				}
			}
			if (other.name == "Bullet" || other.name == "Bullet(Clone)" || other.name == "Sphere" || other.name == "Sphere(Clone)") {
				health -= 0.1f;
				Destroy(other);
			}

		}
		if (health > 1) {
			health = 1;
		}
		if (health <= 0) {
			if (deathScene.Equals("")) {
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			} else {
				SceneManager.LoadScene(deathScene);
			}
		}
	}
}
