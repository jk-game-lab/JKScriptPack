/* NO LONGER SUPPORTED BY UNITY -- needs to be re-written in C# */


#pragma strict

public static var score : int;
public var display : UI.Text;

function Start () {
	score = 0;
}

function Update () {
	display.text = score.ToString();
}