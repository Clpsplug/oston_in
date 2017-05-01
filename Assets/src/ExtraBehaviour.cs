using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraBehaviour : MonoBehaviour {

	public GameObject player;
	public GameObject WallHitEffect;
	public GameObject GlassHitEffect;
	public GameObject[] Buttons;
	public GameObject Indicator;

	private bool instantiatedFlag;
	private Vector3 stageDimensions;

	// Use this for initialization
	void Start () {
		stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,0));
	}

	// Update is called once per frame
	void Update () {
		switch (GameData.state) {
			case GameState.WAIT: {
				Indicator.gameObject.SetActive(false);
				foreach (GameObject button in Buttons) {
					button.gameObject.SetActive(true);
				}
				break;
			}
			case GameState.INPUT: {
				foreach (GameObject button in Buttons) {
					button.gameObject.SetActive(false);
				}
				Indicator.gameObject.SetActive(true);
				break;
			}
			case GameState.ANIM: {
				foreach (GameObject button in Buttons) {
					button.gameObject.SetActive(false);
				}
				Indicator.gameObject.SetActive(false);
				break;
			}
			case GameState.RESULT: {
				foreach (GameObject button in Buttons) {
					button.gameObject.SetActive(true);
				}
				if (instantiatedFlag == true) {
					break;
				}
				if (GameData.playerPower > 650 && GameData.playerPower < 700) {
						Instantiate(WallHitEffect, new Vector3(-stageDimensions.x, 0.5f, 0), Quaternion.identity);
				}
				else if (GameData.playerPower > 700) {
						Instantiate(GlassHitEffect, new Vector3(-stageDimensions.x, 0.5f, 0), Quaternion.identity);
				}
				instantiatedFlag = true;
				break;
			}
			default: {
				foreach (GameObject button in Buttons) {
					button.gameObject.SetActive(false);
				}
				instantiatedFlag = false;
				break;
			}
		}
	}
}
