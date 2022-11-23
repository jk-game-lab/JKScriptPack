using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace JKScriptPack
{

    /// <summary>
    /// Animates a sliding door when a character enters a tigger zone.
    /// Apply this script to the trigger zone.
    /// </summary>
    /// <remarks>
    /// 2022-11-06: Added to JKScriptPack
    /// </remarks>
    public class SlidingDoor : MonoBehaviour
    {

        [Tooltip("Attach the door object here")]
        public GameObject Door = null;

        [Header("Movement")]

        [Tooltip("How far to move when opening")]
        public Vector3 SlideDistance = new Vector3(-1.0f, 0, 0);

        [Tooltip("Speed of movement in m/s")]
        [Range(0, 10)]
        public float Speed = 1.5f;

        [Tooltip("The door's current state")]
        public enum DoorState
        {
            Locked,
            Closed,
            Open
        }
        [SerializeField]
        private DoorState _state = DoorState.Closed;
        public DoorState State
        {
            get
            {
                return _state;
            }
            set
            {
                if (Application.isPlaying) {

                    // Check that change of state is permitted
                    switch (value)
                    {
                        case DoorState.Locked:
                            if (_state == DoorState.Open || _state == DoorState.Closed)
                            {
                                _state = DoorState.Locked;
                            }
                            break;
                        case DoorState.Closed:
                            if (_state == DoorState.Open)
                            {
                                _state = DoorState.Closed;
                            }
                            break;
                        case DoorState.Open:
                            if (_state == DoorState.Closed)
                            {
                                _state = DoorState.Open;
                            }
                            break;
                        default:
                            break;
                    }

                }
                else // editor mode: allow setting to directly changed
                {
                    _state = value;
                }

            }
        }

        [Tooltip("Automatically close door")]
        public bool AutoClose = true;

        [Header("Sound effects")]

        [Tooltip("Play this sound when closing the door")]
        public AudioClip Closing = null;
        [Tooltip("Play this sound when opening the door")]
        public AudioClip Opening = null;

        AudioSource _audiosource;




        private bool _wasOpen;
//        private bool _triggered = false;

        //public float volume = 1.0f;

        private Vector3 _origin;
        private Vector3 _destination;
        private Vector3 pointB;
        private float _travel;   // varies between 0 and 1

        void Start()
        {

            if (Door)
            { 
                // Record the original & destination door positions
                _origin = Door.transform.position;
                _destination = Door.transform.TransformPoint(SlideDistance);

                // Set up audio to come from the door (or, failing that, the trigger zone)
                //_audiosource = Door.AddComponent<AudioSource>();

            }
            else
            {
                // Set up audio to come from the trigger zone
                //_audiosource = gameObject.AddComponent<AudioSource>();
            }

            // initialise
            switch (_state)
            {
            case DoorState.Open:
                _travel = 1;
                break;
            default:
                _travel = 0;
                break;
            }

        }

        void OnTriggerEnter(Collider other)
        {
            this.Open();
        }

        void OnTriggerExit(Collider other)
        {
            this.Close();
        }

        public void Open() SetState(DoorState.Open);
        public void Close() SetState(DoorState.Closed);
        public void Lock() SetState(DoorState.Locked);

        public void Unlock()
        {
            if (_state == DoorState.Locked)
            {
                _state = DoorState.Closed;
            }
        }

        public void Wedge()
        {
            if (_state == DoorState.Open || _state == DoorState.Closed)
            {
                _state = DoorState.WedgedOpen;
            }
        }

        public void Lock()
        {
            if (_state == DoorState.Open || _state == DoorState.Closed)
            {
                _state = DoorState.Locked;
            }
        }

        /*

            void OnTriggerEnter(Collider other)
            {
                _triggered = true;
                if (keyboard == KeyCode.None)
                {
                    IsOpen = true;
                }
            }

            void OnTriggerExit(Collider other)
            {
                _triggered = false;
                IsOpen = false;
            }

                void Update()
                {

                    // Check for a keypress
                    if (_triggered && Input.GetKeyDown(keyboard))
                    {
                        IsOpen = !IsOpen;
                    }

                    // Override open state if keeping open
                    if (KeepOpen && _wasOpen)
                    {
                        IsOpen = true;
                    }

                    // Check if the open state has changed
                    if (IsOpen && !_wasOpen)
                    {
                        //audiosource.volume = volume;
                        _audiosource.PlayOneShot(OpeningSound);
                    }
                    else if (!IsOpen && _wasOpen)
                    {
                        //audiosource.volume = volume;
                        _audiosource.PlayOneShot(ClosingSound);
                    }
                    _wasOpen = IsOpen;

                    // Work out where the door(s) should be
                    if (IsOpen && travel < 1)
                    {
                        travel += Speed * Time.deltaTime;
                        if (travel > 1) travel = 1;
                    }
                    else if (!IsOpen && travel > 0)
                    {
                        travel -= Speed * Time.deltaTime;
                        if (travel < 0) travel = 0;
                    }
                    if (Door) Door.transform.position = Vector3.Lerp(doorOrigin, doorDestination, travel);
                    if (secondDoor) secondDoor.transform.position = Vector3.Lerp(secondDoorOrigin, secondDoorDestination, travel);

                }

                */

    }

}