using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace JKScriptPack
{

    /// <summary>
    /// Orients an object to face toward the 
    /// camera at all times.
    /// Attach this script to the billboard
    /// object.
    /// </summary>
    /// <remarks>
    /// 2022-10-24: Added to JKScriptPack
    /// </remarks>
    public class Billboard : MonoBehaviour
    {

        [Tooltip("Which camera to face")]
        public Camera cameraToFace;

        [Tooltip("Flip object to face the opposite direction")]
        public bool flipZDirection = false;

        [Tooltip("Track the camera vertically")]
        public bool trackVertically = false;

        /// <summary>
        /// Every frame, update this object's position.
        /// </summary>
        void Update()
        {
            if (cameraToFace)
            {

                // Rotate the billboard object to face the camera
                if (flipZDirection)
                {
                    this.transform.rotation = Quaternion.LookRotation(transform.position - cameraToFace.gameObject.transform.position);
                }
                else
                {
                    this.transform.LookAt(cameraToFace.gameObject.transform);
                }

                // Remove vertical component
                if (!trackVertically)
                {
                    Vector3 eulerAngles = this.transform.eulerAngles;
                    eulerAngles.x = 0;
                    this.transform.eulerAngles = eulerAngles;
                }

            }
        }

    }

}
