using UnityEngine;
using UnityEngine.AI;
using System.Collections;

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
    [RequireComponent(typeof(NavMeshAgent))]
    public class Chaser : MonoBehaviour
    {

        [Header("Chaser")]

        [Tooltip("Detection range")]
        public float Range = 10.0f;

        [Tooltip("Detection field of view (in degrees)")]
        public float Angle = 120;

        [Header("Victim")]

        [Tooltip("Which object is being chased? (Typically this would be the first person controller)")]
        public GameObject Victim;

        private NavMeshAgent agent;
        private JKScriptPack2.Patrol patrol;
        private bool patrolOriginalState;
        private Transform chaseStart;
        private bool isChasing;
  
        void Start()
        {
            agent = this.GetComponent<NavMeshAgent>();
            isChasing = false;

            // Move this object & the victim to another layer, so they do not block raycasts
            SetLayerIncludingChildren(this.gameObject, 2);
            SetLayerIncludingChildren(Victim, 2);

        }

        private void SetLayerIncludingChildren(GameObject g, int layer)
        {
            if (g == null)
            {
                return;
            }
            g.layer = layer;
            foreach (Transform t in g.transform)
            {
                if (t != null)
                {
                    SetLayerIncludingChildren(t.gameObject, layer);
                }
            }
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

            // Draw the chase zone
            float halfangle = Angle / 2;
            Vector3 lastpos = Vector3.zero;
            for (float a = -halfangle; a <= halfangle; a += halfangle / 12)
            {
                Vector3 now = (Quaternion.AngleAxis(a, Vector3.up) * thisHeading) * Range;
                Debug.DrawLine(thisPosition + lastpos, thisPosition + now, Color.yellow);
                lastpos = now;
            }
            Debug.DrawLine(thisPosition + lastpos, thisPosition, Color.yellow);

            // Can I see the victim?
            if (Victim)
            {
                // Where is the victim?
                Vector3 victimPosition = Victim.transform.position;
                Vector3 victimHeading = victimPosition - thisPosition;

                // Check range
                float victimDistance = victimHeading.magnitude;
                float victimAngle = Vector3.Angle(thisHeading, victimHeading);
                if (victimDistance <= Range && victimAngle <= (Angle / 2))
                {

                    // Check whether the view is blocked
                    RaycastHit hitinfo;
                    bool viewBlocked = Physics.Raycast(thisPosition, victimHeading.normalized, out hitinfo, victimDistance);
                    if (!viewBlocked)
                    {

                        // Debug mode: draw ray to victim
                        Debug.DrawRay(thisPosition, victimHeading, Color.red);

                        // Victim has been seen
                        return true;
                    }

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
                patrolOriginalState = patrol.enabled;
                patrol.enabled = false;
            }

        }

        private void Chase()
        {
            // Head toward the victim
            agent.destination = Victim.transform.position;

            // Look toward the victim
            Vector3 thisPosition = this.transform.position;
            Vector3 victimPosition = Victim.transform.position;
            Vector3 victimHeading = victimPosition - thisPosition;
            float victimDistance = victimHeading.magnitude;
            float proportion = 1;
            if (Range > 0)
            {
                proportion -= victimDistance / Range;
            }
            Quaternion victimRotation = Quaternion.LookRotation(victimHeading);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, victimRotation, proportion);
        }

        private void StopChasing()
        {
            isChasing = false;

            // Send back to pre-chase position
            agent.destination = chaseStart.position;

            // if Patrol script is being used, re-enable it
            if (patrol)
            {
                patrol.enabled = patrolOriginalState;
            }

        }

    }
}
