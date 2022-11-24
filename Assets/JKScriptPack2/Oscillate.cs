using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JKScriptPack2
{

    /// ------------------------------------------
    /// <summary>
    /// 
    ///     Makes an object move back-and-forth 
    ///     repeatedly.
    ///     
    ///     Attach this script to the gameobject
    ///     that needs to move.
    ///     
    /// </summary>
    /// ------------------------------------------
    public class Oscillate : MonoBehaviour
    {

        [Tooltip("How far to move from the start point")]
        public Vector3 Distance = new Vector3(0, 1, 0);

        [Tooltip("Time period (in seconds) for movement.")]
        public float TransitTime = 1;

        [Tooltip("Accelerate and decelerate?")]
        public bool Smooth = true;

        private Vector3 _origin;
        private float _elapsedTime;
        private bool _reverse;

        void Start()
        {
            _origin = this.transform.localPosition;
            _reverse = false;
        }

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
