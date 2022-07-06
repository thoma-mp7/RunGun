using UnityEngine;
using TMPro;

//ゲーム全体の進行を管理するクラス
public class GameDirector : MonoBehaviour {

    //レベル進行中フラグ
    static public bool IsLevelPlaying = false;
    //ゴール済みフラグ
    static public bool IsGoaled = false;
    //残り時間
    static public float LeftTime = 0;
    //スタート時制限時間
    [SerializeField] private float _timeLimit = 60.0f;
    //ゲームオーバーフラグ
    private bool _isGameOver = false;
    //メニュー画面中フラグ
    private bool _isMenuOpen = true;

    private int _minute = 0;
    private int _second = 0;
    private int _oldSecond = 0;
    private int _oldLeftBullets = 0;
    private bool _oldFireMode = true;

    private GameObject _gunCam;
    private GunController _gunCon;

    [SerializeField] private TextMeshProUGUI _leftTimeText;
    [SerializeField] private TextMeshProUGUI _leftBulletsText;
    [SerializeField] private TextMeshProUGUI _magazineSizeText;
    [SerializeField] private TextMeshProUGUI _fireModeText;
    [SerializeField] private GameObject _inGamePanel;
    [SerializeField] private GameObject _optionPanel;

    void Awake() {
        _gunCam = GameObject.Find("GunCamera");
        _gunCon = _gunCam.GetComponentInChildren<GunController>();
    }
    // Start is called before the first frame update
    void Start() {
        //マウスカーソルを非表示
        Cursor.visible = false;
        //マウスカーソルを画面中央にロック
        Cursor.lockState = CursorLockMode.Locked;
        //マガジンサイズをUIに反映
        _magazineSizeText.text = _gunCon.MagazineSize.ToString();
        //射撃モードをUIに反映
        if (InputController.FireMode) {
            _fireModeText.text = "FULL";
        } else {
            _fireModeText.text = "SEMI";
        }
        //残り時間に制限時間を設定
        LeftTime = _timeLimit;
        //レベル中フラグをクリア
        IsLevelPlaying = false;
        //ゴールフラグをクリア
        IsGoaled = false;
        //オプション入力をクリア
        InputController.OptionInput = false;
    }

    // Update is called once per frame
    void Update() {
        //レベルが始まっている かつ ゴールしていない場合
        if (IsLevelPlaying && !IsGoaled) {
            LeftTime -= Time.deltaTime;
        }
        if (LeftTime <= 0) {
            LeftTime = 0;
        }
        _second = (int)LeftTime;

        //表示する値が変化した時のみテキストを更新

        //残り時間表示を更新
        if (_second != _oldSecond) {
            _minute = _second / 60;
            _second -= _minute * 60;
            _leftTimeText.text = _minute.ToString("00") + ":" + _second.ToString("00");
        }
        _oldSecond = _second;

        //残弾数表示を更新
        if (_gunCon.LeftBullets != _oldLeftBullets) {
            _leftBulletsText.text = _gunCon.LeftBullets.ToString();
        }
        _oldLeftBullets = _gunCon.LeftBullets;

        //射撃モード表示を更新
        if (InputController.FireMode != _oldFireMode) {
            if (InputController.FireMode) {
                _fireModeText.text = "FULL";
            } else {
                _fireModeText.text = "SEMI";
            }
        }
        _oldFireMode = InputController.FireMode;


        //オプション画面表示処理
        //オプション入力時
        if (InputController.OptionInput) {
            if (!_isMenuOpen) {
                OpenMenu();
                _isMenuOpen = true;
            }
            //オプション入力解除時
        } else {
            if (_isMenuOpen) {
                CloseMenu();
                _isMenuOpen = false;
            }
        }

        //残り時間0でゲームオーバー
        if (LeftTime <= 0 && !_isGameOver) {
            _isGameOver = true;
            var goalRing = GameObject.Find("GoalRing");
            var player = GameObject.Find("Player");
            //Playerを強制的にゴールさせる
            player.transform.position = goalRing.transform.position + new Vector3(0, 4.0f, 0);

        }
    }

    //オプション画面に切替
    private void OpenMenu() {
        //すべてのSEを停止
        SoundManager.instance.StopAllSE();

        //ゲーム時間停止
        Time.timeScale = 0;
        //オプション画面UIに切替
        _inGamePanel.SetActive(false);
        _optionPanel.SetActive(true);
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

    //オプション画面を閉じる
    private void CloseMenu() {
        //レベルが始まっている かつ ゴールしていない場合BGM再生
        if (IsLevelPlaying && !IsGoaled) {
            SoundManager.instance.PlaySE("BGM");
        }
        //ゲーム時間停止解除
        Time.timeScale = 1;
        //ゲーム中画面UIに切替
        _inGamePanel.SetActive(true);
        _optionPanel.SetActive(false);
        //マウスカーソルを非表示
        Cursor.visible = false;
        //マウスカーソルを画面中央にロック
        Cursor.lockState = CursorLockMode.Locked;
    }

}
