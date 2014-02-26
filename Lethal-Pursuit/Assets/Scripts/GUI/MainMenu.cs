﻿using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	private static string gameType = "CS354T-Galacticats-LP";

	public GameObject MainPanel;
	public GameObject OptionsPanel;
	public GameObject ModeSelectPanel;
	public GameObject MultiplayerPanel;
	public GameObject VehicleSelectPanel;
	public GameObject MapSelectPanel;
	public GameObject LobbyPanel;
	
	public UIButton StartServerButton;
	public UIButton JoinServerButton;
	public UIButton RefreshButton;
	public UIButton LaunchButton;

	public  GameObject[] ServerButtons;
	public  UILabel[] 	 ButtonLabels;
	private HostData[]   hostdata;
	
	private bool refreshClicked = false;
	private static int lastLevelPrefix;

	private bool serverStarted = false;
	private string chosenShip  = null;
	private string chosenLevel = null;
	
	public void Start() {
		MainPanel.SetActive(true);
		OptionsPanel.SetActive(false);
		ModeSelectPanel.SetActive(false);
		MultiplayerPanel.SetActive(false);
		VehicleSelectPanel.SetActive(false);
		MapSelectPanel.SetActive(false);
		LobbyPanel.SetActive(false);
		
		StartServerButton.isEnabled = true;
		JoinServerButton.isEnabled  = true;
		RefreshButton.isEnabled     = false;
		LaunchButton.isEnabled      = false;

		for(int i=0; i<ServerButtons.Length; i++) {
			ServerButtons[i].SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (NetworkManager.IsServerListReady() && refreshClicked) {
			Debug.Log("Refreshing......");
			refreshClicked = false;
			OnServerListReady();	
		}
	}

	public void OnExitClick() {
		Debug.Log("Exit Clicked");
		LevelManager.Quit();
	}
	
	public void OnOptionsClick() {
		Debug.Log("Options Clicked");
		MainPanel.SetActive(false);
		OptionsPanel.SetActive(true);
	}
	
	public void OnStartClick() {
		Debug.Log("Start Clicked");
		MainPanel.SetActive(false);
		ModeSelectPanel.SetActive(true);
	}
	
	public void OnMultiplayerClick() {
		Debug.Log("Multiplayer Clicked");
		NetworkManager.SetSinglePlayer(false);
		
		ModeSelectPanel.SetActive(false);
		MultiplayerPanel.SetActive(true);
	}
	
	public void OnSingleplayerClick() {
		Debug.Log("Singleplayer Clicked");
		NetworkManager.SetSinglePlayer(true);
	}
	
	public void OnTutorialClick() {
		Debug.Log("Tutorial Clicked");
		chosenShip = "Spaceships/Spaceship02";
		NetworkManager.SetShip(chosenShip);
		LevelManager.LoadLevel(LevelManager.LEVEL.TUTORIAL);
	}
	
	public void OnReturnClick() {
		MainPanel.SetActive(true);
		OptionsPanel.SetActive(false);
		ModeSelectPanel.SetActive(false);
		MultiplayerPanel.SetActive(false);
		VehicleSelectPanel.SetActive(false);
		MapSelectPanel.SetActive(false);
		LobbyPanel.SetActive(false);
		
		JoinServerButton.isEnabled = true;
		RefreshButton.isEnabled = false;
		
		if (serverStarted) {
			NetworkManager.ServerCleanup();
		}
		
		serverStarted = false;
	}

	public void OnStartServerClick() {
		NetworkManager.StartServer();
		JoinServerButton.isEnabled = false;
		
		MultiplayerPanel.SetActive(false);
		MapSelectPanel.SetActive(true);
		
		LaunchButton.isEnabled = true;
		serverStarted = true;
	}

	public void OnJoinServerClick() {
		RefreshButton.isEnabled = true;

		NetworkManager.RefreshHostList();
		refreshClicked = true;
		
		LaunchButton.isEnabled = false;
	}
	
	private void OnServerListReady() {
	
		hostdata = NetworkManager.GetHostData();
		
		for (int i=0; i<ServerButtons.Length; ++i) {
			ServerButtons[i].SetActive(false);
		}
		
		for (int i=0; i<hostdata.Length && i<ServerButtons.Length; ++i) {
			ServerButtons[i].SetActive(true);
			ButtonLabels[i].text = hostdata[i].gameName;
		}
	}

	public void OnRefreshClick() {
		NetworkManager.RefreshHostList();
		refreshClicked = true;
	}

	public void OnServer1Click() {
		NetworkManager.JoinServer(0);
		
		MultiplayerPanel.SetActive(false);
		VehicleSelectPanel.SetActive(true);
	}

	public void OnServer2Click() {
		NetworkManager.JoinServer(1);
		
		MultiplayerPanel.SetActive(false);
		VehicleSelectPanel.SetActive(true);
	}

	public void OnServer3Click() {
		NetworkManager.JoinServer(2);
		
		MultiplayerPanel.SetActive(false);
		VehicleSelectPanel.SetActive(true);
	}

	public void OnServer4Click() {
		NetworkManager.JoinServer(3);
		
		MultiplayerPanel.SetActive(false);
		VehicleSelectPanel.SetActive(true);
	}
	
	public void OnVehicle1Click() {
		
		//Record ship name here
		chosenShip = "Spaceships/Buzz";
		NetworkManager.SetShip(chosenShip);

		VehicleSelectPanel.SetActive(false);
		LobbyPanel.SetActive(true);
	}
	
	public void OnVehicle2Click() {
		
		//Record ship name here
		chosenShip = "Spaceships/Magneto II";
		NetworkManager.SetShip(chosenShip);

		VehicleSelectPanel.SetActive(false);
		LobbyPanel.SetActive(true);
	}
	
	public void OnVehicle3Click() {
		
		//Record ship name here
		chosenShip = "Spaceships/Patriot 69Z";
		NetworkManager.SetShip(chosenShip);

		VehicleSelectPanel.SetActive(false);
		LobbyPanel.SetActive(true);
	}
	
	public void OnMap1Click() {
		
		//Record level name here
		chosenLevel = "Tutorial";

		MapSelectPanel.SetActive(false);
		VehicleSelectPanel.SetActive(true);
	}
	
	public void OnMap2Click() {
		
		//Record level name here
		chosenLevel = "Tutorial";

		MapSelectPanel.SetActive(false);
		VehicleSelectPanel.SetActive(true);
	}

	public void OnLaunchClick() {
		networkView.RPC("LevelLoader", RPCMode.All);
		//LevelManager.LoadLevel(LevelManager.LEVEL.TUTORIAL);
	}
	
	[RPC]
	private void LevelLoader() {
		LevelManager.NetworkLoadLevel(LevelManager.LEVEL.TUTORIAL, 1);	
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
}
