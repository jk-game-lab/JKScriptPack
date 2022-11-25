/*
 *  TankController.cs
 *
 *  Attach to a gameobject to make it move like a tank.
 *
 *  v1.29 -- basic version added to JKScriptPack.
 *  v1.31 -- added custom keyboard controls.
 *
 *  NEEDS TO BE UPDATED TO WORK WITH NEW INPUT SYSTEM
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{

    public float forwardSpeed = 10;
    public float reverseSpeed = 5;
    public float turningSpeed = 45;

    public bool nativeControls = true;
    public KeyCode keyForward = KeyCode.None;
    public KeyCode keyBack = KeyCode.None;
    public KeyCode keyLeft = KeyCode.None;
    public KeyCode keyRight = KeyCode.None;

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
        if ((nativeControls && Input.GetAxis("Vertical") > 0) || Input.GetKey(keyForward))
        {
            myRigidbody.MovePosition(transform.position + transform.forward * forwardSpeed * Time.deltaTime);
        }
        if ((nativeControls && Input.GetAxis("Vertical") < 0) || Input.GetKey(keyBack))
        {
            myRigidbody.MovePosition(transform.position - transform.forward * reverseSpeed * Time.deltaTime);
        }
        if ((nativeControls && Input.GetAxis("Horizontal") > 0) || Input.GetKey(keyRight))
        {
            Vector3 axis = new Vector3(0, turningSpeed, 0);
            Quaternion theta = Quaternion.Euler(axis * Time.deltaTime);
            myRigidbody.MoveRotation(myRigidbody.rotation * theta);
        }
        if ((nativeControls && Input.GetAxis("Horizontal") < 0) || Input.GetKey(keyLeft))
        {
            Vector3 axis = new Vector3(0, -turningSpeed, 0);
            Quaternion theta = Quaternion.Euler(axis * Time.deltaTime);
            myRigidbody.MoveRotation(myRigidbody.rotation * theta);
        }
    }

}
