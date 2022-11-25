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
///     like a tank turret.
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
public class TankTurretController : MonoBehaviour
{

    public bool mouseX = true;
    public bool mouseY = true;

#if ENABLE_INPUT_SYSTEM
    public Key rotateLeft = Key.None;
    public Key rotateRight = Key.None;
    public Key rotateUp = Key.None;
    public Key rotateDown = Key.None;
#else
    public KeyCode rotateLeft = KeyCode.None;
    public KeyCode rotateRight = KeyCode.None;
    public KeyCode rotateUp = KeyCode.None;
    public KeyCode rotateDown = KeyCode.None;
#endif


    public float maxUp = 45.0f;
    public float maxDown = 5.0f;

    public float maxLeft = 90.0f;
    public float maxRight = 90.0f;

    public float turretSpeed = 90.0f;

    private float leftRight;
    private float upDown;

    void Start()
    {

        maxUp = Mathf.Abs(maxUp);
        maxDown = Mathf.Abs(maxDown);
        maxLeft = Mathf.Abs(maxLeft);
        maxRight = Mathf.Abs(maxRight);

        leftRight = 0.0f;
        upDown = 0.0f;

    }

    void Update()
    {

        if (mouseX)
        {
            leftRight += GetAxis("Mouse X") * turretSpeed * Time.deltaTime;
        }
        if (IsKeyHeld(rotateLeft))
        {
            leftRight -= turretSpeed * Time.deltaTime;
        }
        if (IsKeyHeld(rotateRight))
        {
            leftRight += turretSpeed * Time.deltaTime;
        }
        leftRight = Mathf.Clamp(leftRight, -maxLeft, maxRight);

        if (mouseY)
        {
            upDown += GetAxis("Mouse Y") * turretSpeed * Time.deltaTime;
        }
        if (IsKeyHeld(rotateUp))
        {
            upDown += turretSpeed * Time.deltaTime;
        }
        if (IsKeyHeld(rotateDown))
        {
            upDown -= turretSpeed * Time.deltaTime;
        }
        upDown = Mathf.Clamp(upDown, -maxDown, maxUp);

        transform.localRotation = Quaternion.Euler(-upDown, leftRight, 0);

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
