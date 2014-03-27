﻿using UnityEngine;
using System;
using System.Collections;

public class SpaceshipComponent : MonoBehaviour {


	public Spaceship spaceship; 


	/* Tilt of left analogue stick every frame. */
	protected float xTiltLeft {
		get {
			return spaceship.xTiltLeft;
		}
	}
	protected float yTiltLeft {
		get {
			return spaceship.yTiltLeft;
		}
	} 
	/* Tilt of right analogue stick every frame. */
	protected float xTiltRight {
		get {
			return spaceship.xTiltRight;
		}
	}
	protected float yTiltRight {
		get {
			return spaceship.yTiltRight;
		}
	} 
	/* Amount of boost button pressed down. */
	protected float boostAmount {
		get {
			return spaceship.boostAmount;
		}
	}
	/* Amount of strafe button pressed down. */
	protected float strafeAmount {
		get {
			return spaceship.strafeAmount;
		}
	}
	/* Amount of brake button pressed down. */
//	protected float brakeAmount {
//		get {
//			return spaceship.brakeAmount;
//		}
//	}
	/* Is player hitting the shoot button right now? */
	protected bool shooting {
		get {
			return spaceship.shooting;
		}
	}
	protected bool boosting {
		get {
			return spaceship.boosting;
		}
	}
	protected bool reversing {
		get {
			return spaceship.reversing;
		}
	}
	protected bool strafing {
		get {
			return spaceship.strafing;
		}
	}
	protected bool braking {
		get {
			return spaceship.braking;
		}
	}
	protected bool drifting {
		get {
			return spaceship.drifting;
		}
	}
	protected bool nosediving {
		get {
			return spaceship.nosediving;
		}
	}
	protected bool idle {
		get {
			return spaceship.idle;
		}
	}
	protected GameplayManager gameplayManager {
		get {
			return spaceship.gameplayManager;
		}
	}
	protected GameObject spaceshipModel {
		get {
			return spaceship.spaceshipModel;
		}
	}
	protected bool enforceHeightLimit {
		get {
			return spaceship.enforceHeightLimit;
		}
	}
	protected float heightLimit {
		get {
			return spaceship.heightLimit;
		}
	}
	protected float heightAboveGround {
		get {
			return spaceship.heightAboveGround;
		}
	}
	protected Vector3 forward {
		get {
			return spaceship.forward;
		}
	}
	protected Vector3 right {
		get {
			return spaceship.right;
		}
	}
	protected float currentBoostVelocity {
		get {
			return spaceship.currentBoostVelocity;
		}
		set {
			spaceship.currentBoostVelocity = value;
		}
	}
	protected float maxBoostVelocity {
		get {
			return spaceship.maxBoostVelocity;
		}
	}
	protected float currentStrafeVelocity {
		get {
			return spaceship.currentStrafeVelocity;
		}
		set {
			spaceship.currentStrafeVelocity = value;
		}
	}
	protected float maxStrafeVelocity {
		get {
			return spaceship.maxStrafeVelocity;
		}
	}

	


	// Use this for initialization
	public virtual void Start () {
		if (spaceship == null) {
			throw new Exception("spaceship is null for SpaceshipComponent in " + this.gameObject.name);
		}
	}




	
	// Update is called once per frame
	public virtual void Update () {
		;
	}




}
