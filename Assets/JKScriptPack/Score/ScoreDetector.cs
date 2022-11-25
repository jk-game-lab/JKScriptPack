/*
 *  ScoreDetector.cs
 * 
 *  Attach this script to a gameobject which is hit by another game object.
 *
 *  This script adjusts the score held in GlobalScore.cs
 *
 *  v1.40 -- added to JKScriptPack
 *	
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDetector : MonoBehaviour
{

    [Header("On collision with...")]
    public GameObject otherObject;
    public bool destroyOtherObj = true;

    [Header("Change score")]
    public int points = 10;

    private Collider mycollider;

    void Start()
    {
        mycollider = this.GetComponentInChildren<Collider>();
        if (mycollider)
        {
            mycollider.isTrigger = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log ("Trigger: " + other.gameObject.name);
        if (otherObject)
        {
            if (other.gameObject == otherObject || other.gameObject.name == otherObject.name || other.gameObject.name == (otherObject.name + "(Clone)"))
            {
                GlobalScore.Add(points);
                if (destroyOtherObj)
                {
                    Destroy(other.gameObject);
                }
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name != "Terrain")
        {
            //Debug.Log ("Collision: " + other.gameObject.name);
        }
    }

}
