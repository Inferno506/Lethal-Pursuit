﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class AuroraCannon : Pickup {

	public PlaygroundParticles laserBeamEffect;
	public float damageRate = 30.0f;
	public float laserHitForce = 1000.0f;
	public float currentEnergy = 100f;
	public float energyDrainRate = 50.0f;
	public float laserLength = 500.0f;
	private GameObject hitGameObject;
	

	public override void Start() {
	}


	public override void Update() {
		if (spaceship != null && spaceship.shooting) {
			currentEnergy -= energyDrainRate*Time.deltaTime;
			laserBeamEffect.emissionRate = 1.0f;	
			
			Ray ray = new Ray(spaceship.spaceshipModel.transform.position, spaceship.spaceshipModel.transform.forward);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit)) {
				Debug.Log("ray hit: " + hit.collider.gameObject);
				
				/* Apply laser force. */
				if (hit.rigidbody != null) {
					hitGameObject = hit.collider.gameObject;
					hit.rigidbody.AddForceAtPosition(ray.direction*laserHitForce, hit.point);
				}
			}
			else {
			}
		}
		else {
			hitGameObject = null;
			laserBeamEffect.emissionRate = 0.0f;
		}
	}


	void FixedUpdate() {
		if (hitGameObject == null) {
			return;
		}
		IDamageable damageableObject = (IDamageable)hitGameObject.GetComponent(typeof(IDamageable));

		if (damageableObject != null) {
			damageableObject.ApplyDamage(damageRate, hitGameObject, gameObject.name + " is calling ApplyDamage()!");
		}
	}


	public override bool ShouldDrop() {
		return currentEnergy <= 0.0f;
	}


	public override void OnPickup (Spaceship spaceship) {
		base.OnPickup (spaceship);
		spaceship.DisableGun();
		this.transform.localPosition = Vector3.zero;
		laserBeamEffect.transform.localScale = Vector3.one;
		laserBeamEffect.sourceTransform = spaceship.spaceshipModel.transform;
	}


	public override void OnDrop() {
		spaceship.EnableGun();
	}
	
}
