using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JKScriptPack2
{

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
        public float Speed = 1;

        [Tooltip("The axis that the object will rotate around.")]
        public Vector3 Axis = new Vector3(0, 1, 0);

        void Update()
        {
            transform.Rotate(Axis, Speed * 360 * Time.deltaTime);
        }

    }

}
