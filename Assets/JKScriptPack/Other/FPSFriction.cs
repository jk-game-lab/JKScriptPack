using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// ------------------------------------------
/// <summary>
/// 
///     Makes the first person controller 
///     follow the movement of the surface it
///     has collided with (e.g. a moving 
///     platform).
///     
///     Can also be used with Third Person
///     Controller.
///     
/// </summary>
/// <remarks>
/// 
///     This script may not work correctly 
///     because it's based on an older
///     version of the first person controller.
/// 
///     Updated to work with new Input System
///     (as well as old Input Manager).
/// 
/// </remarks>
/// ------------------------------------------
#if ENABLE_INPUT_SYSTEM
[RequireComponent(typeof(PlayerInput))]
#endif
public class FPSFriction : MonoBehaviour
{

    private Rigidbody myRigidbody;
    private Vector3 prevPos;

    void Awake()
    {
        myRigidbody = this.GetComponentInChildren<Rigidbody>();
        if (myRigidbody)
        {
            myRigidbody.freezeRotation = true;     // Don't let the Physics Engine rotate this object (i.e. fall over when running)
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.name != "Terrain")
        {
            prevPos = other.transform.position;
        }
    }

    void OnCollisionStay(Collision other)
    {
        if (other.collider.name != "Terrain")
        {

            // Note: cannot just read the velocity of the other object because script-controlled
            // kinematic rigidbodies have velocity 0.  Hence we have to keep track of previous position
            // in order to calculate the other object's movement.
            Vector3 deltapos = other.collider.transform.position - prevPos;
            prevPos = other.collider.transform.position;

            //Debug.Log ("OnCollisionStay: " + other.collider.name + "other{pos=" + other.rigidbody.position.ToString() + ",rbvel="  + other.rigidbody.velocity.ToString() + ",deltapos=" + deltapos.ToString() + "}, my{pos=" + transform.position.ToString() + "}");

            if (IsButtonPressed("Jump"))
            {
                // Do nothing
            }
            else if (GetAxis("Horizontal") == 0 && GetAxis("Vertical") == 0)
            {
                // Note: AddForce doesn't do anything to an idle RigidbodyFPSController
                // so cannot use myRigidbody.AddForce(velocity);
                if (myRigidbody)
                {
                    myRigidbody.MovePosition(transform.position + deltapos);
                }
                else
                {
                    transform.Translate(deltapos);
                }
            }

        }
    }

    /// <summary>
    /// Check if a button has been pressed.
    /// </summary>
    /// <param name="button">Button on game controller.</param>
    /// <returns>True if pressed; false if not.<returns>
#if ENABLE_INPUT_SYSTEM
    private bool IsButtonPressed(string button)
    {
        return GetComponent<PlayerInput>().actions[button].WasPressedThisFrame();
    }
#else
    private bool IsButtonPressed(string button)
    {
        return Input.GetButtonDown(button);
    }
#endif

    /// <summary>
    /// Measure move/look direction info from the input controller.
    /// </summary>
    /// <param name="direction">Parameter used for old Input.GetAxis()</param>
    /// <returns>Number -1 to +1 indicating strength of movement on axis.<returns>
#if ENABLE_INPUT_SYSTEM
    private float GetAxis(string direction)
    {
        InputAction moveAction = GetComponent<PlayerInput>().actions["move"];
        InputAction lookAction = GetComponent<PlayerInput>().actions["look"];
        Vector2 move = moveAction.ReadValue<Vector2>();
        Vector2 look = lookAction.ReadValue<Vector2>();
        switch (direction)
        {
            case "Horizontal":
                return move.x;
            case "Vertical":
                return move.y;
            case "Mouse X":
                return look.x;
            case "Mouse Y":
                return look.y;
        }
        return 0;
    }
#else
    private float GetAxis(string direction)
    {
        return Input.GetAxis(direction);
    }
#endif

}
