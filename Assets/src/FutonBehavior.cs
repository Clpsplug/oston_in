using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FutonBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		switch (GameData.state) {
			case GameState.WAIT:
			case GameState.AFTERMATH:
			case GameState.RESULT: {
				this.transform.localScale = new Vector3(1, 1, 1);
				break;
			}
			default: {
				this.transform.localScale = new Vector3(0, 0, 0);
				break;
			}
		}
	}
}
