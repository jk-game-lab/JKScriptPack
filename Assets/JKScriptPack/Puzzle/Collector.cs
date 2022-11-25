using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental.GraphView;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// ------------------------------------------
/// <summary>
/// 
///     Attach this to the first person 
///     controller (FPC).
///     
///     Create a list of GameObjects to 
///     collect.  When the FPC collides with 
///     the specified GameObject, the object 
///     will disappear and be added to a list.
///
///     When you have collected all objects, 
///     your reward will be to reveal (or hide) 
///     another gameobject (such as a barrier 
///     or prize).
///     
///     Collectable gameobjects do not need to 
///     be set as triggers; this will be done
///     automatically.
///     
/// </summary>
/// <remarks>
/// 
///     Updated to work with new Input System
///     (as well as old Input Manager).
///     
/// </remarks>
/// ------------------------------------------
public class Collector : MonoBehaviour
{

    [System.Serializable]
    public class Collectable
    {
        public GameObject gameObject;
        public bool collected = false;
        //public int points = 10;
    }
    public Collectable[] collectables;

#if ENABLE_INPUT_SYSTEM
    public Key pressKeyToCollect = Key.None;
#else
    public KeyCode pressKeyToCollect = KeyCode.None;
#endif
    public AudioClip collectionSound;
    public AudioClip finishedSound;
    public GameObject finishedReveal;
    public GameObject finishedHide;

    private AudioSource audiosource;

    void Start()
    {

        // Reset rewards
        if (finishedReveal)
        {
            finishedReveal.gameObject.SetActive(false);
        }
        if (finishedHide)
        {
            finishedHide.gameObject.SetActive(true);
        }

        // Make all collectables be triggers
        SetAllTriggers(true);

        // Initialise audio
        audiosource = gameObject.AddComponent<AudioSource>();

    }

    private void SetAllTriggers(bool state)
    {
        foreach (Collectable item in collectables)
        {
            if (item.gameObject && item.gameObject.GetComponent<Collider>())
            {
                item.gameObject.GetComponent<Collider>().isTrigger = state;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {   // Use for collection with a keypress
        //Debug.Log("OnTriggerStay=" + other.gameObject.name);
        if (IsNone(pressKeyToCollect) || IsKeyPressed(pressKeyToCollect))
        {
            CollectItem(other.gameObject);
        }
    }

    //------------------------------------------------------------
    // This code no longer required but kept for reference

    //	void OnControllerColliderHit(ControllerColliderHit other) {		// Use for collection without a keypress
    //		if (other.gameObject.name != "Terrain") {					// Don't waste time if the FPC is colliding with the terrain
    //			//Debug.Log("OnControllerColliderHit=" + other.gameObject.name);
    //			CollectItem (other.gameObject);
    //		}
    //	}
    //
    //	void OnCollision(Collision other) {
    //		//Debug.Log("OnCollision=" + other.gameObject.name);
    //	}

    //------------------------------------------------------------

    private void CollectItem(GameObject other)
    {

        // Is the item in our list?
        foreach (Collectable collectable in collectables)
        {
            if (other.Equals(collectable.gameObject))
            {

                // Collect it
                collectable.collected = true;
                Destroy(other);
                audiosource.PlayOneShot(collectionSound);

                // Have all objects been collected?
                if (AllCollected())
                {
                    if (finishedReveal)
                    {
                        finishedReveal.gameObject.SetActive(true);
                    }
                    if (finishedHide)
                    {
                        finishedHide.gameObject.SetActive(false);
                    }
                    audiosource.PlayOneShot(finishedSound);
                }

                break;
            }
        }
    }

    public bool AllCollected()
    {
        bool all = true;
        foreach (Collectable item in collectables)
        {
            if (!item.collected)
            {
                all = false;
                break;
            }
        }
        return all;
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
