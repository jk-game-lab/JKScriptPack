/*
 *	GlobalScore.cs
 * 
 *	This script keeps track of score across multiple scenes.
 *	
 *	v1.40 -- added to JKScriptPack
 *	
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalScore : object {

	public static int score = 0;

	public static void Add(int points) {
		score += points;
	}

	public static void Subtract(int points) {
		Add(-points);
	}

}
