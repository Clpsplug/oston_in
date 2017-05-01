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

	public static int gravityConstant = 125; // いわゆる物理のg

	static GameData() {
		state = GameState.WAIT;
		playerPower = 0;
	}

	/// <summary>
	/// ゲーム結果のメインコメント
	/// コメントを設定したあと、距離値が現在の距離値を超えている/等しければストップ
	/// </summary>
	public static DBM[] messages = {
		new DBM(0, "入力が取れませんでした"),
		new DBM(380,"失敗 (近すぎ)"),
		new DBM(390, "微妙"),
		new DBM(400, "成功"),
		new DBM(430, "微妙"),
		new DBM(575, "失敗(遠すぎ)"),
		new DBM(750, "アホ"), // どこまでいってんの から
		new DBM(800, "永眠") // 永眠 (窓をぶち破るパターンのみ)
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
		new DBM(0, "別の操作方法を試してみてください"), // 入力が取れませんでした
		new DBM(1, "まだまだ距離があります"), // ビビらなくても大丈夫
		new DBM(200, "勢い不足です"), // このゲームでは摩擦も計算されてるからね
		new DBM(320, "床で寝てください"), // まあ頭が入ったからいいんじゃね
		new DBM(360, "モガッ"), // 窒息するなよ
		new DBM(390, "惜しい！"), // 惜しい
		new DBM(400, "ｼﾞｬｽﾄｵｽﾄﾝｲﾝ! おやすみなさい！良い夢を！"), // ジャストオストンイン！おやすみなさい！良い夢を！
		new DBM(410, "惜しい！"), // 惜しい
		new DBM(430, "ちょっとお腹が冷えそう"), // ちょっとお腹が冷えそう
		new DBM(500, "勢いをつけすぎました"), // いや布団通り越してるからね
		new DBM(575, "まだ寝なくてもいいぐらい元気"), // まだ寝なくてもいいぐらい元気
		new DBM(649, "どこまで行ってんのーーーー"), // どこまでいってんの
		new DBM(800, "壁に頭をぶつけました"), // 壁に頭をぶつけました
		new DBM(899, "窓をぶち破ってしまいました") // 窓をぶち破ってしまいました
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

}
