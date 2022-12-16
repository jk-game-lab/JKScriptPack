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

        [Tooltip("How high are the eyes (above the object pivot)?")]
        public float EyeHeight = 1.5f;

        private NavMeshAgent agent;
        private bool agentState;

        void Reset()
        {
            // When this script is attached to an object, set certain properties automatically
            GameObject playerCapsule = GameObject.Find("PlayerCapsule");
            if (playerCapsule)
            {
                Observer = playerCapsule;
                CharacterController controller = Observer.transform.GetComponent<CharacterController>();
                EyeHeight = controller.height * 0.85f;
            }
        }

        void Start()
        {
            // Connect to the NavMeshAgent
            agent = this.GetComponent<NavMeshAgent>();
            agentState = agent.enabled;

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
            if (IsObserved())
            {
                agent.enabled = false;
            }
            else
            {
                agent.enabled = agentState;
            }
        }

        private bool IsObserved()
        {
            if (Observer)
            {
                Vector3 eyeOffset = new Vector3(0, EyeHeight, 0);
                Vector3 observerPosition = Observer.transform.position + eyeOffset;
                Vector3 observerRotation = Observer.transform.forward;
                Vector3 angelPosition = this.transform.position + eyeOffset;
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
                    RaycastHit hit;
                    bool isBlocked = Physics.Raycast(ray, out hit);
                    if (!isBlocked)
                    {
                        // DEBUG: Draw a ray to the angel
                        float distance = observerToAngel.magnitude;
                        //Debug.DrawLine(observerPosition, angelPosition, Color.red, distance);
                        Debug.DrawRay(observerPosition, observerToAngel, Color.red);

                        // Observer can see angel
                        return true;
                    }

                }

            }
            return false;
        }

    }
}
