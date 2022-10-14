using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JKScriptPack
{

    /// <summary>
    /// Makes any colliding object jump to a new location.
    /// </summary>
    /// <remarks>
    /// 2022-10-14: Added to JKScriptPack
    /// </remarks>
    public class Teleport : MonoBehaviour
    {

        [Tooltip("GameObject located at the destination (best using an empty gameobject)")]
        public GameObject destination;

        /// <summary>
        /// Detects an object colliding with this one.
        /// </summary>
        void OnTriggerEnter(Collider other)
        {
            if (destination)
            {
                other.transform.position = destination.transform.position;
            }
        }

    }

}
