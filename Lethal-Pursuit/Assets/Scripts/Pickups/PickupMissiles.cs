﻿using UnityEngine;
using System.Collections;

public class PickupMissiles : Pickup {
//
//	public PlaygroundParticles laserBeamEffect;
//	public PlaygroundPresetLaser laserBeamEffectScript;

	
	private GameObject hitGameObject;

	public bool alreadyStartedVolley = false;
	public int maxShots = 5;
	public int  currentShots;
	public float cooldownBetweenShots = 0.3f;
	private float timeUntilCanShoot = 0.0f;

	public string missileResourcePath = "Bullets/PunkMissileBullet";

//	private GameObject cachedBullet;
	public AudioClip shootingSound;

	public Vector3 spawnOffsetFromGun = new Vector3(0.0f, 0.0f, 10.0f);
	public bool enabled = true;
	


	void FixedUpdate() {
		if (spaceship.shooting) {
			alreadyStartedVolley = true;
		}
		if (!alreadyStartedVolley || !active) {
			return;
		}

		timeUntilCanShoot = Mathf.Max(0.0f, timeUntilCanShoot - Time.deltaTime);
		
		if (timeUntilCanShoot == 0.0f && currentShots > 0) {
			SpawnMissile();
			timeUntilCanShoot = cooldownBetweenShots;
			--currentShots;
		}
	}


	public void SpawnMissile() {
		GameObject missileGameObject;
		if (NetworkManager.IsSinglePlayer()) {
			missileGameObject = GameObject.Instantiate(
				Resources.Load(missileResourcePath),
				spaceship.gun.transform.position + spaceship.gun.transform.TransformDirection(spawnOffsetFromGun), 
				spaceship.gun.transform.rotation
			) as GameObject;
		}
		else {
			missileGameObject = Network.Instantiate(
				Resources.Load(missileResourcePath),
				spaceship.gun.transform.position + spaceship.gun.transform.TransformDirection(spawnOffsetFromGun), 
				spaceship.gun.transform.rotation,
				667
			) as GameObject;
		}
		
		Missile missile = missileGameObject.GetComponent<Missile>();
		missile.direction = spaceship.spaceshipModelPitchYaw.transform.forward;
		missile.sourceSpaceship = spaceship;
		
		missileGameObject.SetActive(true);
		spaceship.gun.audio.PlayOneShot(shootingSound);
	}


	public override bool IsEquippable() {
		return true;
	}


	public override bool ShouldDrop() {
		return currentShots <= 0;
	}


	public override void OnPickup (Spaceship spaceship) {
		base.OnPickup (spaceship);
		currentShots = maxShots;
		active = true;
	}


	public override void OnDrop() {

	}
	
}
