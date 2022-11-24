/*
 *	OnTriggerReveal.js
 * 
 *	Reveals an object (usually a UI object, like instructions
 *	for the player) whenever the first person controller
 *	enters a trigger zone.
 *
 *	On exit, another object can be activated, like a door trigger
 *	or a trigger for another puzzle clue.
 *
 *	Attach this script to the trigger zone.
 *
 *  v1.43   Quick fix to include delay
 * 
 */
 
#pragma strict
 
var revealObject: GameObject;
var delay : float = 0;
var revealPermanently : boolean;
var revealSound : AudioClip;
var timeout : float = 0;
var key : KeyCode;
var unrevealSound : AudioClip;
var activateObject : GameObject;

private var inTriggerZone : boolean = false;
private var delayCountdown: float;
private var delaying: boolean = false;
private var countdown : float;
private var a : AudioSource;

function Start () {
	a = gameObject.AddComponent(AudioSource);
	if (revealObject) revealObject.SetActive(false);
    if (activateObject) activateObject.SetActive(false);
    delayCountdown = 0;
	countdown = 0;
}

function Update () {
	if (revealObject && inTriggerZone && Input.GetKeyDown(key)) {
		if (revealObject.activeInHierarchy) {
			Hide();
		} else {
			Show();
		}
    }
    if (delaying) {
        delayCountdown -= Time.deltaTime;
        if (delayCountdown <= 0) {
            delaying = false;
            delayCountdown = 0;
            Reveal();
        }
    }
	if (countdown > 0) {
		countdown -= Time.deltaTime;
		if (countdown <= 0) {
			Hide();
		}
	}
}

function OnTriggerEnter (other : Collider) {
	inTriggerZone = true;
	Show();
}

function OnTriggerExit (other : Collider) {
	inTriggerZone = false;
	Hide();
}

public function Show() {
    delaying = true;
    delayCountdown = delay;
}

public function Reveal() {
    if (revealObject) {
        revealObject.SetActive(true);
        a.PlayOneShot(revealSound);
    }
    countdown = timeout;
}

public function Hide() {
	if (revealObject && !revealPermanently) {
		revealObject.SetActive(false);
		a.PlayOneShot(unrevealSound);
	}
	if (activateObject) {
		activateObject.SetActive(true);
	}
	countdown = 0;
}

