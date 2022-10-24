using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JKScriptPack
{

    /// <summary>
    /// Makes an object move to-and-from another position.
    /// Attach this script to the object that will move.
    /// </summary>
    /// <remarks>
    /// 2022-10-25: Added to JKScriptPack
    /// </remarks>
    public class Oscillate : MonoBehaviour
    {

        [Tooltip("How far to move from the start point")]
        public Vector3 distance = new Vector3(0, 1, 0);

        [Tooltip("Time for transit (in seconds).")]
        public float transitTime = 1;

        [Tooltip("Accelerate and decelerate?")]
        public bool smooth = true;

        private Vector3 origin;
        private float elapsedTime;
        private bool reverse;

        /// <summary>
        /// At game start, record starting position.
        /// </summary>
        void Start()
        {
            origin = this.transform.localPosition;
            reverse = false;
        }

        /// <summary>
        /// Runs every frame.
        /// </summary>
        void Update()
        {

            if (transitTime >= 0)
            {

                // Where is the target?
                Vector3 target = origin + distance;

                // Are we there yet?
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= transitTime)
                {
                    elapsedTime = 0;
                    reverse = !reverse;
                }

                // Move the object
                float proportion = elapsedTime / transitTime;
                if (smooth)
                {
                    proportion = 0.5f - Mathf.Cos(proportion * 180.0f * Mathf.Deg2Rad) / 2;
                }
                if (reverse)
                {
                    this.transform.localPosition = Vector3.Lerp(target, origin, proportion);
                }
                else
                {
                    this.transform.localPosition = Vector3.Lerp(origin, target, proportion);
                }

            }

        }



    }

}
