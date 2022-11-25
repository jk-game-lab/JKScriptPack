using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// ------------------------------------------
/// <summary>
/// 
///     Allows a player to "grab" objects.
///
///     Create a trigger zone (invisible 
///     GameObject with Collider.isTrigger 
///     enabled) and attach it to the player 
///     as a kind of 'invisible hand'.  Apply
///     this script to the zone.
///     
///     A grabbable object that comes within 
///     this zone may be grabbed by pressing 
///     a key.
///		
///     To make an object grabbable, change 
///     its tag to "grabbable".  (You may 
///     need to add a new tag to the list.)
///     
/// </summary>
/// <remarks>
/// 
///     Updated to work with new Input System
///     (as well as old Input Manager).
/// 
/// </remarks>
/// ------------------------------------------
public class Grabber : MonoBehaviour
{

    [Tooltip("Which key grabs things?")]
#if ENABLE_INPUT_SYSTEM
    public Key grabKey = Key.G;
#else
    public KeyCode grabKey = KeyCode.G;
#endif

    [Tooltip("Press key to grab and again to release?")]
    public bool toggle = true;

    [Tooltip("Does the grabbable get pulled to me?")]
    public bool moveToMe = true;
    public float moveSpeed = 10.0f;
    //public bool followMe = false;

    private GameObject triggered = null;
    private GameObject grabbed = null;
    private bool gravity;
    private bool trapped;
    private Vector3 relativePosition;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.ToLower() == "grabbable")
        {
            triggered = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        triggered = null;
    }

    void Update()
    {

        if (IsKeyPressed(grabKey))
        {
            if (triggered && !grabbed)
            {
                Grab(triggered);
            }
            else if (toggle)
            {
                Release();
            }
        }
        else if (IsKeyReleased(grabKey) && !toggle)
        {
            Release();
        }

        if (grabbed)
        {
            if (moveToMe && !trapped)
            {
                if (relativePosition.magnitude > 0.1f)
                {
                    grabbed.transform.position = Vector3.Lerp(grabbed.transform.position, this.transform.position, Time.deltaTime * moveSpeed);
                    relativePosition = grabbed.transform.position - this.transform.position;
                }
                else
                {
                    grabbed.transform.position = this.transform.position;
                    //if (!followMe) {
                    trapped = true;
                    //}
                }
            }
            grabbed.transform.position = this.transform.position + relativePosition;
        }
    }

    /// <summary>
    /// Capture details of the object to drag.
    /// </summary>
    /// <param name="grabbable">The GameObject that will be dragged.</param>
    public void Grab(GameObject grabbable)
    {
        grabbed = grabbable;
        relativePosition = grabbed.transform.position - this.transform.position;
        Rigidbody rb = grabbed.GetComponentInChildren<Rigidbody>();
        if (rb)
        {
            gravity = rb.useGravity;
            rb.useGravity = false;
        }
        trapped = false;
    }

    /// <summary>
    /// Return the dragged object to normality.
    /// </summary>
	void Release()
    {
        if (grabbed)
        {
            Rigidbody rb = grabbed.GetComponentInChildren<Rigidbody>();
            if (rb)
            {
                rb.useGravity = gravity;
            }
            trapped = false;
            grabbed = null;
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
        if (k != Key.None) {     
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

    /// <summary>
    /// Check if a key has been released.
    /// </summary>
    /// <param name="k">Key on keyboard.</param>
    /// <returns>True if pressed; false if not.<returns>
#if ENABLE_INPUT_SYSTEM
    private bool IsKeyReleased(Key k)
    {
        // Check before lookup; current[Key.None] would cause an error
        if (k != Key.None) {     
            return Keyboard.current[k].wasReleasedThisFrame;
        }
        return false;
    }
#else
    private bool IsKeyReleased(KeyCode k)
    {
        return Input.GetKeyUp(k);
    }
#endif

}
