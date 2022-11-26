using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JKScriptPack2
{
    /// ------------------------------------------
    /// <summary>
    /// 
    ///     If this gameobject can see a target
    ///     gameobject then carry out an action.
    ///     
    /// </summary>
    /// ------------------------------------------
    public class ActOnSighting : MonoBehaviour
    {

        [Header("Detection")]

        [Tooltip("Which object are we looking for?")]
        public GameObject Target;

        [Tooltip("Detection range")]
        public float Range = 10.0f;

        [Tooltip("Detection field of view (in degrees)")]
        public float Angle = 90;

        [Header("Action")]

        public GameObject EnableObject;
        public string LoadScene;

        void Start()
        {
            if (EnableObject) EnableObject.SetActive(false);
        }

        void Update()
        {
            // Where am I?
            Vector3 thisPosition = this.transform.position;
            Vector3 thisHeading = this.transform.forward;

            // Can I see the target?
            if (Target)
            {

                // Work out where the target is
                Vector3 targetPosition = Target.transform.position;
                Vector3 targetHeading = targetPosition - thisPosition;

                // How far apart are we?
                float distance = Vector3.Distance(thisPosition, targetPosition);
                float angle = Vector3.Angle(thisHeading, targetHeading);

                // Is the target within range?
                // Does the ray collide with anything along the way ?
                if (distance <= Range && angle <= (Angle / 2)
                    && !Physics.Raycast(thisPosition, targetHeading, distance - 1.5f))
                {

                    // Do the action!
                    if (EnableObject) EnableObject.SetActive(true);
                    if (LoadScene != "")
                    {
                        SceneManager.LoadScene(LoadScene);
                    }

                }
                else
                {

                    // Reset
                    if (EnableObject) EnableObject.SetActive(false);

                }

            }

        }

    }

}
