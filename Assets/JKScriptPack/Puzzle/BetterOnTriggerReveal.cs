using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace JKScriptPack1
{

	/// ------------------------------------------
	/// <summary>
	/// 
	///     Reveals an object (usually a UI object,
	///     like instructions for the player) 
	///     whenever the first person controller
	///     enters a trigger zone.
	///     
	///     Attach this script to the trigger zone 
	///     (GameObject with Collider.isTrigger
	///     enabled) -- usually an invisible cube.
	///     
	///     On exit, another object can be 
	///     activated, like a door trigger
	///     or a trigger for another puzzle clue.
	///     
	/// </summary>
	/// <remarks>
	/// 
	///     Formerly called BetterOnTriggerReveal.
	///     
	///     Updated to work with new Input System
	///     (as well as old Input Manager).
	/// 
	/// </remarks>
	/// ------------------------------------------
	public class BetterOnTriggerReveal : MonoBehaviour
	{

        [Header("When Triggered")]

        [Tooltip("Reveal this object (e.g. chicken appears)")]
        public GameObject revealObject;

        [Tooltip("Hide this object (e.g. egg disappears)")]
        public GameObject hideObject;

        public AudioClip playSound;

        [Tooltip("How long to reveal (in seconds).  If 0, there is no timeout.")]
        public float timeout = 0.0f;

        [Header("After Un-Reveal")]

        [Tooltip("Enable this object (e.g. next clue)")]
        public GameObject onExitEnable;

        [Tooltip("Disable this object (e.g. barrier blocking door)")]
        public GameObject onExitDisable;

        public AudioClip exitSound;



        public KeyCode keypressNeeded = KeyCode.None;
		public bool hideOnExit = true;
		public bool permanent = false;

		private bool withinTriggerZone = false;
		private bool activated;
		private float countdown;
		private AudioSource audiosource;

		void Start()
		{

			// Initialise objects
			if (revealObject) revealObject.SetActive(false);
			if (hideObject) hideObject.SetActive(true);

			withinTriggerZone = false;
			activated = false;

			// Initialise audio
			audiosource = gameObject.AddComponent<AudioSource>();
		}

		void OnTriggerEnter(Collider other)
		{
			withinTriggerZone = true;
			if (keypressNeeded == KeyCode.None)
			{
				Activate();
			}
		}
		void Activate()
		{
			if (!activated)
			{
				activated = true;
				if (revealObject) revealObject.SetActive(true);
				if (hideObject) hideObject.SetActive(false);
				audiosource.PlayOneShot(playSound);
				countdown = timeout;
			}
		}

		void OnTriggerExit(Collider other)
		{
			withinTriggerZone = false;
			if (hideOnExit)
			{
				Deactivate();
			}
		}
		void Deactivate()
		{
			if (activated && !permanent)
			{
				activated = false;
				if (revealObject) revealObject.SetActive(false);
				if (hideObject) hideObject.SetActive(true);
			}
			audiosource.PlayOneShot(exitSound);
			if (onExitEnable) onExitEnable.SetActive(true);
			if (onExitDisable) onExitDisable.SetActive(false);
			countdown = 0.0f;
		}

		void Update()
		{
			if (withinTriggerZone && Input.GetKeyDown(keypressNeeded))
			{
				if (activated)
				{
					Deactivate();
				}
				else
				{
					Activate();
				}
			}
			if (activated && countdown > 0)
			{
				countdown -= Time.deltaTime;
				if (countdown <= 0)
				{
					Deactivate();
				}
			}
		}

        /// <summary>
        /// Check if a key has been pressed.
        /// </summary>
        /// <param name="k">Key on keyboard.</param>
        /// <returns>True if pressed; false if not.<returns>
#if ENABLE_INPUT_SYSTEM
        private bool IsKeyPressed(Key k)
        {
            // Check before lookup; current[Key.None] would cause an error
            if (k != Key.None) {     
                return Keyboard.current[k].wasPressedThisFrame;
            }
            return false;
        }
#else
        private bool IsKeyPressed(KeyCode k)
        {
            return Input.GetKeyDown(k);
        }
#endif

        /// <summary>
        /// Check if a key is set to "None".
        /// </summary>
        /// <param name="k">Key on keyboard.</param>
        /// <returns>True if it matches "None"; otherwise false.<returns>
#if ENABLE_INPUT_SYSTEM
        private bool IsNone(Key k)
        {
            return (k == Key.None);
        }
#else
        private bool IsNone(KeyCode k)
        {
            return (k == KeyCode.None);
        }
#endif

    }
}
