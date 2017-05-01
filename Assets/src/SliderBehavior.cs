using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SliderBehavior : MonoBehaviour {

	public GameObject FillArea;
	public GameObject player;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		switch (GameData.state) {
			case GameState.INPUT: {
				PlayerBehavior pb = player.GetComponent<PlayerBehavior>();
				GameObject fill = FillArea.transform.GetChild(0).gameObject;
				float cp = pb.getChallengePower();
				float a = GameData.gravityConstant * pb.getFriction();

				// 表示されるのはプレーヤーのパワーで
				// 一定のところで緑(=成功)となるとは限らない
				// 表示中のプレーヤーパワーで、ちょうど400となろところを中心地にしたい
				// 400 = centerpp^2 / a / 2
				// 400 * a * 2 = centerpp^2
				// centerpp = 800a ^ 1/2

				int neededPower = (int)(Math.Pow(800 * a, 0.5));
				fill.GetComponent<Image>().color = Color.HSVToRGB(EaseFunctions.EaseIn(0, 1.0f / 3, 5, 3 - (Math.Abs((float)cp - neededPower) / neededPower * 3),3, false, false),1,1);
				this.GetComponent<Slider>().value = cp <= 1500 ? cp : 1500;

				break;
			}
			default: {
				break;
			}
		}
	}
}
