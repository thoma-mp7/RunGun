using UnityEngine;
using UnityEngine.SceneManagement;

//タイトル画面開始ボタン処理
public class StartButtonController : MonoBehaviour {
    public void ClickStart() {
        SceneManager.LoadScene("MainScene");
    }
}
