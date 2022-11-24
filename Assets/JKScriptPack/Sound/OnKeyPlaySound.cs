/*
 *	OnKeyPlaySound.cs
 *
 *	Plays a sound if when a key is pressed.
 *
 *	Apply this script to any gameobject.
 *
 *	v1.38 -- added to JKScriptPack.
 *
 */

using UnityEngine;
using System.Collections;

public class OnKeyPlaySound : MonoBehaviour {

	public KeyCode key = KeyCode.None;
	public AudioClip sound = null;
	public float volume = 1.0f;
	public bool loop = false;
	public AudioClip soundWhenReleased = null;

	private AudioSource audiosource;
	private bool pressed;

	void Start () {
		audiosource = gameObject.AddComponent<AudioSource>();
		pressed = false;
	}

	void Update() {
		if (!pressed && Input.GetKeyDown (key)) {
			pressed = true;
			audiosource.volume = volume;
			if (loop || soundWhenReleased) {
				audiosource.clip = sound;
				audiosource.loop = loop;
				audiosource.Play ();
			} else {
				audiosource.PlayOneShot (sound);
			}
		}
		if (pressed && Input.GetKeyUp (key)) {
			pressed = false;
			if (loop || soundWhenReleased) {
				audiosource.Stop();
			}
			if (soundWhenReleased) {
				audiosource.clip = soundWhenReleased;
				audiosource.loop = false;
				audiosource.Play();
			}
		}
	}

}

