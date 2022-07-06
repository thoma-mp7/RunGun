using System.Collections;
using UnityEngine;
using TMPro;

//レベル開始ボタン射撃時処理
public class LevelStartButtonController : MonoBehaviour {

    private GameObject _testObjects;
    private GameObject _startGate;
    private GameObject _startCount;
    private TextMeshProUGUI _startCountTMP;
    private bool _isFirstPush = true;

    void Awake() {
        _testObjects = GameObject.Find("TestObjects");
        _startGate = GameObject.Find("StartGate");
        _startCount = GameObject.Find("StartCount");
        _startCountTMP = _startCount.GetComponentInChildren<TextMeshProUGUI>();
    }

    //レベル開始ボタンイベント
    public void LevelStart() {
        //ボタン初回射撃の場合
        if (_isFirstPush) {
            //スタート地点のオブジェクト移動開始
            StartCoroutine("MoveTestObjects");
            //カウントダウン開始
            StartCoroutine("StartCountDown");
            //初回射撃フラグクリア
            _isFirstPush = false;
        }
    }

    //スタート地点のオブジェクト移動処理
    private IEnumerator MoveTestObjects() {
        for (var i = 0; i < 200; i++) {
            _testObjects.transform.Translate(0, -0.025f, 0);
            yield return new WaitForSeconds(0.025f);
        }
    }

    //カウントダウン処理
    private IEnumerator StartCountDown() {
        for (var i = 5; i >= 0; i--) {
            if (i <= 0) {
                _startCountTMP.text = "START";
                SoundManager.instance.PlaySE("Start");
                SoundManager.instance.PlaySE("BGM");
                Destroy(_startGate);
                GameDirector.IsLevelPlaying = true;
            } else {
                _startCountTMP.text = i.ToString();
                SoundManager.instance.PlaySE("Count");
            }
            yield return new WaitForSeconds(1);
        }
        Destroy(_startCount);
    }
}
