using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JKScriptPack
{

    /// <summary>
    /// Provides information about the JKScriptPack.
    /// </summary>
    public class About : MonoBehaviour
    {

        private const string VERSION = "2.0";

        /// <summary>
        /// On game start, reports the current pack version.
        /// </summary>
        void Start()
        {
            Debug.Log("JKScriptPack version " + VERSION);
        }

    }

}
