using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Spaceship))]
[RequireComponent (typeof (Collider))]
public class SpaceshipControl : SpaceshipComponent {
	

	public float acceleration = 5.0f; 
	public float extraAccelerationY = 5.0f; 
	
	public float deaccelerationBrake = 500;
	public float deaccelerationDrift = 50;
	public float deaccelerationIdle = 500;


	public float xTiltSpeed = 1.5f;
	public float yTiltSpeed = 3.3f;
	

	public float normalTurningRate = 115.0f;
	public float driftingTurningRate = 300.0f;
	public float nosedivingRate = 2.75f;
	
	
	public float timeUntilMaxTurning = 2.6f;
	private float timeSinceStartedTurning = 0.0f;

	public Crosshairs crosshairs;
	public float lookSpeed = 1.0f;
	
	
	// Use this for initialization
	public override void Start () {
		base.Start();
	}
	
	
	
	
	
	public override void Update () {
		base.Update();
	}
	
	
	
	
	// This happens at a fixed timestep
	void FixedUpdate () {
		HandleRotation();
		HandleMovement();
		HandleTilt();
		HandleFalling();
	}

	
	
	
	
	
	void HandleMovement() {

		/* Forward boost. */
		if (drifting || nosediving) {
			currentVelocity -= deaccelerationDrift;
		}
		else if (boosting) {
			currentVelocity += acceleration;
		}
		else if (braking) {
			currentVelocity -= deaccelerationBrake;
		}
		else if (idle) {
			currentVelocity -= deaccelerationIdle;
		} 

		currentVelocity = Mathf.Clamp(currentVelocity, 0f, maxVelocity);

		/* Boost forward. */
		rigidbody.MovePosition(
			rigidbody.position + Vector3.Slerp(Vector3.zero, forward*Time.deltaTime*currentVelocity, currentVelocity/maxVelocity)
		);

		/* Additional boost up/down. */
		rigidbody.MovePosition(
			rigidbody.position + Vector3.Slerp(Vector3.zero, Vector3.up*yTilt*Time.deltaTime*extraAccelerationY, currentVelocity/maxVelocity)
		);

	}
	
	
	


	
	void HandleTilt() {

		Vector3 newDirection = Vector3.Slerp(
			spaceshipModel.transform.forward, 
			crosshairs.transform.position-spaceshipModel.transform.position, 
			Time.deltaTime*lookSpeed
		);

		spaceshipModel.transform.forward = newDirection;

	}
	
	
	
	
	
	
	void HandleRotation() {

		if (xTilt != 0) {
			
			float turningRateForThisFrame = normalTurningRate;
			
			/* Allow drift turning if player is holding down brake. */
			if (drifting) {
				turningRateForThisFrame = driftingTurningRate;
			}
			
			//			Debug.Log("turningRate: " + turningRateForThisFrame);
			
			this.rigidbody.MoveRotation(
					Quaternion.Slerp (
					this.transform.localRotation,
					Quaternion.Euler(this.transform.localRotation.eulerAngles + Vector3.up*xTilt*turningRateForThisFrame*Time.deltaTime),
					Mathf.Clamp01(timeSinceStartedTurning/timeUntilMaxTurning)
				)
			);
			timeSinceStartedTurning += Time.deltaTime;
		}
		else {
			timeSinceStartedTurning = 0.0f;
		}
		
	}






	void HandleFalling() {

		if (enforceHeightLimit && spaceship.transform.position.y > maxHeightBeforeFalling) {
			rigidbody.MovePosition(
				Vector3.Slerp(
					rigidbody.position,
					rigidbody.position + Vector3.down*Time.deltaTime*fallingRate,
					Time.deltaTime
				)
			);
		}
	}
	
	
	
}