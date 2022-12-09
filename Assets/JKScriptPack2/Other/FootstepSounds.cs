using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JKScriptPack2
{

    /// ------------------------------------------
    /// <summary>
    /// 
    ///     Plays footstep sounds when the first
    ///     person controller moves.
    ///     
    ///     Attach this script to the first person
    ///     controller.  If the floor object is 
    ///     tagged with 
    ///     
    /// </summary>
    /// ------------------------------------------
    public class FootstepSounds : MonoBehaviour
    {

        [Header("Floor detection")]

        [Tooltip("Play footsteps if the floor object has this tag")]
        public string Tag = "Untagged";

        [Header("Sounds")]

        public AudioClip LeftFootSound;
        public AudioClip RightFootSound;

        [Tooltip("Greater pace means faster steps")]
        public float Pace = 0.7f;

        public float Loudness = 1.0f;

        private CharacterController controller;
        private AudioSource audiosource;

        private float timeSinceLastFootstep;
        private bool isLeftFoot = true;

        void Start()
        {

            // Get FPC info
            controller = GetComponent<CharacterController>();

            // Initialise audio
            audiosource = gameObject.AddComponent<AudioSource>();
            audiosource.volume = Loudness;

        }

        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            // Are we walking on the tagged surface?
            if (hit.gameObject.tag == Tag)
            {

                // Are we moving?
                float speed = controller.velocity.magnitude;
                if (speed > 0 && Pace > 0)
                {

                    // Check time elapsed
                    timeSinceLastFootstep += Time.deltaTime;
                    float gapBetweenSteps = 1 / (speed * Pace);
                    if (timeSinceLastFootstep > gapBetweenSteps)
                    {

                        // Make a footstep
                        if (isLeftFoot)
                        {
                            if (LeftFootSound) audiosource.PlayOneShot(LeftFootSound);
                        }
                        else
                        {
                            if (RightFootSound) audiosource.PlayOneShot(RightFootSound);
                        }
                        isLeftFoot = !isLeftFoot;
                        timeSinceLastFootstep = 0;

                    }
                }
                else
                {
                    timeSinceLastFootstep = 0;
                }
            }


        }

    }

}

