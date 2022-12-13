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
    ///     Attach this to the chaser and set the
    ///     victim to be the first person controller.
    ///     
    /// </summary>
    /// ------------------------------------------
    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    public class Chaser : MonoBehaviour
    {

        [Header("Chaser")]

        [Tooltip("Detection range")]
        public float Range = 10.0f;

        [Tooltip("Detection field of view (in degrees)")]
        public float Angle = 90;

        [Header("Victim")]

        [Tooltip("Which object is being chased? (Typically this would be the first person controller)")]
        public GameObject Victim;

        private UnityEngine.AI.NavMeshAgent agent;
        private JKScriptPack2.Patrol patrol;
        private Transform chaseStart;
        private bool isChasing;
  
        void Start()
        {
            agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
            isChasing = false;
        }

        void Update()
        {
            bool isSeen = CanSeeVictim();
            if (isChasing)
            {
                if (isSeen)
                {
                    Chase();
                }
                else
                {
                    StopChasing();
                }
            }
            else
            {
                if (isSeen)
                {
                    StartChasing();
                }
            }
        }

        private bool CanSeeVictim()
        {
            // Where am I?
            Vector3 thisPosition = this.transform.position;
            Vector3 thisHeading = this.transform.forward;

            // Can I see the victim?
            if (Victim)
            {

                // Work out where the victim is
                Vector3 victimPosition = Victim.transform.position;
                Vector3 victimHeading = victimPosition - thisPosition;

                // How far apart are we?
                float distance = Vector3.Distance(thisPosition, victimPosition);
                float angle = Vector3.Angle(thisHeading, victimHeading);

                // Is the victim within range?
                // Does the ray collide with anything along the way ?
                if (distance <= Range && angle <= (Angle / 2)
                    && !Physics.Raycast(thisPosition, victimHeading, distance - 1.5f))
                {
                    return true;
                }

            }

            return false;
        }

        private void StartChasing()
        {
            isChasing = true;

            // Record current position
            chaseStart = this.transform;

            // if Patrol script is being used, disable it
            patrol = this.GetComponent<JKScriptPack2.Patrol>();
            if (patrol)
            {
                patrol.enabled = false;
            }

        }

        private void Chase()
        {
            agent.destination = Victim.transform.position;
        }

        private void StopChasing()
        {
            isChasing = false;

            // Send back to pre-chase position
            agent.destination = chaseStart.position;

            // if Patrol script is being used, re-enable it
            if (patrol)
            {
                patrol.enabled = true;
            }

        }

    }
}
