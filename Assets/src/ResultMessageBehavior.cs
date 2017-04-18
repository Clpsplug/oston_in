using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultMessageBehavior : MonoBehaviour {

	public GameObject player;
	public GameObject ChallengeButton;

	// Use this for initialization
	void Start () {
		this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
																				800);
	}

	// Update is called once per frame
	void Update () {
		switch (GameData.state) {
			case GameState.WAIT: {
				this.GetComponent<Text>().text = "Press 'Challenge!' to try your luck!";
				break;
			}
			case GameState.ANIM: {
				this.GetComponent<Text>().text = "Oston-in Challenge";
				break;
			}
			case GameState.LAUNCH: {
				this.GetComponent<Text>().text = "GO!";
				break;
			}
			case GameState.AFTERMATH: {
				this.GetComponent<Text>().text = String.Format("{0} cm", player.GetComponent<PlayerBehavior>().playerPosition().ToString("+#;-#;0"));
				break;
			}
			case GameState.RESULT: {
				int nPosition = player.GetComponent<PlayerBehavior>().playerPosition();
				if (nPosition > 250 && nPosition < 300) {
					nPosition = 250;
				}
				else if (nPosition >= 300) {
					nPosition = 500;
				}
				string position = (nPosition == 500 ? "----" : nPosition.ToString("+#;-#;0"));
				DBM message = new DBM(0, "");
				DBM comment = new DBM(0, "");
				for (int i = 0; i < GameData.messages.Length; i++) {
					message = GameData.messages[i];
					if (message.distance() >= nPosition + 400) {
						break;
					}
				}
				comment = GameData.comments[0];
				for (int i = 0; i < GameData.comments.Length; i++) {
					comment = GameData.comments[i];
					if (comment.distance() >= nPosition + 400) {
						break;
					}
				}
				this.GetComponent<Text>().text = String.Format("{0} cm [{1}] {2}", position, message.text(), comment.text());
				break;
			}
			default: {
				break;
			}
		}
	}
}
