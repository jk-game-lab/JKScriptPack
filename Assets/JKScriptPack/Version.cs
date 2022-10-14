using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JKScriptPack
{

    /// <summary>
    /// Report the version info for JKScriptPack.
    /// </summary>
    public class Version : MonoBehaviour
    {

        private const string VERSION = "2.00";

        /// <summary>
        /// Displays summary at game start.
        /// </summary>
        void Start()
        {
            Debug.Log("JKScriptPack version " + VERSION);
        }

    }

}
