using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JKScriptPack3
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
        public GameObject Destination;

        /// <summary>
        /// Detects an object colliding with this one.
        /// </summary>
        void OnTriggerEnter(Collider other)
        {
            if (Destination)
            {
                other.transform.position = Destination.transform.position;
            }
        }

    }

}
