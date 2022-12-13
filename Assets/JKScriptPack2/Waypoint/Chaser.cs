using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JKScriptPack2
{
    /// ------------------------------------------
    /// <summary>
    /// 
    ///     Chases a GameObject.
    ///     
    ///     Attach this to the chaser (which must 
    ///     already be using Patrol) and set the
    ///     victim to be the first person controller.
    ///     
    /// </summary>
    /// ------------------------------------------
    [RequireComponent(typeof(JKScriptPack2.Patrol))]
    public class Chaser : MonoBehaviour
    {

        [Header("Chaser")]

        [Tooltip("Detection range")]
        public float Range = 10.0f;

        [Tooltip("Detection field of view (in degrees)")]
        public float Angle = 90;

        [Header("Victim")]

        [Tooltip("Which object is being chased? (Typically first person controller)")]
        public GameObject Victim;

        private UnityEngine.AI.NavMeshAgent Agent;
        private JKScriptPack2.Patrol PatrolScript;

        void Start()
        {
            Agent = new UnityEngine.AI.NavMeshAgent();
            // Set the initial location to current location;
            // set destination to victim;
            // if lost sight, switch back.

            PatrolScript = GetComponent<JKScriptPack2.Patrol>();
        }

        void Update()
        {
            // Where am I?
            Vector3 thisPosition = this.transform.position;
            Vector3 thisHeading = this.transform.forward;

            // Can I see the target?
            if (Victim)
            {

                // Work out where the target is
                Vector3 victimPosition = Victim.transform.position;
                Vector3 victimHeading = victimPosition - thisPosition;

                // How far apart are we?
                float distance = Vector3.Distance(thisPosition, victimPosition);
                float angle = Vector3.Angle(thisHeading, victimHeading);

                // Is the target within range?
                // Does the ray collide with anything along the way ?
                if (distance <= Range && angle <= (Angle / 2)
                    && !Physics.Raycast(thisPosition, victimHeading, distance - 1.5f))
                {


// Note: change chaser so that it disable patrol if it exists, stores current location and chases;
// if released from chase, navigate back to origin.  Then re-enable patrol/
// THis way the chaser can act without patrol if needed.
//
// Similarly, should the weepingangel script disable the navmesh to freeze the player, regardless of whether
// chaser and/or patrol are being used?

                    //                    PatrolScript.AddWaypoint();

                }
                else
                {

                    // Reset

                }

            }

        }





        /*
         * 
         * Break into three parts:
         *  Chase script -- makes something with a NavMesh chase a player (by adding an extra destination to the navmesh temporarily)
         *  WeepingAngel -- detects when the player sees it, and disables the Navmesh agent (freezing the enemy)
         *  Detect when caught script
         * 
         * 
         */



    }
}
