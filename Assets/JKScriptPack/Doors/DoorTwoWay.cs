/*
 * 	DoorTwoWay.cs
 * 
 * 	Makes a door swing open when someone enters a trigger zone.
 * 
 * 	Attach this script to a trigger zone (any GameObject with 
 * 	Collider.isTrigger enabled; usually an invisible cube).
 * 
 *	v1.21 -- added to JKScriptPack.  Quick-and-nasty implementation that takes
 *			 no account of the door closing before opening the other way.
 *
 */

using UnityEngine;
using System.Collections;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace JKScriptPack1
{

	/// ------------------------------------------
	/// <summary>
	/// 
	///     Makes a door swing open when someone
	///     enters a trigger zone.  Opens in BOTH
	///     directions.
	///     
	///     Attach this script to the trigger zone 
	///     (GameObject with Collider.isTrigger
	///     enabled) -- usually an invisible cube.
	///     
	///     When the first-person controller (or
	///     any rigidbody) enters the zone, the
	///     door will swing open.
	///     
	/// </summary>
	/// <remarks>
	/// 
	///		This was an old, quick-and-nasty 
	///		implementation that takes no account 
	///		of the door closing before opening 
	///		the other way.
	/// 
	///		Updated to work with new Input System
	///     (as well as old Input Manager).
	/// 
	/// </remarks>
	/// ------------------------------------------
	public class DoorTwoWay : MonoBehaviour
	{

		public bool open = false;
		public bool keepOpen = false;
		private bool wasOpen;

		public GameObject door;
		public float swingAngle = -120; // in degrees
		public float swingSpeed = 1.5f;
		public Vector3 swingAxis = new Vector3(0, 1, 0);
		public Vector3 doorHinge = new Vector3(-0.5f, 0, 0);

		private float direction = 1.0f;

		public AudioClip openingSound;
		public AudioClip closingSound;
		private AudioSource audiosource;

		private bool triggered = false;
#if ENABLE_INPUT_SYSTEM
        public Key keyboard = Key.None;
#else
        public KeyCode keyboard = KeyCode.None;
#endif
		public bool pressKeyToClose = false;


        //------------------------------------------------------------------------

        [System.Serializable]
		public class Handle : System.Object
		{

			public GameObject handleObject;
			public Vector3 rotation = new Vector3(0, 0, 45);
			public float turnSpeed = 3.0f;
			public float releaseSpeed = 2.0f;
			public bool turnWhenClosing = false;

			private AutoRotator autorotator;

			private float countdown;
			private enum HandleState { Idle, Turn, Unturn };
			private HandleState state = HandleState.Idle;

			public void Start()
			{
				state = HandleState.Turn;
				autorotator = new AutoRotator(handleObject, rotation, turnSpeed, false, true);
			}

			public void Update()
			{
				if (state == HandleState.Turn)
				{
					autorotator.Update();
					if (autorotator.Finished())
					{
						autorotator = new AutoRotator(handleObject, rotation, releaseSpeed, true, true);
						state = HandleState.Unturn;
					}
				}
				else if (state == HandleState.Unturn)
				{
					autorotator.Update();
					if (autorotator.Finished())
					{
						autorotator = null;
						state = HandleState.Idle;
					}
				}
			}

		}
		public Handle handle;

		private Vector3 pivotAbsolute;
		private float travel;           // a proportion between 0 and 1
		private float prevTravel;

		void Start()
		{

			// Work out the hinge position
			if (door) pivotAbsolute = door.transform.TransformPoint(doorHinge);

			// Set up audio
			if (door)
			{
				audiosource = door.AddComponent<AudioSource>();
			}
			else
			{
				audiosource = gameObject.AddComponent<AudioSource>();
			}

			// initialise
			travel = open ? 1 : 0;
			prevTravel = travel;
			wasOpen = open;

		}

		void OnTriggerEnter(Collider other)
		{
			triggered = true;
			if (IsNone(keyboard))
			{
				open = true;
			}

			// Work out which way we're travelling
			Vector3 difference = this.gameObject.transform.position - other.gameObject.transform.position;
			float angle = Vector3.Angle(this.gameObject.transform.forward, difference);
			if (angle > 90)
			{
				direction = -1;
			}
			else
			{
				direction = 1;
			}
			//Debug.Log("Vector: " + difference.ToString() + ", Angle: " + angle);
		}

		void OnTriggerExit(Collider other)
		{
			triggered = false;
			if (IsNone(keyboard) || !pressKeyToClose)
			{
				open = false;
			}
		}

		void Update()
		{

			// Check for a keypress
			if (triggered && IsKeyPressed(keyboard))
			{
				open = !open;
			}

			// Override open state if keeping open
			if (keepOpen && wasOpen)
			{
				open = true;
			}

			// Check if the open state has changed
			if (open && !wasOpen)
			{
				//audiosource.volume = volume;
				audiosource.PlayOneShot(openingSound);
				handle.Start();
			}
			else if (!open && wasOpen)
			{
				//audiosource.volume = volume;
				audiosource.PlayOneShot(closingSound);
			}
			wasOpen = open;

			// Work out where the door should be
			if (open && travel < 1)
			{
				travel += swingSpeed * Time.deltaTime;
				if (travel > 1) travel = 1;
			}
			else if (!open && travel > 0)
			{
				travel -= swingSpeed * Time.deltaTime;
				if (travel < 0)
				{
					travel = 0;
					if (handle.turnWhenClosing) handle.Start();
				}
			}
			if (door) door.transform.RotateAround(pivotAbsolute, swingAxis, (travel - prevTravel) * swingAngle * direction);
			prevTravel = travel;
			handle.Update();

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

        //------------------------------------------------------------------------

        private class AutoRotator
		{

			public GameObject thing;
			public Vector3 rotation;
			public float speed;
			public bool reverse = false;
			public bool natural = true;

			private float progress = 0;

			public AutoRotator(GameObject thing, Vector3 rotation, float speed, bool reverse, bool natural)
			{
				this.thing = thing;
				this.rotation = rotation;
				this.speed = speed;
				this.natural = natural;
				this.reverse = reverse;
			}

			public void Update()
			{
				if (progress < 1)
				{
					progress += Time.deltaTime * speed;
				}
				float proportion = progress;
				if (reverse) proportion = 1.0f - proportion;
				if (natural) proportion = 0.5f - Mathf.Cos(proportion * 180.0f * Mathf.Deg2Rad) / 2;
				Quaternion angleA = Quaternion.Euler(Vector3.zero);
				Quaternion angleB = Quaternion.Euler(rotation);
				if (thing) thing.transform.localRotation = Quaternion.Lerp(angleA, angleB, proportion);
			}

			public bool Finished()
			{
				return (progress >= 1);
			}

		}

	}
}
