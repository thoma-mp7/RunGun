using System.Collections;
using UnityEngine;
using TMPro;

//ゴール処理
public class GoalController : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI _resultTimeHigh;
    [SerializeField] private TextMeshProUGUI _resultTimeLow;
    private GameObject _resultPanel;

    void Awake() {
        _resultPanel = GameObject.Find("ResultPanel");
    }

    //ゴールに触れた場合
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            //まだゴールしていない場合
            if (!GameDirector.IsGoaled) {
                GameDirector.IsGoaled = true;
                //BGMを停止
                SoundManager.instance.StopSE("BGM");
                //リザルト更新
                var second = GameDirector.LeftTime;
                var minute = (int)second / 60;
                second -= (float)minute * 60;
                _resultTimeHigh.text = minute.ToString("00") + ":" + second.ToString("00");
                var secondDeci = (int)(second % 1 * 100);
                _resultTimeLow.text = secondDeci.ToString("00");

                //リザルト画面の移動開始
                StartCoroutine("MoveResultPanel");
            }
        }
    }

    //リザルト画面の移動処理
    private IEnumerator MoveResultPanel() {
        for (var i = 0; i < 150; i++) {
            _resultPanel.transform.Translate(0, 0.02f, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
