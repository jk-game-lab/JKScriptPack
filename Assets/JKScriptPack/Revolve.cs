using UnityEngine;
using System.Collections;

/// ------------------------------------------
/// <summary>
/// 
///     Makes an object revolve around 
///     its pivot point.
///     
///     Attach this script to the gameobject
///     that needs to revolve.
///     
/// </summary>
/// ------------------------------------------
public class Revolve : MonoBehaviour
{

    [Tooltip("Rotational speed, measured in revolutions per second.")]
    public float speed = 1;

    [Tooltip("The axis that the object will rotate around.")]
    public Vector3 axis = new Vector3(0, 1, 0);

    void Update()
    {
        transform.Rotate(axis, speed * 360 * Time.deltaTime);
    }

}
