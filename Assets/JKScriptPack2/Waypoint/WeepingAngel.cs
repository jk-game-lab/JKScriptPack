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
    ///     being observed, it freezes and cannot 
    ///     move.
    ///     
    ///     Attach this to a chaser and set the
    ///     observer to the first person controller.
    ///     
    /// </summary>
    /// ------------------------------------------
    [RequireComponent(typeof(NavMeshAgent))]
    public class WeepingAngel : MonoBehaviour
    {
        [Header("Observer")]

        [Tooltip("Object being chased by the angel? (Typically the first person controller)")]
        public GameObject Observer;

        [Tooltip("Detection field of view (in degrees)")]
        public float FieldOfView = 60;

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
            Debug.Log("Seen = " + isSeen);
        }

        private bool IsObserved()
        {
            if (Observer)
            {
                Vector3 observerPosition = Observer.transform.position;
                Vector3 observerRotation = Observer.transform.forward;
                Vector3 angelPosition = this.transform.position;
                Vector3 observerToAngel = angelPosition - observerPosition;
                float halfFOV = FieldOfView / 2;

                // DEBUG: Draw the field-of-view
                float lineLength = 50.0f;
                Vector3 leftLimit = (Quaternion.AngleAxis(-halfFOV, Vector3.up) * observerRotation) * lineLength;
                Vector3 rightLimit = (Quaternion.AngleAxis(+halfFOV, Vector3.up) * observerRotation) * lineLength;
                Debug.DrawLine(observerPosition, observerPosition + leftLimit, Color.yellow);
                Debug.DrawLine(observerPosition, observerPosition + rightLimit, Color.yellow);

                // Is the angel within the observer's field-of-view?
                float angle = Vector3.Angle(observerToAngel, observerRotation);
                if (angle <= halfFOV)
                {

                    // Check whether the angel can be seen
                    Ray ray = new Ray(observerPosition, observerToAngel.normalized);
                    RaycastHit hitinfo;
                    bool isVisible = Physics.Raycast(ray, out hitinfo);
                    if (isVisible)
                    {
                        // DEBUG: Draw a ray to the angel
                        Debug.DrawLine(observerPosition, angelPosition, Color.red);

                        // Victim can see me
                        return true;
                    }

                }

            }
            return false;
        }

    }
}
