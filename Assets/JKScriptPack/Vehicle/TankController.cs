using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// ------------------------------------------
/// <summary>
/// 
///     Attach to a gameobject to make it move
///     like a tank.
///     
/// </summary>
/// <remarks>
/// 
///     Updated to work with new Input System
///     (as well as old Input Manager).
///     
/// </remarks>
/// ------------------------------------------
#if ENABLE_INPUT_SYSTEM
[RequireComponent(typeof(PlayerInput))]
#endif
public class TankController : MonoBehaviour
{

    public float forwardSpeed = 10;
    public float reverseSpeed = 5;
    public float turningSpeed = 45;

    public bool nativeControls = true;
#if ENABLE_INPUT_SYSTEM
    public Key keyForward = Key.None;
    public Key keyBack = Key.None;
    public Key keyLeft = Key.None;
    public Key keyRight = Key.None;
#else
    public KeyCode keyForward = KeyCode.None;
    public KeyCode keyBack = KeyCode.None;
    public KeyCode keyLeft = KeyCode.None;
    public KeyCode keyRight = KeyCode.None;
#endif


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
        if ((nativeControls && GetAxis("Vertical") > 0) || IsKeyHeld(keyForward))
        {
            myRigidbody.MovePosition(transform.position + transform.forward * forwardSpeed * Time.deltaTime);
        }
        if ((nativeControls && GetAxis("Vertical") < 0) || IsKeyHeld(keyBack))
        {
            myRigidbody.MovePosition(transform.position - transform.forward * reverseSpeed * Time.deltaTime);
        }
        if ((nativeControls && GetAxis("Horizontal") > 0) || IsKeyHeld(keyRight))
        {
            Vector3 axis = new Vector3(0, turningSpeed, 0);
            Quaternion theta = Quaternion.Euler(axis * Time.deltaTime);
            myRigidbody.MoveRotation(myRigidbody.rotation * theta);
        }
        if ((nativeControls && GetAxis("Horizontal") < 0) || IsKeyHeld(keyLeft))
        {
            Vector3 axis = new Vector3(0, -turningSpeed, 0);
            Quaternion theta = Quaternion.Euler(axis * Time.deltaTime);
            myRigidbody.MoveRotation(myRigidbody.rotation * theta);
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
