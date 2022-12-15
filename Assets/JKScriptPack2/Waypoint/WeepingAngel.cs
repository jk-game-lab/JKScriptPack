using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace JKScriptPack2
{
    /// ------------------------------------------
    /// <summary>
    /// 
    ///     Makes a GameObject act like a Weeping
    ///     Angel (from Doctor Who).  If it is 
    ///     being watched, it freezes and cannot 
    ///     move.
    ///     
    ///     Attach this to the chaser and set the
    ///     victim to be the first person controller.
    ///     
    /// </summary>
    /// ------------------------------------------
    [RequireComponent(typeof(NavMeshAgent))]
    public class WeepingAngel : MonoBehaviour
    {
        [Header("Observer")]

        [Tooltip("Which object is being chased? (Typically this would be the first person controller)")]
        public GameObject Observer;

        [Tooltip("Detection field of view (in degrees)")]
        public float Angle = 120;

        private NavMeshAgent agent;

        void Start()
        {
            agent = this.GetComponent<NavMeshAgent>();

            // Move this object & the victim to another layer, so they do not block raycasts
            SetLayerIncludingChildren(this.gameObject, 2);
            SetLayerIncludingChildren(Observer, 2);

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
            bool isSeen = IsObserved();
            Debug.Log(isSeen);
        }

        private bool IsObserved()
        {
            // Can the observer see me?
            if (Observer)
            {
                // Where is the observer?
                Vector3 observerPosition = Observer.transform.position;
                Vector3 observerHeading = Observer.transform.forward;

                // Where is the angel?
                Vector3 angelPosition = this.transform.position;
                Vector3 angelHeading = this.transform.forward;

                float angle = Vector3.Angle(observerHeading, angelHeading);
                if (angle <= (Angle / 2))
                {

                    // Check whether the view is blocked
                    RaycastHit hitinfo;
                    bool viewBlocked = Physics.Raycast(observerPosition, observerHeading.normalized, out hitinfo);
                    if (!viewBlocked)
                    {

                        // Debug mode: draw ray to victim
                        Debug.DrawRay(observerPosition, observerHeading, Color.red);

                        // Victim can see me
                        return true;
                    }

                }

            }

            return false;
        }

    }
}
