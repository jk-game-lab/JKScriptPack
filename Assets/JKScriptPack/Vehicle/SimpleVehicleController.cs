using UnityEngine;
using System.Collections;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// ------------------------------------------
/// <summary>
/// 
///     Attach to a gameobject to make it move
///     like a vehicle.
///     
/// </summary>
/// <remarks>
/// 
///     Updated to work with new Input System
///     (as well as old Input Manager).
///     
///     Had previously started adding 
///     collision detection but this was 
///     unfinished.
/// 
/// </remarks>
/// ------------------------------------------
#if ENABLE_INPUT_SYSTEM
[RequireComponent(typeof(PlayerInput))]
#endif
public class SimpleVehicleController : MonoBehaviour
{

    public float maxAcceleration = 2.5f;
    public float maxForwardSpeed = 30.0f;
    public float maxReverseSpeed = 5.0f;
    public float maxSteeringAngle = 50.0f;

    public GameObject wheelFrontLeft;
    public GameObject wheelFrontRight;
    public GameObject wheelRearLeft;
    public GameObject wheelRearRight;

    public float rollingResistance = 0.25f;
    public string brakeInput = "Fire2";
    public bool allowTurnOnSpot = false;

    private float speed;
    private float brakingDeceleration = 10.0f;
    private float wheelRollAngle;

    void Start()
    {

        // Initialise
        speed = 0.0f;
        wheelRollAngle = 0.0f;

        // Force speed limits to be positive numbers
        maxForwardSpeed = Mathf.Abs(maxForwardSpeed);
        maxReverseSpeed = Mathf.Abs(maxReverseSpeed);
        maxSteeringAngle = Mathf.Abs(maxSteeringAngle);

    }

    void Update()
    {

        // Forward & backward acceleration
        float thrust = GetAxis("Vertical");
        if (thrust == 0)
        {
            speed *= 1 - (rollingResistance * Time.deltaTime);
        }
        else
        {
            speed += (thrust * maxAcceleration) * Time.deltaTime;
        }
        if (GetAxis(brakeInput) > 0)
        {
            speed -= Mathf.Sign(speed) * (brakingDeceleration * Time.deltaTime);
        }
        if (speed > maxForwardSpeed) speed = maxForwardSpeed;
        if (speed < -maxReverseSpeed) speed = -maxReverseSpeed;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Apply wind resistance

        // Make wheels revolve
        wheelRollAngle += 180 * speed * Time.deltaTime;
        while (wheelRollAngle > 360) wheelRollAngle -= 360;

        // Turning
        float steeringAngle = GetAxis("Horizontal") * maxSteeringAngle;
        if (allowTurnOnSpot || Mathf.Abs(speed) > 1.0f)
        {
            transform.Rotate(0, steeringAngle * Time.deltaTime, 0);
        }

        // Rotate front wheels
        Quaternion frontRotation = Quaternion.Euler(wheelRollAngle, steeringAngle / 2, 0);
        if (wheelFrontLeft)
        {
            wheelFrontLeft.transform.localRotation = frontRotation;
        }
        if (wheelFrontRight)
        {
            wheelFrontRight.transform.localRotation = frontRotation;
        }
        Quaternion rearRotation = Quaternion.Euler(wheelRollAngle, 0, 0);
        if (wheelRearLeft)
        {
            wheelRearLeft.transform.localRotation = rearRotation;
        }
        if (wheelRearRight)
        {
            wheelRearRight.transform.localRotation = rearRotation;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger " + other.gameObject.name);
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision " + other.gameObject.name);
    }

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
