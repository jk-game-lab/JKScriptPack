using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JKScriptPack2
{
    /// ------------------------------------------
    /// <summary>
    /// 
    ///     Chases a GameObject.
    ///     
    ///     Attach this to the chaser and set the
    ///     victim to be the first person controller.
    ///     
    /// </summary>
    /// ------------------------------------------
    public class Chaser : MonoBehaviour
    {

        [Header("Chaser")]

        [Tooltip("Detection range")]
        public float Range = 10.0f;

        [Tooltip("Detection field of view (in degrees)")]
        public float Angle = 90;

        [Header("Victim")]

        [Tooltip("Which object is being chased? (Typically first person controller)")]
        public GameObject Victim;

        /*
         * 
         * Break into three parts:
         *  Chase script -- makes something with a NavMesh chase a player (by adding an extra destination to the navmesh temporarily)
         *  WeepingAngel -- detects when the player sees it, and disables the Navmesh agent (freezing the enemy)
         *  Detect when caught script
         * 
         * 
         */



    }
}
