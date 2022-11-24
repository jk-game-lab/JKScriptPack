using UnityEngine;
using System.Collections;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// ------------------------------------------
/// <summary>
/// 
///     Allows a torch to be controlled by
///     keypresses.
///     
///     Attach this script anywhere in the 
///     game (typically on first person 
///     controller) and drag the torch & 
///     light objects onto the script.
///     
/// </summary>
/// <remarks>
/// 
///     Updated to work with new Input System
///     (as well as old Input Manager).
/// 
/// </remarks>
/// ------------------------------------------
public class Torch : MonoBehaviour {

	public GameObject lightsource;
	public bool illuminate = false;
#if ENABLE_INPUT_SYSTEM
        public Key illuminateKey = Key.L;
#else
    public KeyCode illuminateKey = KeyCode.L;
#endif

    public GameObject torch;
	public bool brandish = false;
#if ENABLE_INPUT_SYSTEM
        public Key brandishKey = Key.T;
#else
    public KeyCode brandishKey = KeyCode.T;
#endif

    void Update () {

		// Check for key presses
		if (IsKeyPressed(illuminateKey)) {
			illuminate = !illuminate;
		}
		if (IsKeyPressed(brandishKey)) {
			brandish = !brandish;
		}

		// If there's a torch object, decide whether to show it
		if (torch) {
			if (brandish) {
				torch.SetActive (true);
				if (lightsource) {
					lightsource.SetActive (illuminate);
				}
			} else {
				torch.SetActive (false);
				if (lightsource) {
					lightsource.SetActive (false);
				}
			}

		// Otherwise, just handle the light on its own
		} else {
			if (lightsource) {
				lightsource.SetActive (illuminate);
			}
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

}
