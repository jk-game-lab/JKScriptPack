using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JKScriptPack2
{
    /// ------------------------------------------
    /// <summary>
    /// 
    ///     Orients an object to face toward the 
    ///     camera at all times.
    ///     
    ///     Attach this script to the gameobject
    ///     that needs to face the camera.
    ///     
    /// </summary>
    /// ------------------------------------------
    public class Billboard : MonoBehaviour
    {

        [Tooltip("Which camera to face")]
        public Camera CameraToFace;

        [Tooltip("Flip object to face the opposite direction")]
        public bool FlipZDirection = false;

        [Tooltip("Track the camera vertically")]
        public bool TrackVertically = false;

        void Update()
        {
            if (CameraToFace)
            {

                // Rotate the billboard object to face the camera
                if (FlipZDirection)
                {
                    this.transform.rotation = Quaternion.LookRotation(transform.position - CameraToFace.gameObject.transform.position);
                }
                else
                {
                    this.transform.LookAt(CameraToFace.gameObject.transform);
                }

                // Remove vertical component
                if (!TrackVertically)
                {
                    Vector3 eulerAngles = this.transform.eulerAngles;
                    eulerAngles.x = 0;
                    this.transform.eulerAngles = eulerAngles;
                }

            }
        }

    }

}
