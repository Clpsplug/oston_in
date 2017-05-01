using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FutonBehavior : MonoBehaviour {

	private float desiredScale;
	private Vector3 stageDimensions;

	// Use this for initialization
	void Start () {
		// スクリーンの端っこをワールド座標系でとるときにはこれを使用する。
		// これを打つと、右上(Unityでは座標は数学座標系なので)の点の座標が返ってくる。
		// 2Dの場合、カメラが中心座標に向いているのなら座標を逆にした点がちょうど左上になる。
		// そうじゃない場合は、零ベクトルで呼べば良い。
		stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,0));

		// 80から布団をかぶるように布団の大きさを調整する
		// Scaleが１の時、布団の横幅は2.66666...である(8/3, Pixel per Unitより)。
		// 例えば画面のスケールが4:3の時には、端っこの座標が6.7だがそこをGameData.cameraRangeとしている
		// そこを基準として80をを見ると端っこの座標6.7を(80 / cameraRange)で割ればほしい座標数値が出てくる
		desiredScale = (float)(stageDimensions.x * (80.0f / GameData.cameraRange) / (800.0f / 300.0f));
		// この逆数を、child objectのx座標にかけてやらないとずれてしまうのでそうする
		Vector3 tmp = this.transform.GetChild(0).gameObject.transform.position;
		tmp.x = 1.0f / desiredScale;
		this.transform.GetChild(0).gameObject.transform.position = tmp;
	}

	// Update is called once per frame
	void Update () {
		switch (GameData.state) {
			case GameState.WAIT:
			case GameState.AFTERMATH:
			case GameState.RESULT: {
				this.transform.localScale = new Vector3(desiredScale, desiredScale, 1);
				break;
			}
			default: {
				this.transform.localScale = new Vector3(0, 0, 0);
				break;
			}
		}
	}
}
