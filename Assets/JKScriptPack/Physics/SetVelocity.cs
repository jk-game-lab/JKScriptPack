/*
 *  SetVelocity.js
 *
 *  Attach to a rigidbody gameobject to set its initial velocity.
 *
 *  v1.32 -- added to JKScriptPack.
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVelocity : MonoBehaviour
{

    public Vector3 velocity;

    private Rigidbody myRigidbody;

    void Reset()
    {
        velocity = new Vector3(0, 0, 10);
    }

    void Start()
    {
        myRigidbody = this.GetComponentInChildren<Rigidbody>();
        if (!myRigidbody)
        {
            myRigidbody = new Rigidbody();
        }
        myRigidbody.velocity = velocity;
    }

}
