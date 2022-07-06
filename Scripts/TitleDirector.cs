using UnityEngine;

//タイトル画面管理クラス
public class TitleDirector : MonoBehaviour {

    // Start is called before the first frame update
    void Start() {
        //マウスカーソルを表示
        Cursor.visible = true;
#if UNITY_EDITOR
        //エディタの場合、マウスカーソルを自由に動かせるように
        Cursor.lockState = CursorLockMode.None;
#else
            //エディタでない場合、マウスカーソルを画面内で動かせるように
            Cursor.lockState = CursorLockMode.Confined;
#endif
    }

}
