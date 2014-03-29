﻿using UnityEngine;
using System.Collections;

public class HudWinnerText : MonoBehaviour {

	UILabel winnerText;
	MatchManager matchManager;

	// Use this for initialization
	void Start () {
		winnerText = GetComponent<UILabel>();
		winnerText.text = string.Format("PLAYER {0} WINS", matchManager.scoreLeader);
	}

}
