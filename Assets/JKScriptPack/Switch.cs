using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JKScriptPack
{

    /// <summary>
    /// Switches between specified objects.
    /// Attach this script to any static object in the game.
    /// Do not attach it to a switchable object, because this may diable the script!
    /// </summary>
    /// <remarks>
    /// 2022-10-14: Added to JKScriptPack
    /// </remarks>
    public class Switch : MonoBehaviour
    {

        [System.Serializable]
        public class Combo {
            public GameObject item;
            public KeyCode key = KeyCode.None;
        }
        [Tooltip("List of objects and associated keys.")]
        public List<Combo> combos;

        [Tooltip("Enable if the object should switch back when the key is released.")]
        public bool temporary = true;

        [Tooltip("GameObject located at the destination (best using an empty gameobject)")]
        public GameObject destination;

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
                if (Input.GetKeyDown(combo.key)) {
                    enableCombo(combo);
                    break;
                }
                if (temporary && Input.GetKeyUp(combo.key)) {
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
