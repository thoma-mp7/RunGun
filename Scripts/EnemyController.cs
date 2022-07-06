using UnityEngine;
using TMPro;

//敵管理クラス
public class EnemyController : MonoBehaviour {

    private GameObject _getScoreCanvas;
    private GameObject _destroyEffect;
    private TextMeshProUGUI _scoreText;

    //敵のHP
    public int EnemyHP = 1;
    //敵撃破時の追加時間
    [SerializeField] private float _enemyScore = 3.0f;
    //スコアに設定する文字列
    private string _scoreStrings;


    void Start() {
        _getScoreCanvas = (GameObject)Resources.Load("GetScoreCanvas");
        _destroyEffect = (GameObject)Resources.Load("PuffParticle(Custom)");
        _scoreText = _getScoreCanvas.gameObject.GetComponentInChildren<TextMeshProUGUI>();

    }

    void Update() {
        //HP0以下の場合オブジェクトを消去
        if (EnemyHP <= 0) {
            //追加時間を文字列に設定
            _scoreStrings = "+ " + _enemyScore.ToString("F0") + "sec";
            //スコアのテキストを反映
            _scoreText.text = _scoreStrings;
            //撃破時スコアUIを読み込んで破壊されるターゲットの位置に生成
            Instantiate(_getScoreCanvas, transform.position, Quaternion.identity);
            //破壊時エフェクトを再生
            Instantiate(_destroyEffect, transform.position, Quaternion.identity);

            //ターゲット破壊で制限時間追加
            SoundManager.instance.PlaySE("Destroy");
            //ターゲット破壊で制限時間追加
            GameDirector.LeftTime += _enemyScore;
            Destroy(gameObject);
        }
    }

}
