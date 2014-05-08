﻿using InControl;
using UnityEngine;
using System;
using System.Collections;

public class HudManager : MonoBehaviour {

	public UIRoot uiRoot;
	public Spaceship spaceship;

	public bool pressingStart;
	public bool releasedStart;

	private bool pressedStartLastFrame;
	private Level currentLevel;

	private bool isTutorialLevel;

	public GameObject tutorialGui;
	public GameObject menuGui;
	public GameObject controlsGui;
	public GameObject quitConfirmationGui;
	public GameObject matchOverGui;

	public UIButton[] currentPanelButtons;
	public int selectedButtonIndex;

	public Sprite buttonNormalSprite;
	public Sprite buttonHoverSprite;
	public Color buttonNormalTextColor = Color.black;
	public Color buttonHoverTextColor = new Color(0.4784f, 0.5373f, 0.8471f);
	public float buttonNormalOpacity = 0.6f;
	public float buttonTweenDuration = 0.04f;

	// Use this for initialization
	void Start () {
		spaceship = GameplayManager.spaceship;
		currentLevel = LevelManager.GetLoadedLevel();

		isTutorialLevel = currentLevel.name.Equals("Tutorial");
		tutorialGui.SetActive(isTutorialLevel);

		RegisterEventHandlers();
//		ImmediatelyReloadCurrentPanelButtons();
		menuGui.SetActive(false);
		
		
//		HideAllMenus();
	}
	

	
	// Update is called once per frame
	void Update () {
		bool pressedConfirm = InputManager.ActiveDevice.Action1.WasPressed || InputManager.ActiveDevice.GetControl(InputControlType.Start).WasPressed;
		bool releasedConfirm = InputManager.ActiveDevice.Action1.WasReleased || InputManager.ActiveDevice.GetControl(InputControlType.Start).WasReleased;
		bool releasedCancel = InputManager.ActiveDevice.Action2.WasReleased;
		bool releasedUp = InputManager.ActiveDevice.DPadUp.WasReleased;
		bool releasedDown = InputManager.ActiveDevice.DPadDown.WasReleased;
		bool releasedLeft = InputManager.ActiveDevice.DPadLeft.WasReleased;
		bool releasedRight = InputManager.ActiveDevice.DPadRight.WasReleased;
		pressedStartLastFrame = pressingStart;
		pressingStart = InputManager.ActiveDevice.GetControl(InputControlType.Start);
		releasedStart = !pressingStart && pressedStartLastFrame;
		
		if (releasedStart || releasedCancel) {
			if (menuGui.activeInHierarchy) {
				HideMenu();
				StartCoroutine(ReloadCurrentPanelButtons());
				return;
			}
			else if (controlsGui.activeInHierarchy) {
				HideControls();
				StartCoroutine(ReloadCurrentPanelButtons());
				return;
			}
		}
		if (releasedStart) {
			if (!tutorialGui.activeInHierarchy && !controlsGui.activeInHierarchy && !quitConfirmationGui.activeInHierarchy && !matchOverGui.activeInHierarchy) {
				DisplayMenu();
				StartCoroutine(ReloadCurrentPanelButtons());
				return;
			}
		}
		if (releasedCancel) {
			if (quitConfirmationGui.activeInHierarchy) {
				HideQuitConfirmation();
				StartCoroutine(ReloadCurrentPanelButtons());
				return;
			}
		}

		
		if (pressedConfirm) {
			GetSelectedButton().GetComponent<UIWidget>().color = GetSelectedButton().pressed;
			GetSelectedButton().SendMessage("OnPress", true);
		}
		else if (releasedConfirm) {
			GetSelectedButton().SendMessage("OnClick");
		}
		else if (releasedCancel) {
	
			StartCoroutine(ReloadCurrentPanelButtons());
		}
		else if (releasedDown || releasedRight) {
			GetSelectedButton().SendMessage("OnHover", false);
			SelectNextButton();
			GetSelectedButton().SendMessage("OnHover", true);
		}
		else if (releasedUp || releasedLeft) {
			GetSelectedButton().SendMessage("OnHover", false);
			SelectPreviousButton();
			GetSelectedButton().SendMessage("OnHover", true);
		}

	}
	

	// Make this script subscribe to ui events from buttons in the scene
	void RegisterEventHandlers() {
		uiRoot = GameObject.FindGameObjectWithTag("UIRoot").GetComponent<UIRoot>();
		UIButton [] buttons = uiRoot.gameObject.GetComponentsInChildren<UIButton>(true);
		for (int i = 0; i < buttons.Length; ++i) {
			UIButton button = buttons[i];
			Debug.Log ("Adding listeners for button: " + button.gameObject);
			UIEventListener.Get(button.gameObject).onClick += OnButtonClick;
			UIEventListener.Get(button.gameObject).onHover += OnButtonHover;
			
			Color normalColor = button.defaultColor;
			normalColor.a = buttonNormalOpacity;
			button.defaultColor = normalColor;
			button.duration = buttonTweenDuration;
		}
	}
	
	
	public void OnButtonClick(GameObject source) {
		Debug.Log ("Clicked on: " + source);
		StartCoroutine(ReloadCurrentPanelButtons());
	}
	
	
	public void OnButtonHover(GameObject source, bool isOver) {
		
		//		Debug.Log ("source: " + source);
		UIButton button = source.GetComponentInChildren<UIButton>();
		UILabel text = source.transform.parent.GetComponentInChildren<UILabel>();
		
		if (isOver) {
			SetSelectedButton(button);
			button.GetComponent<UI2DSprite>().sprite2D = buttonHoverSprite;
			text.color = buttonHoverTextColor;
			//			Debug.Log ("MainMenu: Got 'isOver=true' OnHover event from: " + source);
		}
		else {
			button.GetComponent<UI2DSprite>().sprite2D = buttonNormalSprite;
			text.color = buttonNormalTextColor;
			
			//			Debug.Log ("MainMenu: Got 'isOver=false' OnHover event from: " + source);
		}
		
		
	}
	
	
	IEnumerator ReloadCurrentPanelButtons() {
		yield return new WaitForEndOfFrame();
		currentPanelButtons = uiRoot.gameObject.GetComponentsInChildren<UIButton>();
		
		Array.Sort(
			currentPanelButtons, 
				(UIButton lhs, UIButton rhs) => {
			return lhs.name.CompareTo(rhs.name);
			}
		);
		
		for (int i = 0; i < currentPanelButtons.Length; ++i) {
			UIButton button = currentPanelButtons[i];
			button.defaultColor = new Color(1.0f, 1.0f, 1.0f, buttonNormalOpacity);
			button.GetComponent<UI2DSprite>().sprite2D = buttonNormalSprite;
			button.transform.parent.GetComponentInChildren<UILabel>().color = buttonNormalTextColor;
		}
		
		selectedButtonIndex = 0;
		GetSelectedButton().SendMessage("OnHover", true);
	}


	void ImmediatelyReloadCurrentPanelButtons() {
		currentPanelButtons = uiRoot.gameObject.GetComponentsInChildren<UIButton>();
		
		Array.Sort(
			currentPanelButtons, 
			(UIButton lhs, UIButton rhs) => {
			return lhs.name.CompareTo(rhs.name);
			}
		);
		
		for (int i = 0; i < currentPanelButtons.Length; ++i) {
			UIButton button = currentPanelButtons[i];
			button.defaultColor = new Color(1.0f, 1.0f, 1.0f, buttonNormalOpacity);
			button.GetComponent<UI2DSprite>().sprite2D = buttonNormalSprite;
			button.transform.parent.GetComponentInChildren<UILabel>().color = buttonNormalTextColor;
		}
		
		selectedButtonIndex = 0;
		GetSelectedButton().SendMessage("OnHover", true);
	}


	
	public void SelectNextButton() {
		++selectedButtonIndex;
		if (selectedButtonIndex >= currentPanelButtons.Length) {
			selectedButtonIndex = 0;
		}
	}
	
	
	public void SelectPreviousButton() {
		--selectedButtonIndex;
		if (selectedButtonIndex < 0) {
			selectedButtonIndex = currentPanelButtons.Length-1;
		}
	}
	
	
	public void SetSelectedButton(UIButton button) {
		Debug.Log ("Button to select: " + button);
		for (int i = 0; i < currentPanelButtons.Length; ++i) {
			if (currentPanelButtons[i] == button) {
				selectedButtonIndex = i;
				return;
			}
		}
		Debug.LogError("Couldn't set '" + button + "' as selected because it isn't active in current UIPanel!");
	}
	
	
	public UIButton GetSelectedButton() {
		return currentPanelButtons[selectedButtonIndex];
	}



	public void DisplayMenu() {
		spaceship.enabled = false;
		menuGui.SetActive(true);
		ImmediatelyReloadCurrentPanelButtons();
	}


	public void HideMenu() {
		spaceship.enabled = true;
		StartCoroutine(ReloadCurrentPanelButtons());
		menuGui.SetActive(false);
		
	}


	public void DisplayQuitConfirmation() {
		menuGui.SetActive(false);
		quitConfirmationGui.SetActive(true);
	}
	
	
	public void HideQuitConfirmation() {
		menuGui.SetActive(true);
		quitConfirmationGui.SetActive(false);
	}


	public void DisplayControls() {
		menuGui.SetActive(false);
		controlsGui.SetActive(true);
	}
	
	
	public void HideControls() {
		menuGui.SetActive(true);
		controlsGui.SetActive(false);
	}


	public void DisplayMatchOver() {
		spaceship.enabled = false;
		matchOverGui.SetActive(true);
	}
	
	
	public void HideMatchOver() {
		spaceship.enabled = true;
		matchOverGui.SetActive(false);
	}


	public void ReplayMatch() {
		if (NetworkManager.IsSinglePlayer()) {
			LevelManager.ReloadLevel();
		}
		else {
			networkView.RPC("NetworkReplayMatch", RPCMode.All);
		}
	}


	[RPC]
	private void NetworkReplayMatch() {
		LevelManager.ReloadLevel();
	}


	public void LoadMainMenu() {
		if (Network.isServer) {
			NetworkManager.ServerCleanup();
		}
		LevelManager.LoadMainMenu();
	}

	
}
