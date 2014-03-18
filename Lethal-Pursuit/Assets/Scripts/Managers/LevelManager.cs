﻿using UnityEngine;
using System;
using System.Collections;

public class LevelManager : MonoBehaviour {
	
	private static int lastLevelPrefix;

	private Level loadedLevel;
	private string spaceshipFilename; // Spaceship prefab relative to resources directory.
	private static LevelManager singletonInstance;

	private bool showLoadingScreen = true;
	private UIPanel loadingScreen;
	private string loadingScreenFilename = "GUI/LoadingScreen"; // Loading screen prefab relative to resources directory.
	public float loadingScreenFadeTime = 0.75f;

	private static LevelManager instance {
		get {
			/* If first time accessing instance, then find it... */
			if (singletonInstance == null) {
				singletonInstance = FindObjectOfType(typeof (LevelManager)) as LevelManager;
			}
			
			/* If instance is null, then no LevelManager exists in the scene, so create one. */
			if (singletonInstance == null) {
				GameObject obj = new GameObject("LevelManager");
				singletonInstance = obj.AddComponent(typeof (LevelManager)) as LevelManager;
				obj.name = "Level Manager";
				singletonInstance.loadedLevel = GetLoadedLevel();
				Debug.Log ("Loaded level on LevelManager creation: " + singletonInstance.loadedLevel);
				//Debug.Log ("Could not find a LevelManager object, so automatically generated one.");
			}
			
			return singletonInstance;
		}
	}

	
	public void Awake() {
		DontDestroyOnLoad(this);

		GameObject loadingScreenGameObject = GameObject.Instantiate(
			Resources.Load (loadingScreenFilename)
		) as GameObject;

		if (loadingScreenGameObject == null) {
			throw new Exception("LevelManager: Error creating loading screen!");
		}
		else {
			loadingScreen = loadingScreenGameObject.GetComponent<UIPanel>();
			DontDestroyOnLoad(loadingScreenGameObject);
			loadingScreen.transform.parent = this.transform;
			loadingScreenGameObject.name = "Loading Screen";
			loadingScreen.alpha = 0.0f;
		}
	}


	private static void ShowLoadingScreen() {
		Debug.Log ("LevelManager: Showing loading screen...");
		if (instance.loadingScreen == null) {
			return;
		}
		if (instance.loadingScreen.GetComponent<TweenAlpha>() != null) {
			GameObject.DestroyImmediate(instance.loadingScreen.GetComponent<TweenAlpha>());
		}

		instance.loadingScreen.alpha = 1.0f; // For now, just set alpha to 0 until I can discover why code below doesn't always activate.
		return; 

		TweenAlpha alphaTween = instance.loadingScreen.gameObject.AddComponent<TweenAlpha>();
		alphaTween.from = 0.0f;
		alphaTween.to = 1.0f;
		alphaTween.duration = instance.loadingScreenFadeTime;
		alphaTween.animationCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, instance.loadingScreenFadeTime, 1.0f);
		alphaTween.Play();
//		GameObject.Destroy(alphaTween, instance.loadingScreenFadeTime);
	}


	private static void HideLoadingScreen() {
		Debug.Log ("LevelManager: Hiding loading screen...");
		if (instance.loadingScreen == null) {
			return;
		}
		if (instance.loadingScreen.GetComponent<TweenAlpha>() != null) {
			GameObject.DestroyImmediate(instance.loadingScreen.GetComponent<TweenAlpha>());
		}

		instance.loadingScreen.alpha = 0.0f; // For now, just set alpha to 0 until I can discover why code below doesn't always activate.
		return; 

		TweenAlpha alphaTween = instance.loadingScreen.gameObject.AddComponent<TweenAlpha>();
		alphaTween.from = 1.0f;
		alphaTween.to = 0.0f;
		alphaTween.duration = instance.loadingScreenFadeTime;
		alphaTween.animationCurve = AnimationCurve.EaseInOut(0.0f, 1.0f, instance.loadingScreenFadeTime, 0.0f);
		alphaTween.Play();
//		GameObject.Destroy(alphaTween, instance.loadingScreenFadeTime);
	}


	/* Spaceship filename is relative to resources folder. */
	public static void SetSpaceship(string filename) {
		Debug.Log("LevelManager: Setting spaceship filename: " + filename);
		instance.spaceshipFilename = filename;
	}


	public Level GetLevel(string levelName) {
		
		if (levelName.Equals("MainMenu")) {
			return new LevelMainMenu();
		}
		if (levelName.Equals("Tutorial")) {
			return new LevelTutorial();
		}
		if (levelName.Equals("Highway")) {
			return new LevelHighway();
		}
		else {
			throw new NotImplementedException("LevelManager: Level '" + levelName + "' is either not known or programmed in yet!");
		}
	}
	

	public static Level GetLoadedLevel() {
		if (instance.loadedLevel == null) {
			instance.loadedLevel = instance.GetLevel(Application.loadedLevelName);
		}
		return instance.loadedLevel;
	}


	public static bool IsLoadedLevelName(string levelName) {
		Debug.Log ("loadedLevel: " + instance.loadedLevel);
		return instance.loadedLevel.name.Equals(levelName);
	}


	public static bool IsMainMenu() {
		return IsLoadedLevelName("MainMenu");
	}
	

	public static void LoadLevel(string levelName, bool showLoadingScreen = true) {
		Level levelToLoad = instance.GetLevel(levelName);
		Debug.Log("LevelManager: Loading level: " + levelToLoad);
		instance.showLoadingScreen = showLoadingScreen;
		instance.StartCoroutine(LoadLevelHelper(levelToLoad));
	}


	private static IEnumerator LoadLevelHelper(Level levelToLoad) {
		if (instance.showLoadingScreen) {
			ShowLoadingScreen();
			yield return new WaitForSeconds(instance.loadingScreenFadeTime);
		}
		Application.LoadLevel(levelToLoad.sceneName);
		yield break;
	}

	
	public static void ReloadLevel() {
		Debug.Log("LevelManager: Reloading level: " + instance.loadedLevel);
		Application.LoadLevel(instance.loadedLevel.sceneName);
	}


	public static void NetworkLoadLevel(string levelName, int levelPrefix) {
		LevelManager.instance.StartCoroutine(NetworkLoadLevelHelper(levelName, levelPrefix));
	}


	private static IEnumerator NetworkLoadLevelHelper(string levelName, int levelPrefix, bool showLoadingScreen = true) {
		instance.showLoadingScreen = showLoadingScreen;

		if (instance.showLoadingScreen) {
			ShowLoadingScreen();
		}
		yield return new WaitForSeconds(instance.loadingScreenFadeTime);

		Debug.Log("LevelManager: Loading level " + levelName + " with prefix " + levelPrefix);
		lastLevelPrefix = levelPrefix;
		
		// There is no reason to send any more data over the network on the default channel,
		// because we are about to load the level, thus all those objects will get deleted anyway
		Network.SetSendingEnabled(0, false);   
		
		// We need to stop receiving because first the level must be loaded.
		// Once the level is loaded, RPC's and other state update attached to 
		// objects in the level are allowed to fire
		Network.isMessageQueueRunning = false;
		
		// All network views loaded from a level will get a prefix into their NetworkViewID.
		// This will prevent old updates from clients leaking into a newly created scene.
		Network.SetLevelPrefix(levelPrefix);
		
		Level levelToLoad = instance.GetLevel(levelName);
		Debug.Log("LevelManager: Loading level: " + levelToLoad);
		Application.LoadLevel(levelToLoad.sceneName);
		
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		Debug.Log("Loading complete");
		
		Debug.Log("load level DONE");
		// Allow receiving data again
		Network.isMessageQueueRunning = true;
		// Now the level has been loaded and we can start sending out data
		Network.SetSendingEnabled(0, true);
		
		Debug.Log("LevelManager: sending load msg");
		// Notify our objects that the level and the network is ready
		foreach (GameObject go in FindObjectsOfType(typeof(GameObject)) ) {
			Debug.Log("LevelManager: sending load msg");
			go.SendMessage("LevelManager: OnNetworkLoadedLevel", levelToLoad, SendMessageOptions.DontRequireReceiver);  
		}
		
	}


	public static void LoadMainMenu(bool showLoadingScreen = true) {
		if (!NetworkManager.IsSinglePlayer()) {
			if (Network.isClient) {
				Network.Disconnect();
			}
			else {
				NetworkManager.ServerCleanup();
			}
		}
		LoadLevel("MainMenu", showLoadingScreen);
	}
	

	public static void Quit() {
		Application.Quit();
	}


	void OnLevelWasLoaded(int levelNumber) {
	
		Level level = GetLevel(Application.loadedLevelName);		
		Debug.Log("LevelManager: OnLevelWasLoaded() for Level: " + level);

		try {
			if (!IsMainMenu()) {
				SpawnPlayer();
			}
		}
		catch (Exception) { 
			if (instance.showLoadingScreen) {
				HideLoadingScreen();
			}
			throw;
		}
	}


	void SpawnPlayer() {
		GameObject spaceship = null;

		GameObject [] previousShipsInScene = GameObject.FindGameObjectsWithTag("Spaceship");
		for (int i = 0; i < previousShipsInScene.Length; ++i) {
			previousShipsInScene[i].SetActive(false);
		}

		if (spaceshipFilename == null) {
			throw new Exception("LevelManager: Spaceship filename is null!");
		}
	
		
		if (NetworkManager.IsSinglePlayer()) {
			spaceship = Instantiate(
				Resources.Load (spaceshipFilename),
				Vector3.zero, 
				Quaternion.identity) as GameObject;
		}
		else {
			spaceship = Network.Instantiate(
				Resources.Load (spaceshipFilename),
				Vector3.zero, 
				Quaternion.identity,
				0) as GameObject;
		}
		
		if (NetworkManager.IsSinglePlayer()) {
			// Disable network view if having performance issues.
		}

		
		Checkpoint initialCheckpoint = Checkpoint.GetCheckpointByID(0);
		if (initialCheckpoint == null) {
			throw new Exception("LevelManager: No checkpoint to spawn at! Spawning at world origin...");
		}
		else {
			initialCheckpoint.SpawnSpaceship(spaceship.GetComponent<Spaceship>());
		}

		Debug.Log ("spaceshipFilename: " + spaceshipFilename);
		Debug.Log ("spawned spaceship: " + spaceship);
		Debug.Log ("spawned spaceship id: " + spaceship.gameObject.GetInstanceID());

	}


}
