using UnityEngine;
using System.Collections;

/// ------------------------------------------
/// <summary>
/// 
///     Makes an object swing around its pivot.
///     
///     Attach this script to the gameobject
///     that needs to swing.
///     
/// </summary>
/// ------------------------------------------
public class Swing : MonoBehaviour
{

    public float speed = 2;
    public Vector3 axis = new Vector3(0, 0, 1);
    public float angle = 45;

    private Quaternion angleA;
    private Quaternion angleB;
    private float elapsedTime;

    void Start()
    {
        angleA = transform.localRotation * Quaternion.AngleAxis(angle, axis);
        angleB = transform.localRotation * Quaternion.AngleAxis(-angle, axis);
        elapsedTime = 0.0f;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        transform.localRotation = Quaternion.Lerp(angleA, angleB, 0.5f * (1.0f + Mathf.Sin(elapsedTime * speed)));
    }

}
