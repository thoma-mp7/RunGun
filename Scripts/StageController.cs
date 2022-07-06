using System.Collections;
using UnityEngine;

//ステージ移動処理
public class StageController : MonoBehaviour {
    //移動速度
    [SerializeField] private float _moveSpeed = 10.0f;
    //移動時間
    [SerializeField] private float _moveTime = 3.0f;
    //停止時間
    [SerializeField] private float _stopTime = 10.0f;

    //移動方向切り替わりカウント
    private float _moveCount = 0;
    //移動方向切替フラグ
    private bool _directionMode = false;
    //ステージ移動許可フラグ
    private bool _canMoveStage = true;



    // Update is called once per frame
    void Update() {
        //ステージ移動許可の場合
        if (_canMoveStage) {
            //設定した時間までステージを移動させる
            if (_moveCount < _moveTime) {
                _moveCount += Time.deltaTime;
                //移動方向フラグによって移動する方向が切り替わる
                if (_directionMode) {
                    gameObject.transform.Translate(transform.forward * _moveSpeed * Time.deltaTime, Space.World);
                } else {
                    gameObject.transform.Translate(-transform.forward * _moveSpeed * Time.deltaTime, Space.World);
                }
            } else {
                //設定した時間を超えた場合、移動方向フラグを切り替える
                _moveCount = 0;
                _directionMode = !_directionMode;
            }
        }

    }

    //ステージ停止処理
    public void StopStage() {
        _canMoveStage = false;
        StartCoroutine("MoveStage");
    }

    //ステージ再始動処理
    private IEnumerator MoveStage() {
        yield return new WaitForSeconds(_stopTime);
        _canMoveStage = true;
    }

}
