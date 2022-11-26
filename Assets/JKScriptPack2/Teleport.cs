using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JKScriptPack2
{

    /// ------------------------------------------
    /// <summary>
    /// 
    ///     Makes objects jump to a new location.
    ///     
    ///     Attach this script to a gameobject 
    ///     that has a collider.  Anything that 
    ///     hits this collider will be teleported 
    ///     to the destination.
    ///     
    ///     Set that destination using another
    ///     gameobject (preferably an empty one).
    ///     
    /// </summary>
    /// ------------------------------------------
    public class Teleport : MonoBehaviour
    {

        [Tooltip("GameObject located at the destination (best using an empty gameobject)")]
        public GameObject Destination;

        void OnTriggerEnter(Collider other)
        {
            Transport(other.gameObject);
        }

        void OnCollisionEnter(Collision other)
        {
            Transport(other.gameObject);
        }

        /// <summary>
        /// Teleport the specified object to the desination.
        /// </summary>
        /// <param name="choice">Numbered camera in the list.</param>
        private void Transport(GameObject teleportee)
        {
            if (Destination)
            {
                teleportee.transform.position = Destination.transform.position;
                Physics.SyncTransforms();
            }
        }

    }

}
