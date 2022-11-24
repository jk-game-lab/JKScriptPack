using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JKScriptPack3
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
        public Vector3 Distance = new Vector3(0, 1, 0);

        [Tooltip("Time for transit (in seconds).")]
        public float TransitTime = 1;

        [Tooltip("Accelerate and decelerate?")]
        public bool Smooth = true;

        private Vector3 _origin;
        private float _elapsedTime;
        private bool _reverse;

        /// <summary>
        /// At game start, record starting position.
        /// </summary>
        void Start()
        {
            _origin = this.transform.localPosition;
            _reverse = false;
        }

        /// <summary>
        /// Runs every frame.
        /// </summary>
        void Update()
        {

            if (TransitTime >= 0)
            {

                // Where is the target?
                Vector3 target = _origin + Distance;

                // Are we there yet?
                _elapsedTime += Time.deltaTime;
                if (_elapsedTime >= TransitTime)
                {
                    _elapsedTime = 0;
                    _reverse = !_reverse;
                }

                // Move the object
                float proportion = _elapsedTime / TransitTime;
                if (Smooth)
                {
                    proportion = 0.5f - Mathf.Cos(proportion * 180.0f * Mathf.Deg2Rad) / 2;
                }
                if (_reverse)
                {
                    this.transform.localPosition = Vector3.Lerp(target, _origin, proportion);
                }
                else
                {
                    this.transform.localPosition = Vector3.Lerp(_origin, target, proportion);
                }

            }

        }



    }

}
