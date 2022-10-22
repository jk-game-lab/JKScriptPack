using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace JKScriptPack
{

    /// <summary>
    /// Switches between specified objects.
    /// Attach this script to any static object in the game.
    /// Do not attach it to a switchable object, because this 
    /// may disable the script!
    /// </summary>
    /// <remarks>
    /// 2022-10-14: Added to JKScriptPack
    /// 2022-10-22: Now compatible with both InputSystem and InputManager.
    /// </remarks>
    public class Switch : MonoBehaviour
    {

        [System.Serializable]
        public class Combo {
            public GameObject item;
#if ENABLE_INPUT_SYSTEM
            public Key key = UnityEngine.InputSystem.Key.None;
#else
    		public KeyCode key = KeyCode.None;
#endif
        }
        [Tooltip("List of objects and associated keys.")]
        public List<Combo> combos;

        [Tooltip("Enable if the object should switch back when the key is released.")]
        public bool temporary = false;

        /// <summary>
        /// Enable the first item in the switch list
        /// </summary>
        void Start () {
            if (combos.Count > 0) {
                enableCombo(combos[0]);
            }
        }

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
                    enableCombo(combo);
                    break;
                }
                if (temporary && keyReleased) {
                    enableCombo(combos[0]);
                    break;
                }
            }				
        }

        private void enableCombo (Combo choice) {
            foreach (Combo combo in combos) {
                combo.item.SetActive(combo == choice);
            }
        }

    }

}
