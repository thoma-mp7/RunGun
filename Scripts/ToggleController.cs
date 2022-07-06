using UnityEngine;
using UnityEngine.UI;
using System.Linq;

//設定画面のトグル処理
public class ToggleController : MonoBehaviour {

    private ToggleGroup _toggleGroup;
    private Toggle _toggleA;
    private Toggle _toggleB;
    [SerializeField] private string _nameA = "Hold";
    [SerializeField] private string _nameB = "Toggle";

    void Awake() {
        _toggleGroup = GetComponent<ToggleGroup>();


        foreach (var toggle in gameObject.GetComponentsInChildren<Toggle>()) {
            if (toggle.name == _nameA) {
                _toggleA = toggle;
            } else if (toggle.name == _nameB) {
                _toggleB = toggle;
            }
            //トグルの値の変化で設定値を更新する処理を呼び出すように設定
            toggle.onValueChanged.AddListener(ToggleChanged);
        }
    }

    void Start() {
        //オプションUIの値を初期化
        switch (transform.tag) {
            case "ZoomSet":
                if (InputController.ZoomMode) {
                    //切替設定
                    _toggleB.isOn = true;
                } else {
                    //長押し設定
                    _toggleA.isOn = true;
                }
                break;

            case "SplintSet":
                if (InputController.SplintMode) {
                    //切替設定
                    _toggleB.isOn = true;
                } else {
                    //長押し設定
                    _toggleA.isOn = true;
                }
                break;

            case "CrouchSet":
                if (InputController.CrouchMode) {
                    //切替設定
                    _toggleB.isOn = true;
                } else {
                    //長押し設定
                    _toggleA.isOn = true;
                }
                break;
        }
    }

    //トグルの状態で設定値を更新する処理
    private void ToggleChanged(bool state) {
        //Activeなトグルのみ処理
        if (state) {
            //トグルグループ内でActiveなトグルを取得
            Toggle activeToggle = _toggleGroup.ActiveToggles().First();
            //Holdが選択された場合
            if (activeToggle.name == _nameA) {
                switch (transform.tag) {
                    case "ZoomSet":
                        InputController.ZoomMode = false;
                        break;

                    case "SplintSet":
                        InputController.SplintMode = false;
                        break;

                    case "CrouchSet":
                        InputController.CrouchMode = false;
                        break;
                }
                //Toggleが選択された場合
            } else if (activeToggle.name == _nameB) {
                switch (transform.tag) {
                    case "ZoomSet":
                        InputController.ZoomMode = true;
                        break;

                    case "SplintSet":
                        InputController.SplintMode = true;
                        break;

                    case "CrouchSet":
                        InputController.CrouchMode = true;
                        break;
                }
            }
        }
    }

}
