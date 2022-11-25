/*
 *  ScoreDisplay.cs
 * 
 *  Attach this script to any gameobject in a scene
 *  (preferably the UI Text gameobject which displays the score)
 *
 *  This script reads the score held in GlobalScore.cs
 *
 *  v1.40 -- added to JKScriptPack
 *	
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{

    public Text textObject;
    public string prefix = "Score: ";

    void Reset()
    {

        // If attached to a text object, auto-link to itself
        Text myowntext = this.GetComponent<Text>();
        if (myowntext)
        {
            textObject = myowntext;
        }

    }

    void Update()
    {
        if (textObject)
        {
            string intro = "";
            if (prefix.Length > 0)
            {
                intro = prefix + " ";
            }
            textObject.text = intro + GlobalScore.score.ToString();
        }
    }

}
