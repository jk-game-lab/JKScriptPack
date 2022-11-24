using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace JKScriptPack
{

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
        public class SelectableCamera {
            public Camera Camera;

#if ENABLE_INPUT_SYSTEM
            public Key Key = Key.None;
#else
            public KeyCode Key = KeyCode.None;
#endif

        }

        [Tooltip("List of cameras and associated keys.")]
        public List<SelectableCamera> CameraList;

        [Tooltip("Enable to switch back to camera 0 when the key is released.")]
        public bool IsTemporary = false;

        void Start () {
            if (CameraList.Count > 0) {
                SwitchToCamera(CameraList[0]);
            }
        }

        void Update () {
            foreach (SelectableCamera camera in CameraList) {

                bool keyPressed = false;
                bool keyReleased = false;
#if ENABLE_INPUT_SYSTEM
                if (camera.Key != Key.None) {    // 'None' is unavailable in current[] array
                    keyPressed = Keyboard.current[camera.Key].wasPressedThisFrame;
                    keyReleased = Keyboard.current[camera.Key].wasReleasedThisFrame;
                }
#else
                keyPressed = Input.GetKeyDown(camera.Key);
                keyReleased = Input.GetKeyUp(camera.Key);
#endif

                if (keyPressed) {
                    SwitchToCamera(camera);
                    break;
                }
                if (IsTemporary && keyReleased) {
                    SwitchToCamera(CameraList[0]);
                    break;
                }
            }				
        }

        /// <summary>
        /// Switch to the numbered camera in the list.
        /// </summary>
        /// <param name="choice">Numbered camera in the list.</param>
        private void SwitchToCamera (SelectableCamera choice) {
            foreach (SelectableCamera camera in CameraList) {
                camera.Camera.gameObject.SetActive(camera == choice);
            }
        }

    }

}
