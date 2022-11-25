using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// ------------------------------------------
/// <summary>
/// 
///     This script lets the player jump to 
///     any scene with a keypress.
///     
///     It is intended as an aid during game 
///     development and should be disabled in 
///     the final build.
///
///     Do NOT drag this script into your game.
///     Instead, drop the associated PREFAB 
///     into the ROOT of any scene hierarchy.
///     
/// </summary>
/// <remarks>
/// 
///     Updated to work with new Input System
///     (as well as old Input Manager).
/// 
/// </remarks>
/// ------------------------------------------
public class SceneCheat : MonoBehaviour
{

#if ENABLE_INPUT_SYSTEM
    public Key exitKey = Key.None;
#else
    public KeyCode exitKey = KeyCode.None;
#endif

    [System.Serializable]
    public class CheatItem
    {
#if ENABLE_INPUT_SYSTEM
        public Key secretKey = Key.None;
#else
        public KeyCode secretKey = KeyCode.None;
#endif
        public string sceneName;
    }

    public List<CheatItem> cheatList;

    // If this scene is reloaded, make sure it does not re-create this gameobject.
    public static SceneCheat existingInstance;

    void Awake()
    {

        if (!existingInstance)
        {
            existingInstance = this;
        }
        else if (existingInstance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(transform.gameObject);
    }

    void Reset()
    {

        // If empty, auto-populate the list
        if (SceneManager.sceneCountInBuildSettings > 0)
        {
            cheatList = new List<CheatItem>();
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                CheatItem c = new CheatItem();
                c.sceneName = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)); // Unfortunately GetSceneByBuildIndex(i).name does not work for unloaded scenes due to a bug in Unity.
                if (i < 12)
                {
#if ENABLE_INPUT_SYSTEM
                    // Unfortunately, due to Key being enumerated, this appears to be the only way to do this.
                    switch (i)
                    {
                        case 0: c.secretKey = Key.F1; break;
                        case 1: c.secretKey = Key.F2; break;
                        case 2: c.secretKey = Key.F3; break;
                        case 3: c.secretKey = Key.F4; break;
                        case 4: c.secretKey = Key.F5; break;
                        case 5: c.secretKey = Key.F6; break;
                        case 6: c.secretKey = Key.F7; break;
                        case 7: c.secretKey = Key.F8; break;
                        case 8: c.secretKey = Key.F9; break;
                        case 9: c.secretKey = Key.F10; break;
                        case 10: c.secretKey = Key.F11; break;
                        case 11: c.secretKey = Key.F12; break;
                    }
#else
                    // Unfortunately, due to KeyCode being enumerated, this appears to be the only way to do this.
                    switch (i)
                    {
                        case 0: c.secretKey = KeyCode.F1; break;
                        case 1: c.secretKey = KeyCode.F2; break;
                        case 2: c.secretKey = KeyCode.F3; break;
                        case 3: c.secretKey = KeyCode.F4; break;
                        case 4: c.secretKey = KeyCode.F5; break;
                        case 5: c.secretKey = KeyCode.F6; break;
                        case 6: c.secretKey = KeyCode.F7; break;
                        case 7: c.secretKey = KeyCode.F8; break;
                        case 8: c.secretKey = KeyCode.F9; break;
                        case 9: c.secretKey = KeyCode.F10; break;
                        case 10: c.secretKey = KeyCode.F11; break;
                        case 11: c.secretKey = KeyCode.F12; break;
                    }
#endif
                }
                cheatList.Add(c);
            }
        }

    }

    private void Update()
    {

        // Is the key in our list?
        if (IsKeyPressed(exitKey))
        {
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        foreach (CheatItem cheat in cheatList)
        {
            if (IsKeyPressed(cheat.secretKey))
            {
                LoadScene(cheat.sceneName);
                break;
            }
        }

    }

    /// <summary>
    /// Load a named scene.
    /// </summary>
    /// <param name="sceneName">Name of the scene to load.  If empty, the current scene is restarted.</param>
    public void LoadScene(string sceneName)
    {
        if (sceneName.Equals(""))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            SceneManager.LoadScene(sceneName);
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
