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
	///     Makes a door (or pair of doors) slide 
	///     open when someone enters a trigger zone.
	///     
	///     Attach this script to the trigger zone 
	///     (GameObject with Collider.isTrigger
	///     enabled) -- usually an invisible cube.
	///     
	///     When the first-person controller (or
	///     any rigidbody) enters the zone, the
	///     door(s) will slide open.
	///     
	///     If you have a pair of doors, the 
	///     second door will open in the opposite
	///     direction to the first.
	///     
	/// </summary>
	/// <remarks>
	/// 
	///     Updated to work with new Input System
	///     (as well as old Input Manager).
	/// 
	/// </remarks>
	/// ------------------------------------------
	public class DoorSlide : MonoBehaviour
	{

		[Header("Door(s)")]

		public GameObject door = null;
		public GameObject secondDoor = null;

		[Header("Movement")]

		public Vector3 slide = new Vector3(-1.0f, 0, 0);
		public float speed = 3.0f;
#if ENABLE_INPUT_SYSTEM
        public Key keyboard = Key.None;
#else
        public KeyCode keyboard = KeyCode.None;
#endif
		public bool open = false;
		public bool keepOpen = false;

		[Header("Sound Effects")]

		public AudioClip openingSound = null;
		public AudioClip closingSound = null;

		private Vector3 doorOrigin;
		private Vector3 doorDestination;
		private Vector3 secondDoorOrigin;
		private Vector3 secondDoorDestination;
		private Vector3 pointB;
		private float travel;   // varies between 0 and 1
		private bool wasOpen;
		private bool triggered = false;
		private AudioSource audiosource;
		//public float volume = 1.0f;

		void Start()
		{

			// Record the original & destination door positions
			if (door)
			{
				doorOrigin = door.transform.position;
				doorDestination = door.transform.TransformPoint(slide);
			}
			if (secondDoor)
			{
				secondDoorOrigin = secondDoor.transform.position;
				secondDoorDestination = secondDoor.transform.TransformPoint(-slide);
			}

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
			wasOpen = open;

		}

        void OnTriggerEnter(Collider other)
        {
            triggered = true;
            if (IsNone(keyboard))
            {
                open = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            triggered = false;
            open = false;
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
            }
            else if (!open && wasOpen)
            {
                //audiosource.volume = volume;
                audiosource.PlayOneShot(closingSound);
            }
            wasOpen = open;

            // Work out where the door(s) should be
            if (open && travel < 1)
            {
                travel += speed * Time.deltaTime;
                if (travel > 1) travel = 1;
            }
            else if (!open && travel > 0)
            {
                travel -= speed * Time.deltaTime;
                if (travel < 0) travel = 0;
            }
            if (door)
            {
                door.transform.position = Vector3.Lerp(doorOrigin, doorDestination, travel);
            }
            if (secondDoor)
            {
                secondDoor.transform.position = Vector3.Lerp(secondDoorOrigin, secondDoorDestination, travel);
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
