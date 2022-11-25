using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.UIElements.UxmlAttributeDescription;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// ------------------------------------------
/// <summary>
/// 
///     Attach this script to any gameobject 
///     in the scene.
///     
///     However, if you wish to use the 
///     'collide' facility, you must attach 
///     this script to the first person 
///     controller.
///     
///     Leaving the scene blank will restart 
///     the current level.
///     
/// </summary>
/// <remarks>
/// 
///     Updated to work with new Input System
///     (as well as old Input Manager).
/// 
/// </remarks>
/// ------------------------------------------
public class LoadNewScene : MonoBehaviour
{

    public string sceneName;
    public GameObject onCollisionWith;
#if ENABLE_INPUT_SYSTEM
    public Key onKeyPress;
#else
    public KeyCode onKeyPress;
#endif
    public float onTimeout;
    public bool triggerAndKey = false;

    private float timeSoFar;
    private bool triggered = false;

    void Start()
    {
        timeSoFar = 0.0f;
    }

    void Update()
    {

        // Check for keypress
        if (IsKeyPressed(onKeyPress))
        {
            if (triggerAndKey)
            {
                if (triggered)
                {
                    LoadScene();
                }
            }
            else
            {
                LoadScene();
            }
        }

        // Check for timeout
        if (onTimeout > 0)
        {
            timeSoFar += Time.deltaTime;
            if (timeSoFar >= onTimeout)
            {
                LoadScene();
            }
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        CheckCollision(collision.gameObject);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        CheckCollision(hit.collider.gameObject);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (triggerAndKey)
        {
            triggered = true;
        }
        else
        {
            CheckCollision(collider.gameObject);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        triggered = false;
    }

    private void CheckCollision(GameObject other)
    {
        if (other.name != "Terrain")
        {
            //Debug.Log (other.name + " ... " + onCollisionWith.gameObject.name);
            if (onCollisionWith && (other.Equals(onCollisionWith) || other.name == onCollisionWith.name || other.name == (onCollisionWith.name + "(Clone)")))
            {
                LoadScene();
            }
        }
    }

    public void LoadScene()
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
        if (k != Key.None)
        {
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
