using UnityEngine;

//終了ボタン処理
public class QuitButtonController : MonoBehaviour {
    public void ClickQuit() {
#if UNITY_EDITOR
        //ゲームプレイ終了
        UnityEditor.EditorApplication.isPlaying = false;
#else
        //アプリケーション終了
        Application.Quit();
#endif

    }
}