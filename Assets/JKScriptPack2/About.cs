using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JKScriptPack2
{

    /// ------------------------------------------
    /// <summary>
    /// 
    ///     Shows information about JKScriptPack.
    ///     
    ///     Attach this script to any gameobject
    ///     to show the pack version number in the
    ///     debug console at game start.
    ///     
    /// </summary>
    /// ------------------------------------------
    public class About : MonoBehaviour
    {

        [Tooltip("Current version number of the script pack")]
        public const string Version = "2.10";

        void Start()
        {
            Debug.Log("JKScriptPack version " + Version);
        }

    }

}
