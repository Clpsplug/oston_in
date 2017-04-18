using System;
﻿using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

	public Vector3 initPosition;
	public bool rumble;
	public float rumbleAmount;
	public uint rumblewait;
	public bool rumbleX;
	public bool rumbleY;
	public float rumbleTime;
	private int cnt;
	private System.Random rnd = new System.Random();
	public GameObject player;
	private float timer;

	// Use this for initialization
	void Start () {
		cnt = 0;
		initPosition = this.transform.position;
	}

	// Update is called once per frame
	void Update () {
		switch (GameData.state) {
			case GameState.RESULT: {
				timer += Time.deltaTime;
				if (timer < rumbleTime && player.GetComponent<PlayerBehavior>().playerPosition() >= 250) {
					rumble = true;
				}
				else {
					rumble = false;
				}
				break;
			}
			default: {
				timer = 0.0f;
				break;
			}
		}
		cnt++;
		if (rumble) {
			if (rumblewait == 0) {
				throw new Exception("[OOPS: CameraBehaviour] rumblewait cannot be smaller than 1!!");
			}
			if (cnt % rumblewait == 0) {
				float xpos = rumbleX ? rumbleAmount * (float)rnd.NextDouble() * 2 - rumbleAmount : initPosition.x;
				float ypos = rumbleY ? rumbleAmount * (float)rnd.NextDouble() * 2 - rumbleAmount : initPosition.y;
				this.transform.position = new Vector3(xpos, ypos, -10.0f);
			}
		}
		else {
			this.transform.position = initPosition;
		}

	}
}
