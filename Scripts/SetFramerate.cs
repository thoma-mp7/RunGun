using UnityEngine;

//フレームレート上限設定処理
public class SetFramerate : MonoBehaviour {
    //制限フレームレート
    [SerializeField] private int MaxFramerate = 60;

    void Awake() {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = MaxFramerate;
    }
}
