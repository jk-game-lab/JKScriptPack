using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace JKScriptPack2
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
    ///     Works with both new Input System and
    ///     old Input Manager.
    /// 
    /// </remarks>
    /// ------------------------------------------
    public class SwitchCamera : MonoBehaviour
    {

        [System.Serializable]
        public class SelectableCamera
        {
            public Camera Camera;

#if ENABLE_INPUT_SYSTEM
            public Key Key = Key.None;
#else
            public KeyCode Key = KeyCode.None;
#endif

        }

        [Tooltip("List of cameras and associated keys.")]
        public List<SelectableCamera> CameraList;

        [Tooltip("Should it switch back to camera 0 when the key is released?")]
        public bool IsTemporary = false;

		// void Reset()
		// {
		// 	SelectableCamera main = new SelectableCamera();
		// 	main.Camera = GameObject.FindGameObjectWithTag("MainCamera");
		// 	CameraList = new List<SelectableCamera>();
		// 	CameraList.Add(main);
		// }

        void Start()
        {
            if (CameraList.Count > 0)
            {
                SwitchToCamera(CameraList[0]);
            }
        }

        void Update()
        {
            foreach (SelectableCamera camera in CameraList)
            {
                if (IsKeyPressed(camera.Key))
                {
                    SwitchToCamera(camera);
                    break;
                }
                if (IsTemporary && IsKeyReleased(camera.Key))
                {
                    SwitchToCamera(CameraList[0]);
                    break;
                }
            }
        }

        /// <summary>
        /// Switch to the numbered camera in the list.
        /// </summary>
        /// <param name="choice">Numbered camera in the list.</param>
        private void SwitchToCamera(SelectableCamera choice)
        {
            foreach (SelectableCamera camera in CameraList)
            {
                camera.Camera.gameObject.SetActive(camera == choice);
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

    }

}
