﻿using UnityEngine;
using System;
using System.Collections;

public class SpaceshipComponent : MonoBehaviour {


	public Spaceship spaceship; 


	/* Tilt of analogue stick every frame. */
	protected float xTilt {
		get {
			return spaceship.xTilt;
		}
	}
	protected float yTilt {
		get {
			return spaceship.yTilt;
		}
	} 
	/* Amount of boost button pressed down. */
	protected float boostAmount {
		get {
			return spaceship.boostAmount;
		}
	}
	/* Amount of brake button pressed down. */
	protected float brakeAmount {
		get {
			return spaceship.brakeAmount;
		}
	}
	/* Is player hitting the shoot button right now? */
	protected bool currentlyShooting {
		get {
			return spaceship.currentlyShooting;
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
	protected float heightLimit {
		get {
			return spaceship.heightLimit;
		}
	}
	protected float maxHeightBeforeFalling {
		get {
			return spaceship.maxHeightBeforeFalling;
		}
	}
	protected float fractionOfHeightLimitToBeginSputtering {
		get {
			return spaceship.fractionOfHeightLimitToBeginSputtering;
		}
	}
	protected float heightAboveGround {
		get {
			return spaceship.heightAboveGround;
		}
	}
	protected float fallingRate {
		get {
			return spaceship.fallingRate;
		}
	}
	protected bool enforceHeightLimit {
		get {
			return spaceship.enforceHeightLimit;
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
