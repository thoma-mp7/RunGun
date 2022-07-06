using UnityEngine;

//メニューの閉じるボタンクリック処理
public class MenuCloseButtonController : MonoBehaviour {

    public void OnClickCloseButton() {
        InputController.OptionInput = false;
    }
}
