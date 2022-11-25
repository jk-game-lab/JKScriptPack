using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// ------------------------------------------
/// <summary>
/// 
///     Pressing set keys will make the object
///     move to a set waypoint.
///     
///     Attach this to a gameobject.
///     
/// </summary>
/// <remarks>
/// 
///     Updated to work with new Input System
///     (as well as old Input Manager).
///     
/// </remarks>
/// ------------------------------------------
public class Hopper : MonoBehaviour
{

    [System.Serializable]
    public class Waypoint
    {
        public GameObject gameObject;
#if ENABLE_INPUT_SYSTEM
        public Key directKey = Key.None;
#else
        public KeyCode directKey = KeyCode.None;
#endif
    }
    public Waypoint[] waypoints;
    public int currentWaypoint = 0;

    public float speed = 1.0f;

#if ENABLE_INPUT_SYSTEM
    public Key nextKey = Key.None;
    public Key prevKey = Key.None;
#else
    public KeyCode nextKey = KeyCode.None;
    public KeyCode prevKey = KeyCode.None;
#endif

    void Update()
    {

        // Prev/Next keys
        if (IsKeyPressed(prevKey))
        {
            currentWaypoint--;
            if (currentWaypoint < 0)
            {
                currentWaypoint = 0;
            }
        }
        if (IsKeyPressed(nextKey))
        {
            currentWaypoint++;
            if (currentWaypoint > waypoints.Length)
            {
                currentWaypoint = waypoints.Length;
            }
        }

        // Direct key
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (IsKeyPressed(waypoints[i].directKey))
            {
                currentWaypoint = i;
                break;
            }
        }

        // Do we need to move?
        Vector3 current = this.transform.position;
        Vector3 target = waypoints[currentWaypoint].gameObject.transform.position;
        Vector3 movement = target - current;
        if (movement.magnitude < 0.1)
        {
            this.transform.position = target;
        }
        else
        {
            this.transform.position = Vector3.Lerp(current, target, speed * Time.deltaTime);
        }

    }

    /// <summary>
    /// Check if a key has been pressed.
    /// </summary>
    /// <param name="k">Key on keyboard.</param>
    /// <returns>True if pressed; false if not.<returns>
#if ENABLE_INPUT_SYSTEM
    private bool IsKeyPressed(Key k)
    {
        // Check before lookup; current[Key.None] would cause an error
        if (k != Key.None)
        {
            return Keyboard.current[k].wasPressedThisFrame;
        }
        return false;
    }
#else
    private bool IsKeyPressed(KeyCode k)
    {
        return Input.GetKeyDown(k);
    }
#endif

}
