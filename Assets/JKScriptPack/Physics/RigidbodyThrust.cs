using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// ------------------------------------------
/// <summary>
/// 
///     Makes a rigidbody accelerate when a 
///     key is pressed.
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
public class RigidbodyThrust : MonoBehaviour
{

#if ENABLE_INPUT_SYSTEM
    public Key key = Key.None;
#else
    public KeyCode key = KeyCode.None;
#endif

    public Vector3 thrust = Vector3.forward;

    private float scaleFactor = 20.0f;
    private Rigidbody myRigidbody;

    void Start()
    {
        myRigidbody = this.GetComponentInChildren<Rigidbody>();
        if (!myRigidbody)
        {
            myRigidbody = new Rigidbody();
        }
    }

    void FixedUpdate()
    {
        if (IsKeyHeld(key))
        {
            myRigidbody.AddRelativeForce(thrust * scaleFactor);
        }
    }

    /// <summary>
    /// Check if a key is currently being held down.
    /// </summary>
    /// <param name="k">Key on keyboard.</param>
    /// <returns>True if held; false if not.<returns>
#if ENABLE_INPUT_SYSTEM
    private bool IsKeyHeld(Key k)
    {
        // Check before lookup; current[Key.None] would cause an error
        if (k != Key.None)
        {
            return Keyboard.current[k].isPressed;
        }
        return false;
    }
#else
    private bool IsKeyHeld(KeyCode k)
    {
        return Input.GetKey(k);
    }
#endif

}
