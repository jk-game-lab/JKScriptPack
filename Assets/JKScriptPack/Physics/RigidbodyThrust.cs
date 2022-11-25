/*
 *  RigidbodyThrust.cs
 *
 *  Makes a rigidbody accelerate when a key is pressed.
 *
 *  Attach this script to a gameobject.
 *
 *  v1.26 -- added to JKScriptPack.
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyThrust : MonoBehaviour
{

    public KeyCode key = KeyCode.None;
    public Vector3 thrust = Vector3.forward;

    private float scaleFactor = 20.0f;
    private Rigidbody myRigidbody;

    void Start()
    {
        myRigidbody = this.GetComponentInChildren<Rigidbody>();
        if (!myRigidbody)
        {
            myRigidbody = new Rigidbody();
        }
    }

    void FixedUpdate()
    {
        if (Input.GetKey(key))
        {
            myRigidbody.AddRelativeForce(thrust * scaleFactor);
        }
    }

}
