using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//リトライボタン処理
public class RetryButtonController : MonoBehaviour {

    public void OnClickRetryButton() {
        //すべてのSEを停止後、メインシーンをロード
        SoundManager.instance.StopAllSE();
        SceneManager.LoadScene("MainScene");
    }

}
