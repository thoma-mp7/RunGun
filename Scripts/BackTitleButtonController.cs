using UnityEngine;
using UnityEngine.SceneManagement;

//"タイトルに戻る"ボタン処理
public class BackTitleButtonController : MonoBehaviour {

    //タイトルボタンクリックイベント
    public void OnClickTitleButton() {
        //すべてのSEを停止後、タイトルシーンをロード
        SoundManager.instance.StopAllSE();
        SceneManager.LoadScene("TitleScene");
    }
}
