using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JKScriptPack
{

    /// <summary>
    /// Makes an object revolve around its pivot point.
    /// </summary>
    /// <remarks>
    /// 2022-10-14: Added to JKScriptPack
    /// </remarks>
    public class Revolve : MonoBehaviour
    {

        [Tooltip("Rotational speed, measured in revolutions per second.")]
        public float Speed = 1;

        [Tooltip("The axis that the object will rotate around.")]
        public Vector3 Axis = new Vector3(0, 1, 0);

        /// <summary>
        /// Runs every frame.
        /// </summary>
        void Update()
        {
            transform.Rotate(Axis, Speed * 360 * Time.deltaTime);
        }

    }

}
