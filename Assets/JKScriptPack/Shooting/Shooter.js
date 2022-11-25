/* NO LONGER SUPPORTED BY UNITY -- needs to be re-written in C# */


/*
 *	Shooter.js
 *
 *	Attach to a gameobject to allow it to shoot bullets.
 *	Bullets will be spawned at the gameobject's location, so it
 *	should ideally be an invisible object just off the tip of the gun.
 *
 *	IMPORTANT: Do not attach this to an object that has a collider!
 *
 *	v1.00 -- added to JKScriptPack.
 *	v1.26 -- amended to generate bullets automatically.
 *	v1.30 -- added keyboard control and rate of fire.
 *	v1.31 -- made mouse button 'fire' optional.
 *	v1.34 -- changed bullet name.
 *	v1.38 -- fixed bug with bullet rotation at instantiation.
 *	v1.39 -- added sound.
 *
 */

 #pragma strict

public var bullet : GameObject;
public var mouseButtonFire : boolean = true;
public var fireKey : KeyCode;
public var speed : float = 50.0f;
public var maxRateOfFire : float = 1.0f;	// Bullets per second; setting to 0 disables this limit.

private var actualBullet : GameObject;
private var countdown : float = 0.0;

public var sound : AudioClip;
private var audiosource : AudioSource;

function Start () {

		// Make sure bullets exist
		if (bullet) {
			actualBullet = bullet.gameObject;
		} else {
			actualBullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			actualBullet.name = "Bullet";
			actualBullet.transform.position = new Vector3(0, -10, 0);			// Create off-screen
			actualBullet.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
			var rr : Renderer = actualBullet.GetComponent(Renderer);
			rr.material = new Material(Shader.Find("Diffuse"));
			rr.material.color = Color.black;
			var rb : Rigidbody = actualBullet.AddComponent(Rigidbody);
			rb.useGravity = false;
			rb.isKinematic = false;
			rb.constraints = RigidbodyConstraints.FreezeRotation;
		}

		audiosource = gameObject.AddComponent(AudioSource);

}

function Update () {

	if (countdown > 0.0) {
		countdown -= Time.deltaTime;
	} else if ( Input.GetKeyDown(fireKey) || (mouseButtonFire && Input.GetButtonDown("Fire1")) ) {

		var projectile : GameObject;
		projectile = Instantiate(actualBullet, this.transform.position, this.transform.rotation);
//		projectile = Instantiate(actualBullet, this.transform.position, this.transform.localRotation);
		audiosource.PlayOneShot(sound);

		projectile.GetComponent.<Rigidbody>().velocity = this.transform.forward * speed;
			
		Destroy(projectile, 2.0f);
		
		if (maxRateOfFire > 0) {
			countdown = 1 / maxRateOfFire;
		} else {
			countdown = 0.0;
		}
	}

}

