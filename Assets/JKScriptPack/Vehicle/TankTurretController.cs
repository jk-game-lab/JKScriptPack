/*
 *  TankTurretController.cs
 *
 *  Attach to a gameobject to make it move like a tank turret.
 *
 *  v1.26 -- added to JKScriptPack.
 *  v1.29 -- added keyboard control.
 *
 *  NEEDS TO BE UPDATED TO WORK WITH NEW INPUT SYSTEM
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTurretController : MonoBehaviour
{

    public bool mouseX = true;
    public bool mouseY = true;

    public KeyCode rotateLeft = KeyCode.None;
    public KeyCode rotateRight = KeyCode.None;
    public KeyCode rotateUp = KeyCode.None;
    public KeyCode rotateDown = KeyCode.None;

    public float maxUp = 45.0f;
    public float maxDown = 5.0f;

    public float maxLeft = 90.0f;
    public float maxRight = 90.0f;

    public float turretSpeed = 90.0f;

    private float leftRight;
    private float upDown;

    void Start()
    {

        maxUp = Mathf.Abs(maxUp);
        maxDown = Mathf.Abs(maxDown);
        maxLeft = Mathf.Abs(maxLeft);
        maxRight = Mathf.Abs(maxRight);

        leftRight = 0.0f;
        upDown = 0.0f;

    }

    void Update()
    {

        if (mouseX)
        {
            leftRight += Input.GetAxis("Mouse X") * turretSpeed * Time.deltaTime;
        }
        if (Input.GetKey(rotateLeft))
        {
            leftRight -= turretSpeed * Time.deltaTime;
        }
        if (Input.GetKey(rotateRight))
        {
            leftRight += turretSpeed * Time.deltaTime;
        }
        leftRight = Mathf.Clamp(leftRight, -maxLeft, maxRight);

        if (mouseY)
        {
            upDown += Input.GetAxis("Mouse Y") * turretSpeed * Time.deltaTime;
        }
        if (Input.GetKey(rotateUp))
        {
            upDown += turretSpeed * Time.deltaTime;
        }
        if (Input.GetKey(rotateDown))
        {
            upDown -= turretSpeed * Time.deltaTime;
        }
        upDown = Mathf.Clamp(upDown, -maxDown, maxUp);

        transform.localRotation = Quaternion.Euler(-upDown, leftRight, 0);

    }

}
