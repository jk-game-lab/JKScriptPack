/*
 *  Spawner.cs
 * 
 *  Spawns objects automatically.  Objects appear at set intervals. 
 *  The object will be created at locations specified by a list of gameobjects.
 *  (These gameobjects should, ideally, be empty gameobjects like you'd use
 *  for waypoints.)
 * 
 *  Apply this script to any permanent gameobject.
 * 
 *  v1.32 -- added to JKScriptPack.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [Header("Object to be spawned")]
    [Tooltip("Best to use a prefab")]
    public GameObject spawnNew;
    [Tooltip("Seconds before auto-destruction; set to 0 to live forever")]
    public float lifespan = 0;

    [Header("Time between spawns")]
    [Tooltip("Set time gap in seconds")]
    public float minTime = 10;
    public float maxTime = 10;

    [Header("List of spawn points")]
    [Tooltip("List of game objects")]
    public GameObject[] spawnPoints;

    private float countdown;

    void Start()
    {
        RestartCountdown();
    }

    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            Spawn();
        }
    }

    void RestartCountdown()
    {
        countdown = Random.Range(minTime, maxTime);
    }

    void Spawn()
    {
        if (spawnNew)
        {

            // Choose spawn point
            GameObject point = this.gameObject;
            if (spawnPoints.Length >= 0)
            {
                point = spawnPoints[Random.Range(0, spawnPoints.Length)];
            }

            // Spawn at that point
            if (point)
            {
                GameObject newobject = Instantiate(spawnNew, point.transform.position, point.transform.localRotation);
                if (lifespan > 0)
                {
                    Destroy(newobject, lifespan);
                }
            }

        }
        RestartCountdown();
    }

}
