public enum GameState {
		WAIT, // タイトル画面の待機状態
		ENVSET, // 手動モード時のチャレンジ環境を決定 (摩擦など)
		INPUT, // 手動モード時の入力受け付け (この間にSpaceキーや画面連打)
		ANIM, // 手動、自動モードともに、まず共通の「Oston-in Challenge!」を表示
		LAUNCH, // 「GO!」でキャラクターが飛ぶ瞬間のアニメーション
		AFTERMATH, // プレーによって変わるキャラクターの末路
		RESULT, // 結果 [FAIL]や[GREAT]や[R.I.P]など (ｸﾞﾜｼｬｰﾝとかもここで行う)
		COUNT // enumの個数
}

public struct RoomSettings {
	float friction;
}

/// <summary>
/// Distance Based Message
/// 距離に応じたメッセージ
/// </summary>
public class DBM {
	private int _distance;
	private string _text;
	public DBM(int dist, string msg) {
		this._distance = dist;
		this._text = msg;
	}
	public int distance() {return this._distance;}
	public string text() {return this._text;}
}

public static class GameData{

	public static int cameraRange = 200; // +-何センチまでカメラにプレーヤーを写すか

	static GameData() {
		state = GameState.WAIT;
		playerPower = 0;
	}

	/// <summary>
	/// ゲーム結果のメインコメント
	/// コメントを設定したあと、距離値が現在の距離値を超えている/等しければストップ
	/// </summary>
	public static DBM[] messages = {
		new DBM(0, "NO INPUT"),
		new DBM(380,"TOO NEAR"),
		new DBM(390, "GREAT"),
		new DBM(400, "EXCELLENT"),
		new DBM(430, "GREAT"),
		new DBM(575, "TOO FAR"),
		new DBM(649, "FAIL"), // どこまでいってんの から
		new DBM(800, "R.I.P") // 永眠 (窓をぶち破るパターンのみ)
	};

	/// <summary>
	/// ゲーム結果のサブコメント
	/// </summary>
	/// <comments>
	/// 手前から順に並んでいる。最初の１つと最後の２つは別枠。
	/// 設定された距離値をみて、設定中の要素の次の距離値が現在の距離値を超えたところでストップ
	/// 100なら50のcommentsに設定したところで3番目の200の距離値が現在の距離値を超えるので、２番目になる
	/// </comments>
	public static DBM[] comments = {
		new DBM(0, "I couldn't get any input. Maybe check if your device is compatible with this game?"), // 入力が取れませんでした
		new DBM(1, "Don't fear - the futon is very far away."), // ビビらなくても大丈夫
		new DBM(100, "Too little momentum. Think about friction."), // このゲームでは摩擦も計算されてるからね
		new DBM(340, "You need more power."), // まあ頭が入ったからいいんじゃね
		new DBM(380, "Try not to suffocate. Seriously."), // 窒息するなよ
		new DBM(390, "A little too near, but not a bad shot."), // 惜しい
		new DBM(400, "JUST OSTON-IN! Have a good dream!"), // ジャストオストンイン！おやすみなさい！良い夢を！
		new DBM(410, "A little too far, but not a bad shot."), // 惜しい
		new DBM(430, "Covering skill: B-."), // ちょっとお腹が冷えそう
		new DBM(500, "No, you just went straight through the futon."), // いや布団通り越してるからね
		new DBM(575, "I think you're in too high spirit to sleep just yet."), // まだ寝なくてもいいぐらい元気
		new DBM(649, "OK, where do you think you're going?"), // どこまでいってんの
		new DBM(750, "Enjoy your headache from the wall of the room."), // 壁に頭をぶつけました
		new DBM(899, "You crashed out of the room through the window glass.") // 窓をぶち破ってしまいました
	};

	public static DBM[] resultsndkey = {
		new DBM(0, "Fail"),
		new DBM(380, "Great"),
		new DBM(400, "Win"),
		new DBM(410, "Great"),
		new DBM(430, "Fail"),
		new DBM(650, "Rip")
	};

	/// <summary>
	/// ゲームの状態
	/// </summary>
	public static GameState state {get; set;}

	/// <summary>
	/// プレーヤーの脚力 (ランダムもしくはタップで決まる)
	/// </summary>
	public static float playerPower {get; set;}

	public static RoomSettings roomEnv {
		set {roomEnv = value;}
		get {return roomEnv;}
	}

}
