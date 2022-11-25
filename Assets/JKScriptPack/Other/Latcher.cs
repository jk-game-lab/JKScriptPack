using UnityEngine;
using System.Collections;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// ------------------------------------------
/// <summary>
/// 
///     When the First Person Controller (FPC) 
///     wanders into a trigger area then the 
///     FPC will latch to the trigger area.
///     
///     Pressing any key will make the FPC 
///     un-latch.
///     
///     Apply this script to the trigger area.
///     
/// </summary>
/// <remarks>
/// 
///     The purpose of this script has been
///     lost in the mists of time!  Looks like
///     it was used to lock onto a moving
///     platform or conveyor of some kind.
/// 
///     Updated to work with new Input System
///     (as well as old Input Manager).
/// 
/// </remarks>
/// ------------------------------------------
public class Latcher : MonoBehaviour
{

#if ENABLE_INPUT_SYSTEM
    public Key keyToHold = Key.L;
#else
    public KeyCode keyToHold = KeyCode.L;
#endif

    private GameObject fpc;                 // Set to FPC if latched; null if not latched

    private Quaternion fpcOriginalRotation;

    void Start()
    {
        fpcOriginalRotation = this.transform.rotation;      // Set a default just in case
    }

    void OnTriggerEnter(Collider other)
    {
        if (!fpc && IsNone(keyToHold))
        {
            Latch(other);
        }
    }

    void OnTriggerStay(Collider other)
    {

        if (!fpc && !IsNone(keyToHold) && IsKeyPressed(keyToHold))
        {
            Latch(other);
        }

        // Move with trigger zone
        if (fpc)
        {
            fpc.transform.position = this.transform.position;
            fpc.transform.rotation = this.transform.rotation;
        }

        // Unlatch?
        if (fpc && ((IsNone(keyToHold) && IsAnyKeyPressed()) || IsKeyReleased(keyToHold)))
        {
            fpc.transform.rotation = fpcOriginalRotation;
            fpc = null;
        }

    }

    /// <summary>
    /// Latch the First Person Controller.
    /// </summary>
    /// <param name="other">Collider to latch to.</param>
	void Latch(Collider other)
    {
        if (other.gameObject.name == "FPSController"
            || other.gameObject.name == "RigidbodyFPSController"
            || other.gameObject.name == "ThirdPersonController"
            || other.gameObject.name == "PlayerCapsule")
        {
            fpc = other.gameObject;
            fpcOriginalRotation = fpc.transform.localRotation;
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

    /// <summary>
    /// Check if a key is set to "None".
    /// </summary>
    /// <param name="k">Key on keyboard.</param>
    /// <returns>True if it matches "None"; otherwise false.<returns>
#if ENABLE_INPUT_SYSTEM
    private bool IsNone(Key k)
    {
        return (k == Key.None);
    }
#else
    private bool IsNone(KeyCode k)
    {
        return (k == KeyCode.None);
    }
#endif

    /// <summary>
    /// Check if any key is pressed.
    /// </summary>
    /// <returns>True if any key is pressed; otherwise false.<returns>
#if ENABLE_INPUT_SYSTEM
    private bool IsAnyKeyPressed()
    {
        return Keyboard.current.anyKey.isPressed;
    }
#else
    private bool IsAnyKeyPressed()
    {
        return Input.anyKeyDown;
    }
#endif

}
