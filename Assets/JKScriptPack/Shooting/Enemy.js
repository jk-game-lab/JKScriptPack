/*
 * 	Enemy.js
 * 
 * 	Attach this to an enemy who is getting hit by bullets.
 *	
 *	(This script was quickly written for a game and the name has stuck;
 *	it will be changed to something more descriptive in a future version
 *	of JKScriptPack.)

 * 	v1.00 -- added to JKScriptPack
 *	v1.34 -- fixed to work with different types of object.
 *
 */

#pragma strict

public var bullet : GameObject;
public var destroyBullet : boolean = true;
public var destroyMe : boolean = true;

function Start () {

}

function Update () {

}

function OnTriggerEnter(other : Collider) {
//Debug.Log("OnTrigger");
	HitBy(other.gameObject);
}

function OnCollisionEnter( other : Collision ) {
//Debug.Log("OnCollision");
	HitBy(other.gameObject);
}

function HitBy(other : GameObject) {
	var impact : boolean = false;
	if (bullet) {
		if (other.name == bullet.name || other.name == (bullet.name + "(Clone)")) {
			impact = true;
		}
	} else {
		if (other.name == "Bullet" || other.name == ("Bullet(Clone)")) {
			impact = true;
		}
		if (other.name == "Sphere" || other.name == ("Sphere(Clone)")) {		// Compatibility with old shooter scripts
			impact = true;
		}
	}
	if (impact) {
		if (destroyBullet) {
			Destroy(other);	
		}
		if (destroyMe) {
			Destroy(this.gameObject);
		}
		if (Scoreboard) {
			Scoreboard.score = Scoreboard.score + 10;
		}
	}
}
