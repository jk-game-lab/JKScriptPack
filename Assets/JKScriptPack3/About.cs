using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JKScriptPack3
{

    /// <summary>
    /// Provides information about the JKScriptPack.
    /// </summary>
    public class About : MonoBehaviour
    {

        public const string Version = "3.00";

        [Tooltip("Display the version number in the console when the game starts.")]
        public bool ShowVersion = false;

        /// <summary>
        /// On game start, reports the current pack version.
        /// </summary>
        void Start()
        {
            if (ShowVersion)
            {
                Debug.Log("JKScriptPack version " + Version);
            }
        }

    }

}
