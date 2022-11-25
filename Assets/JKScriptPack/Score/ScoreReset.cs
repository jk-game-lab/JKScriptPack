/*
 *  ScoreReset.cs
 * 
 *  Attach this script to any gameobject in a scene
 *  to wipe the score when this level starts.
 *
 *  This script updates the score held in GlobalScore.cs
 *
 *  v1.42 -- added to JKScriptPack
 *	
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreReset : MonoBehaviour
{

    void Start()
    {
        GlobalScore.score = 0;
    }

}
