using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JKScriptPack2
{

    /// ------------------------------------------
    /// <summary>
    /// 
    ///     When the first person is sprinting,
    ///     disable or enable gameobjects.
    ///     
    ///     Attach this script to the first person 
    ///     controller.
    ///     
    /// </summary>
    /// ------------------------------------------
    public class SprintDetector : MonoBehaviour
    {

        [Header("If sprinting then ...")]

        [Tooltip("Reveal this object (e.g. speed alert)")]
        public GameObject revealObject;

        [Tooltip("Hide this object (e.g. light source)")]
        public GameObject hideObject;

        private CharacterController controller;
        private StarterAssets.FirstPersonController fpc;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
            fpc = GetComponent<StarterAssets.FirstPersonController>();
        }

        void Update()
        {

            // Sometimes when walking, the character exceeds MoveSpeed;
            // therefore, we detect sprinting as faster than halfway between
            // MoveSpeed and SprintSpeed.
            float speedThreshold = (fpc.MoveSpeed + fpc.SprintSpeed) / 2;

            // Are we sprinting?
            if (controller.velocity.magnitude > speedThreshold)
            {
                if (revealObject) revealObject.SetActive(true);
                if (hideObject) hideObject.SetActive(false);
            }
            else
            {
                if (revealObject) revealObject.SetActive(false);
                if (hideObject) hideObject.SetActive(true);
            }
        }

    }

}
