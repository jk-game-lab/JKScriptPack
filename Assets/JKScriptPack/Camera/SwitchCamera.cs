using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace JKScriptPack
{

    /// <summary>
    /// Switches between specified cameras.
    /// Attach this script to any object in the game.
    /// Do not attach it to any camera that might be
    /// disabled!
    /// </summary>
    /// <remarks>
    /// 2022-10-14: Added to JKScriptPack
    /// 2022-10-22: Now compatible with both InputSystem and InputManager.
    /// </remarks>
    public class SwitchCamera : MonoBehaviour
    {

        [System.Serializable]
        public class Combo {
            public Camera camera;
#if ENABLE_INPUT_SYSTEM
            public Key key = UnityEngine.InputSystem.Key.None;
#else
    		public KeyCode key = KeyCode.None;
#endif
        }
        [Tooltip("List of cameras and associated keys.")]
        public List<Combo> combos;

        [Tooltip("Enable if the object should switch back when the key is released.")]
        public bool temporary = false;

        /// <summary>
        /// Enable the first item in the switch list
        /// </summary>
        void Start () {
            if (combos.Count > 0) {
                SwitchToCamera(combos[0]);
            }
        }

        /// <summary>
        /// Every frame, check for a keypress.
        /// </summary>
        void Update () {
            foreach (Combo combo in combos) {
#if ENABLE_INPUT_SYSTEM
                bool keyPressed = Keyboard.current[combo.key].wasPressedThisFrame;
                bool keyReleased = Keyboard.current[combo.key].wasReleasedThisFrame;
#else
                bool keyPressed = Input.GetKeyDown(combo.key);
                bool keyReleased = Input.GetKeyUp(combo.key);
#endif
                if (keyPressed) {
                    SwitchToCamera(combo);
                    break;
                }
                if (temporary && keyReleased) {
                    SwitchToCamera(combos[0]);
                    break;
                }
            }				
        }

        /// <summary>
        /// Switch to the numbered camera in the combo list.
        /// </summary>
        /// <param name="choice">Numbered camera in the combo list.</param>
        private void SwitchToCamera (Combo choice) {
            foreach (Combo combo in combos) {
                combo.camera.gameObject.SetActive(combo == choice);
            }
        }

    }

}
