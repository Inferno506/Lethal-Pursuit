﻿using UnityEngine;
using InControl;
using System.Collections;

public class SpaceshipGun : SpaceshipComponent {
	
	public string bulletResourcePath = "Bullets/TestLaz0r";
	
	public float cooldownBetweenShots = 0.1f;
	private float timeUntilCanShoot = 0.0f;
	
	private GameObject cachedBullet;
	public AudioSource guns;
	public AudioClip shot;
	
	
	
	// Use this for initialization
	public override void Start () {
		base.Start();
		
		cachedBullet = Resources.Load(bulletResourcePath, typeof(GameObject)) as GameObject;
		cachedBullet.SetActive(false);
	}

	
	
	// Update is called once per frame
	public override void Update () {
		base.Update();	
	}
	
	
	
	void FixedUpdate() {
		
		if (shooting && timeUntilCanShoot == 0.0f) {
			
			GameObject bulletGameObject = GameObject.Instantiate(
				cachedBullet,
				this.transform.position, 
				spaceshipModel.transform.rotation
			) as GameObject;
			
			Bullet bullet = bulletGameObject.GetComponent<Bullet>();
			bullet.direction = spaceshipModel.transform.forward;
			bullet.sourceSpaceship = spaceship;

			bulletGameObject.SetActive(true);
			
			timeUntilCanShoot = cooldownBetweenShots;
			audio.PlayOneShot(shot);
		}
		else {
			timeUntilCanShoot = Mathf.Max(0.0f, timeUntilCanShoot - Time.deltaTime);
		}
	}
	

	void OnDestroy() {
		cachedBullet.SetActive(true);
	}
	
	
}