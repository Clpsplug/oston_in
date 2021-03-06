using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RBM;

public class PlayerBehavior : MonoBehaviour {

	private float timer;
	private RandomBoxMuller rnd = new RandomBoxMuller();
	private Vector3 stageDimensions;
	private float startPosition;
	private float endPosition;
	private System.Random lrnd = new System.Random();
	private bool resultsnd = false;
	private float friction = 0.0f;
	private float challengePower = 0;
	private AudioSource chargeSound;
	private AudioSource dirtSound;
	private float desiredScale;

	// Use this for initialization
	void Start () {
		// スクリーンの端っこをワールド座標系でとるときにはこれを使用する。
		// これを打つと、右上(Unityでは座標は数学座標系なので)の点の座標が返ってくる。
		// 2Dの場合、カメラが中心座標に向いているのなら座標を逆にした点がちょうど左上になる。
		// そうじゃない場合は、零ベクトルで呼べば良い。
		stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,0));

		// オストンイン・ガイのアスペクト比による大きさ調整
		desiredScale = (float)(stageDimensions.x * (80.0f / GameData.cameraRange) / (800.0f / 300.0f));
		this.transform.localScale = new Vector3(desiredScale, desiredScale, 1);

		// GameData.cameraRangeをstageDimensionsのMAXとする場合、スタート地点である-500cmはどこか
		startPosition = stageDimensions.x / GameData.cameraRange * 400;
		Debug.Log(startPosition);
		Debug.Log(stageDimensions);
		Debug.Log(Camera.main.ScreenToWorldPoint(Vector3.zero));
	}

	// Update is called once per frame
	void Update () {
		switch (GameData.state) {
			case GameState.WAIT: {
				this.transform.position = new Vector3(1, 0, 0);
				break;
			}
			case GameState.ENVSET: {
				// ランダムで摩擦が決定
				GameData.state = GameState.INPUT;
				friction = (float)(rnd.next() + 5.0f);
				challengePower = 0;
				chargeSound = AudioManager.PlaySoundEvent("Charge");
				break;
			}
			case GameState.INPUT: {
				// For PC
				// 長押しで反応してしまわないように、anyKeyDOWNで判定する
				if (Input.anyKeyDown) {
					challengePower += 12;
				}
				// For Smartphones
				//
				for (int i = 0; i < Input.touches.Length; i++) {
					if (Input.GetTouch(i).phase == TouchPhase.Began) {
						challengePower += 15;
					}
				}
				this.transform.position = new Vector3(1, 0, 0);
				// 毎フレーム力が抜けていく
				challengePower -= challengePower * 0.003f;
				// キャラクターがちょうど半分になるところがベストとする
				// 初速が決まるので、物理計算をして結果を決定
				// 初速を二乗してa = g * frictionで割って半分
				// 1000の時に800cm(R.I.P)となるようにgを調整したい
				float a = friction * GameData.gravityConstant;
				GameData.playerPower = (float)(Math.Pow(challengePower, 2) / a / 2);
				invTick();
				chargeSound.pitch = EaseFunctions.Linear(0.5, 1, challengePower, 1000, true, false);
				if (timer < 0.0f) {
					chargeSound.Stop();
					Debug.Log(String.Format("Player Power is {0}", GameData.playerPower));
					// もし+-10cm以内にランディングしたら成功とする
					GameData.playerPower = Math.Abs(GameData.playerPower - 400) < 10 ? 400 : GameData.playerPower;
					GameData.state = GameState.ANIM;
					endPosition = -stageDimensions.x / GameData.cameraRange * (GameData.playerPower - 400.0f);
				}
				break;
			}
			case GameState.ANIM: {
				this.transform.position = new Vector3(1, 0, 0);
				this.transform.localScale = new Vector3(desiredScale * EaseFunctions.Linear(1, 0.3, timer, 1.0f, false, false), desiredScale, 1);
				if (timer > 1.0f) {
					AudioManager.PlaySoundEvent("Launch");
					GameData.state = GameState.LAUNCH;
					timer = 0.0f;
				}
				tick();
				break;
			}
			case GameState.LAUNCH: {
				this.transform.localScale = new Vector3(desiredScale * EaseFunctions.Linear(0.3, 1, timer, 0.1f, false, false), desiredScale, 1);
				this.transform.position = new Vector3(EaseFunctions.Linear(1, -stageDimensions.x, timer, 0.15f, true, false), 0, 0);
				if (timer > 1.5f) {
					dirtSound = AudioManager.PlaySoundEvent("Dirt");
					GameData.state = GameState.AFTERMATH;
					timer = 0.0f;
				}
				tick();
				break;
			}
			case GameState.AFTERMATH: {
				// 普通は最後までアニメーションする
				this.transform.position = new Vector3(EaseFunctions.EaseIn(endPosition, startPosition, 2, (2.0f - timer), 2.0f, false, false) + 1, 0, 0);
				// ただし、+250cm ~ +300cmの場合、+250で止めてプレーヤーが壁に頭をぶつける演出が発生
				// +300cmオーバーの場合は、そこまで演出を続ける
				if (timer > 2.0f || (GameData.playerPower > 650.0f && this.playerPosition() > 250)) {
					dirtSound.Stop();
					GameData.state = GameState.RESULT;
					timer = 0.0f;
					if (GameData.playerPower > 700) {
						AudioManager.PlaySoundEvent("Glass");
					}
					else if (GameData.playerPower > 650) {
						AudioManager.PlaySoundEvent("Crash");
					}
				}
				tick();
				break;
			}
			case GameState.RESULT: {
				// +250cm ~ +300cmの場合、+250で止めてプレーヤーが壁に頭をぶつける演出が発生
				// +300cmオーバーの場合は、やはり+250で止めて距離を測定不能としてR.I.P.
				if (GameData.playerPower > 650 && GameData.playerPower < 700) {
					this.transform.position = new Vector3(-stageDimensions.x / GameData.cameraRange * (250.0f), 0, 0);
				}
				if (GameData.playerPower > 700) {
					// 内部では+600cmとする
					this.transform.position = new Vector3(-stageDimensions.x / GameData.cameraRange * (900.0f), 0, 0);
				}
				if (!resultsnd){
					StartCoroutine(resultSound());
					resultsnd = true;
				}
				break;
			}
		}
	}

	IEnumerator resultSound() {
		DBM key = new DBM(0, "");
		if (GameData.playerPower > 650) {
			yield return new WaitForSeconds(1);
		}
		for (int i = 0; i < GameData.resultsndkey.Length; i++) {
			if (GameData.playerPower < GameData.resultsndkey[i].distance()) {
				break;
			}
			key = GameData.resultsndkey[i];
		}
		AudioManager.PlaySoundEvent(key.text());
	}

	public void Challenge() {
		switch (GameData.state) {
			case GameState.WAIT: case GameState.RESULT: {
				// 10秒与えてプレーヤーに入力させる
				timer = 10.0f;
				resultsnd = false;
				GameData.playerPower = 0;
				GameData.state = GameState.ENVSET;
				break;
			}
			default: {
				break;
			}
		}
	}

	public void Launch() {
		switch (GameData.state) {
			case GameState.WAIT: case GameState.RESULT: {
				timer = 0.0f;
				resultsnd = false;
				// Currently, Player power is equal to the resulting distance.
				// Deduct 500 from the value to get the real distance!
				GameData.playerPower = (float)(rnd.next() + 5.0f) * 100.0f - 100.0f;
				double lucky = lrnd.NextDouble(); // 10%の確率で確実に成功し、5%の確率で確実に永眠する
				// Rare, but if playerPower is below 0, then set it to 0
				if (GameData.playerPower < 0) {
					GameData.playerPower = 0;
				}
				// +-10cmは成功とする。luckyが0.90以上でも成功。
				if ((GameData.playerPower > 390.0f && GameData.playerPower < 410.0f) || lucky > 0.90f) {
					GameData.playerPower = 400.0f;
				}
				else if (lucky < 0.15f) {
					if (lucky < 0.05f) {
						// luckyが0.05以下の場合確定で永眠する
						GameData.playerPower = 1000;
					}
					else {
						GameData.playerPower = 690;
					}
				}
				GameData.state = GameState.ANIM;
				endPosition = -stageDimensions.x / GameData.cameraRange * (GameData.playerPower - 400.0f);
				Debug.Log(String.Format("Player power (distance) is {0}m and Lucky number was {1}", (GameData.playerPower - 400.0f) / 100.0f, lucky));
				break;
			}
			default: {
				//
				break;
			}
		}
	}

	void tick() {
		timer += Time.deltaTime;
	}

	void invTick() {
		timer -= Time.deltaTime;
	}

	public int playerPosition() {
		switch (GameData.state) {
			case GameState.AFTERMATH: case GameState.RESULT: {
				int calculatedPosition = (int)((startPosition - this.transform.position.x + 1) / (startPosition / 400.0f) - 400.0f);
				return calculatedPosition;
			}
			default: {
				throw new Exception("You should not be calling this function when the game state is not AFTERMATH or RESULT.");
			}
		}
	}

	public float getChallengePower() {
		return this.challengePower;
	}
	public float getFriction() {
		return this.friction;
	}

}
