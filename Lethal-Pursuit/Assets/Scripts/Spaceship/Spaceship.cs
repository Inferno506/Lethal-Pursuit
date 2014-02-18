﻿using UnityEngine;
using InControl;
using System.Collections;


/* 
	Contains data for a spaceship that other spaceship components may use.
 */
[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (Collider))]
public class Spaceship : MonoBehaviour {

	public GameplayManager gameplayManager;
	public GameObject spaceshipModel;

	#region input variables
	public float xTilt; /* Tilt of analogue stick every frame. */
	public float yTilt; /* Tilt of analogue stick every frame. */
	public float boostAmount;
	public float brakeAmount;
	public bool shooting;
	public bool boosting;
	public bool braking;
	public bool drifting;
	public bool nosediving;
	public bool idle;
	#endregion

	public Vector3 forward;
	public float heightAboveGround;

	public bool enforceHeightLimit = true;
	public float worldHeightLimit = 300.0f;
//	public float fractionOfHeightLimitToBeginSputtering = 0.8f;
//	public float maxHeightBeforeFalling = 500.0f;
//	public float fallingRate = -98.1f;

	public float currentVelocity;
	public float maxVelocity = 150.0f;

	public static bool singlePlayer = true;
	public bool mapKeyboardDiagonalAmountToAnalogueDiagonalAmount = false;




	void Start () {
		gameplayManager = GameplayManager.instance;
	}


	
	void FixedUpdate () {
		if (singlePlayer || networkView.isMine) {
			HandleInput();
		}
	}



	void Update () {
		if (singlePlayer || networkView.isMine) {
			forward = spaceshipModel.transform.forward;
		}
	}



	void HandleInput() {
		xTilt = InputManager.ActiveDevice.LeftStickX.Value;		
		yTilt = InputManager.ActiveDevice.LeftStickY.Value;
		boostAmount = InputManager.ActiveDevice.RightTrigger.Value;
		brakeAmount = InputManager.ActiveDevice.LeftTrigger.Value;
		shooting = InputManager.ActiveDevice.Action3.State;

		braking = false;
		boosting = false;
		drifting = false;
		nosediving = false;

		if (boostAmount > 0 && brakeAmount == 0) {
			boosting = true;
		}
		else if (brakeAmount > 0) {
			drifting = (xTilt != 0);
			nosediving = (yTilt != 0);
			braking = (xTilt == 0 && yTilt == 0);
		}
		else if (boostAmount == 0 && brakeAmount == 0) {
			idle = true;
		}
		
		
		/* Map keyboard diagonal axis amount to joystick diagonal axis amount. */
		if (mapKeyboardDiagonalAmountToAnalogueDiagonalAmount) {
			if (Mathf.Abs(xTilt) > 0.5f && Mathf.Abs(yTilt) > 0.5f) {
				xTilt *= 0.5f;
				yTilt *= 0.5f;
			}
		}
	}
	

	
}
