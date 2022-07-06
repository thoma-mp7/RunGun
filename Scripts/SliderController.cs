using UnityEngine;
using UnityEngine.UI;
using TMPro;

//設定画面のスライダー処理
public class SliderController : MonoBehaviour {

    private Slider _slider;
    private TMP_InputField _inputField;

    void Awake() {
        _slider = GetComponent<Slider>();
        //スライダーの値の変化でスライダー値テキスト反映処理を呼び出すように設定
        _slider.onValueChanged.AddListener(SliderValueUpdate);

        _inputField = GetComponentInChildren<TMP_InputField>();
        //テキストの編集終了でテキスト値スライダー反映処理を呼び出すように設定
        _inputField.onEndEdit.AddListener(FieldValueUpdate);
    }

    void Start() {
        //設定画面のスライダー値を初期化
        switch (transform.tag) {
            case "Volume":
                _slider.value = AudioListener.volume * 100;
                _inputField.text = (AudioListener.volume * 100).ToString();
                break;

            case "NormalSensi":
                _slider.value = CameraController.DefaultLookSpeed;
                _inputField.text = CameraController.DefaultLookSpeed.ToString("F1");
                break;

            case "ZoomSensi":
                _slider.value = CameraController.ZoomRatio;
                _inputField.text = CameraController.ZoomRatio.ToString("F1");
                break;
        }
    }

    //スライダー値テキスト反映処理
    private void SliderValueUpdate(float value) {
        //整数の場合
        if ((int)value == value) {
            _inputField.text = value.ToString();
        } else {
            //小数点以下を含む場合は小数点第2以下を切り捨てる
            value = float.Parse(value.ToString("F1"));
            _inputField.text = value.ToString("");
            _slider.value = value;
        }

        switch (transform.tag) {
            case "Volume":
                var tempVolume = value / 100.0f;
                AudioListener.volume = tempVolume;
                break;

            case "NormalSensi":
                CameraController.DefaultLookSpeed = value;
                break;

            case "ZoomSensi":
                CameraController.ZoomRatio = value;
                break;
        }
    }

    //テキスト値スライダー反映処理を呼び出すように設定
    private void FieldValueUpdate(string value) {
        var fValue = float.Parse(value);
        _slider.value = fValue;
        switch (transform.tag) {
            case "Volume":
                if (fValue > 100) {
                    fValue = 100;
                    _inputField.text = "100";
                } else if (fValue < 0) {
                    fValue = 0;
                    _inputField.text = "0";
                } else {
                    //制限値以内の場合何もしない
                }
                var tempVolume = fValue / 100.0f;
                AudioListener.volume = tempVolume;
                break;

            case "NormalSensi":
                if (fValue > 100) {
                    fValue = 100;
                    _inputField.text = "100";
                } else if (fValue < 0) {
                    fValue = 0;
                    _inputField.text = "0";
                } else {
                    //制限値以内の場合何もしない
                }
                CameraController.DefaultLookSpeed = fValue;
                break;

            case "ZoomSensi":
                if (fValue > 1) {
                    fValue = 1;
                    _inputField.text = "1";
                } else if (fValue < 0) {
                    fValue = 0;
                    _inputField.text = "0";
                } else {
                    //制限値以内の場合何もしない
                }
                CameraController.ZoomRatio = fValue;
                break;
        }
    }

}
