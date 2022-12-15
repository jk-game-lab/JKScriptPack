using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace JKScriptPack2
{
    /// ------------------------------------------
    /// <summary>
    /// 
    ///     Makes a gameobject follow a series of 
    ///     waypoints within a NavMesh.
    ///     
    /// </summary>
    /// <remarks>
    /// 
    ///     Adapted from an example at
    ///     https://docs.unity3d.com/560/Documentation/Manual/nav-AgentPatrol.html
    /// 
    /// </remarks>
    /// ------------------------------------------
    [RequireComponent(typeof(NavMeshAgent))]
    public class Patrol : MonoBehaviour
    {

        public Transform[] Waypoints;

        private int Destination = 0;
        private NavMeshAgent Agent;

        void Start()
        {
            Agent = GetComponent<NavMeshAgent>();

            // Disabling auto-braking allows for continuous movement
            // between points (i.e. the agent doesn't slow down as it
            // approaches a destination point).
            Agent.autoBraking = false;

            GoToNextWaypoint();
        }

        private void GoToNextWaypoint()
        {
            // Returns if no points have been set up
            if (Waypoints.Length == 0)
            {
                return;
            }

            // Set the agent to go to the currently selected destination.
            Agent.destination = Waypoints[Destination].position;

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            Destination = (Destination + 1) % Waypoints.Length;

        }

        void Update()
        {
            // Debug mode only: draw a line to the destination
            Vector3 thisPosition = this.transform.position;
            Vector3 waypointPosition = Agent.destination;
            Debug.DrawRay(thisPosition, waypointPosition - thisPosition, Color.cyan);

            // Choose the next destination point when the agent gets
            // close to the current one.
            if (Agent.enabled && !Agent.pathPending && Agent.remainingDistance < 0.5f)
            {
                GoToNextWaypoint();
            }
        }

    }
}
