using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
/// <remarks>
/// 
///		This pack is now in "maintenance mode".
///		No further scripts will be added; 
///		new releases of this pack will contain
///		bug fixes only.
///		
///		It will eventually be replaced by 
///		JKScriptPack2, which is currently in
///		development.
/// 
/// </remarks>
/// ------------------------------------------
public class Version_Number : MonoBehaviour
{

    [Tooltip("Current version number of the script pack")]
    public const string Version = "1.90";

    void Start()
    {
        Debug.Log("JKScriptPack version " + Version);
    }

}
