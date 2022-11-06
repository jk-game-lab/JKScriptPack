using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace JKScriptPack
{

    /// <summary>
    /// Animates a sliding door (or a pair of sliding doors).
    /// Apply this script to a trigger zone.
    /// This will make a door slide open when the character
    /// enters the zone. If you have a pair of doors, the
    /// second door will move in the opposite direction to 
    /// the first.
    /// </summary>
    /// <remarks>
    /// 2022-11-06: Added to JKScriptPack
    /// </remarks>
    public class SlidingDoor : MonoBehaviour
    {
        [Tooltip("Door object")]
        public GameObject door = null;

        [Tooltip("How far the door should move when opening")]
        public Vector3 slide = new Vector3(-1.0f, 0, 0);

        [Tooltip("Speed of opening in m/s")]
        public float speed = 1.5f;

        [Tooltip("Is the door open?")]
        public bool isOpen = false;

        [Tooltip("Once opened, should the door stay open?")]
        public bool keepOpen = false;

        public GameObject secondDoor = null;

        private bool wasOpen;

        private bool triggered = false;
        public KeyCode keyboard = KeyCode.None;

        public AudioClip openingSound = null;
        public AudioClip closingSound = null;
        //public float volume = 1.0f;
        private AudioSource audiosource;

        private Vector3 doorOrigin;
        private Vector3 doorDestination;
        private Vector3 secondDoorOrigin;
        private Vector3 secondDoorDestination;
        private Vector3 pointB;
        private float travel;   // varies between 0 and 1

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
            travel = isOpen ? 1 : 0;
            wasOpen = isOpen;

        }

        void OnTriggerEnter(Collider other)
        {
            triggered = true;
            if (keyboard == KeyCode.None)
            {
                isOpen = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            triggered = false;
            isOpen = false;
        }

        void Update()
        {

            // Check for a keypress
            if (triggered && Input.GetKeyDown(keyboard))
            {
                isOpen = !isOpen;
            }

            // Override open state if keeping open
            if (keepOpen && wasOpen)
            {
                isOpen = true;
            }

            // Check if the open state has changed
            if (isOpen && !wasOpen)
            {
                //audiosource.volume = volume;
                audiosource.PlayOneShot(openingSound);
            }
            else if (!isOpen && wasOpen)
            {
                //audiosource.volume = volume;
                audiosource.PlayOneShot(closingSound);
            }
            wasOpen = isOpen;

            // Work out where the door(s) should be
            if (isOpen && travel < 1)
            {
                travel += speed * Time.deltaTime;
                if (travel > 1) travel = 1;
            }
            else if (!isOpen && travel > 0)
            {
                travel -= speed * Time.deltaTime;
                if (travel < 0) travel = 0;
            }
            if (door) door.transform.position = Vector3.Lerp(doorOrigin, doorDestination, travel);
            if (secondDoor) secondDoor.transform.position = Vector3.Lerp(secondDoorOrigin, secondDoorDestination, travel);

        }

    }
}