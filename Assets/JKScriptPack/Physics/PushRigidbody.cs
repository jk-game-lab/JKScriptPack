using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// ------------------------------------------
/// <summary>
/// 
///     Pushes all rigidbodies that the 
///     character touches.
///     
///     Attach to first person controller.
///     
/// </summary>
/// <remarks>
/// 
///     Converted from the JavaScript original.
/// 
/// </remarks>
/// ------------------------------------------
public class PushRigidbody : MonoBehaviour
{

    public float Strength = 2.0f;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {

        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // Calculate push direction from move direction;
        // we only push objects to the sides never up and down
        Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDirection * Strength;

    }


}
