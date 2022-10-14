using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JKScriptPack
{

    /// <summary>
    /// Information about JKScriptPack.
    /// </summary>
    public class About : MonoBehaviour
    {

        private const string VERSION = "2.0";

        /// <summary>
        /// Displays into summary at game start.
        /// </summary>
        void Start()
        {
            Debug.Log("JKScriptPack version " + VERSION);
        }

    }

}
