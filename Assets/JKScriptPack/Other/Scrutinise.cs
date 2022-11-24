using UnityEngine;
using System.Collections;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace JKScriptPack1
{

    /// ------------------------------------------
    /// <summary>
    /// 
    ///     Zooms the camera's Field-Of-View (FOV)
    ///     when a key is pressed.
    ///     
    /// </summary>
    /// <remarks>
    /// 
    ///     Updated to work with new Input System
    ///     (as well as old Input Manager).
    ///     
    ///     Bug: did not zoom fully.  Now fixed.
    /// 
    /// </remarks>
    /// ------------------------------------------
    public class Scrutinise : MonoBehaviour
    {

#if ENABLE_INPUT_SYSTEM
        public Key zoomKey = Key.None;
#else
        public KeyCode zoomKey = KeyCode.None;
#endif

        public float zoomedFOV = 20.0f;
        public float transitionTime = 0.33f;

        private float defaultFOV = 60.0f;
        private float transit;

        void Start()
        {
            defaultFOV = this.GetComponent<Camera>().fieldOfView;
            transit = 0.0f;
        }

        void Update()
        {
            float proportionOfTransit = Time.deltaTime / transitionTime;
            if (IsKeyHeld(zoomKey))
            {
                transit += proportionOfTransit;
                if (transit > 1.0f)
                {
                    transit = 1.0f;
                }
            }
            else
            {
                transit -= proportionOfTransit;
                if (transit < 0.0f)
                {
                    transit = 0.0f;
                }
            }
            this.GetComponent<Camera>().fieldOfView = Mathf.Lerp(defaultFOV, zoomedFOV, transit);

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
            if (k != Key.None) {     
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
}
