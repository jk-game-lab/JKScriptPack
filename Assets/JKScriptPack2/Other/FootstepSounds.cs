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
    /// </summary>
    /// ------------------------------------------
    public class FootstepSounds : MonoBehaviour
    {
        [Header("Audio")]

        public AudioClip LeftFootSound;
        public AudioClip RightFootSound;

        [Header("Timing")]

        [Tooltip("Greater period means slower steps")]
        public float Period = 1.4f;

        private CharacterController controller;
        private StarterAssets.FirstPersonController fpc;
        private AudioSource audiosource;
        private float timeSinceLastFootstep;
        private bool isLeftFoot = true;

        void Start()
        {

            // Get FPC info
            controller = GetComponent<CharacterController>();
            fpc = GetComponent<StarterAssets.FirstPersonController>();

            // Initialise audio
            audiosource = gameObject.AddComponent<AudioSource>();

        }

        void Update()
        {

            // What is the ground underneath our feet?

            // Is it time for a footstep?
            float speed = controller.velocity.magnitude;
            if (speed > 0 && speed < 20)
            {
                float gapBetweenSteps = Period / speed;

                // Check time elapsed
                timeSinceLastFootstep += Time.deltaTime;
                if (timeSinceLastFootstep > gapBetweenSteps)
                {
                    timeSinceLastFootstep = 0;
                    Step();
                }
            }
            else
            {
                timeSinceLastFootstep = 0;
            }

        }

        private void Step()
        {
            if (isLeftFoot)
            {
                if (LeftFootSound) audiosource.PlayOneShot(LeftFootSound);
            }
            else
            {
                if (RightFootSound) audiosource.PlayOneShot(RightFootSound);
            }
            isLeftFoot = !isLeftFoot;
        }

    }

}

