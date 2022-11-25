using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// ------------------------------------------
/// <summary>
/// 
///     Uses key presses to switch between 
///     selected cameras.
///     
///     Attach this script to any gameobject
///     in the scene.  Do not attach it to any
///     camera that might be disabled!
///     
///     The first camera in the list is the
///     default camera.
///     
/// </summary>
/// <remarks>
/// 
///     Updated to work with new Input System
///     (as well as old Input Manager).
/// 
/// </remarks>
/// ------------------------------------------
public class SwitchCamera : MonoBehaviour
{

    [System.Serializable]
    public class CameraCombo
    {
        public GameObject camera;
#if ENABLE_INPUT_SYSTEM
        public Key key = Key.None;
#else
        public KeyCode key = KeyCode.None;
#endif
    }

    [Tooltip("List of cameras and associated keys.")]
    public List<CameraCombo> combos;

    [Tooltip("Should it switch back to camera 0 when the key is released?")]
    public bool temporary = true;

    void Reset()
    {
        CameraCombo main = new CameraCombo();
        main.camera = GameObject.FindGameObjectWithTag("MainCamera");
        combos = new List<CameraCombo>();
        combos.Add(main);
    }

    void Start()
    {
        if (combos.Count > 0)
        {
            enableCombo(combos[0]);
        }
    }

    void Update()
    {
        foreach (CameraCombo combo in combos)
        {
            if (IsKeyPressed(combo.key))
            {
                enableCombo(combo);
                break;
            }
            if (temporary && IsKeyReleased(combo.key))
            {
                enableCombo(combos[0]);
                break;
            }
        }
    }

    void enableCombo(CameraCombo choice)
    {
        foreach (CameraCombo combo in combos)
        {
            combo.camera.SetActive(combo == choice);
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
