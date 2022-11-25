using UnityEngine;
using System.Collections;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// ------------------------------------------
/// <summary>
/// 
///     Plays a sound when a key is pressed.
///     
///     Attach this script to any gameobject
///     in the scene.
///     
/// </summary>
/// <remarks>
/// 
///     Updated to work with new Input System
///     (as well as old Input Manager).
/// 
/// </remarks>
/// ------------------------------------------
public class OnKeyPlaySound : MonoBehaviour
{

#if ENABLE_INPUT_SYSTEM
    public Key key = Key.None;
#else
    public KeyCode key = KeyCode.None;
#endif

    public AudioClip sound = null;
    public float volume = 1.0f;
    public bool loop = false;
    public AudioClip soundWhenReleased = null;

    private AudioSource audiosource;
    private bool pressed;

    void Start()
    {
        audiosource = gameObject.AddComponent<AudioSource>();
        pressed = false;
    }

    void Update()
    {
        if (!pressed && IsKeyPressed(key))
        {
            pressed = true;
            audiosource.volume = volume;
            if (loop || soundWhenReleased)
            {
                audiosource.clip = sound;
                audiosource.loop = loop;
                audiosource.Play();
            }
            else
            {
                if (sound) audiosource.PlayOneShot(sound);
            }
        }
        if (pressed && IsKeyReleased(key))
        {
            pressed = false;
            if (loop || soundWhenReleased)
            {
                audiosource.Stop();
            }
            if (soundWhenReleased)
            {
                audiosource.clip = soundWhenReleased;
                audiosource.loop = false;
                audiosource.Play();
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

