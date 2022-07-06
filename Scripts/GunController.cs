using UnityEngine;

//射撃関連処理
public class GunController : MonoBehaviour {

    [System.NonSerialized] public Animator GunAnimator;
    [SerializeField] private GameObject _bulletHoleDecal;
    [SerializeField] private GameObject _bulletHoleParticle;
    private GameObject _mainCamera;
    private Camera _camComp;

    private GameObject _muzzleFlash;
    private Vector2 _recoilCount = Vector2.zero;
    private Vector2 _restoreCount = Vector2.zero;
    //反動の大きさ
    private Vector2 _recoilValue = new Vector2(5.0f, 0);
    //残弾数
    public int LeftBullets = 0;
    //リロード終了フラグ
    private bool _isReloadEnd = true;
    //現在のFOVを格納する変数
    private float _currentFOV = 0;
    //発射レート
    [SerializeField] private int _fireRate = 700;
    //マガジンサイズ
    public int MagazineSize = 30;
    //通常時のFOV
    [SerializeField] private float _defaultFOV = 90;
    //ズーム時のFOV
    [SerializeField] private float _zoomFOV = 60;
    //発射間隔カウント用変数
    private float _fireInterval = 0;

    void Awake() {
        _mainCamera = GameObject.Find("MainCamera");
        _muzzleFlash = transform.Find("Sup/MuzzleFlash2").gameObject;
        GunAnimator = GetComponent<Animator>();
        _camComp = _mainCamera.GetComponent<Camera>();
    }

    // Start is called before the first frame update
    void Start() {
        //残弾数を初期値にセット
        LeftBullets = MagazineSize;
        //FOV変数を通常時のFOV値にセット
        _currentFOV = _defaultFOV;
    }

    // Update is called once per frame
    void Update() {
        //一時停止中でない場合
        if (Time.timeScale != 0) {
            //射撃間隔カウント頭打ち処理
            if (_fireInterval < 10) {
                _fireInterval += Time.deltaTime;
            }
            //射撃入力時
            if (InputController.FireState) {
                //リロード中でなければ
                if (_isReloadEnd) {
                    //残弾数があれば
                    if (LeftBullets > 0) {
                        //フルオートの場合
                        if (InputController.FireMode) {
                            //設定した射撃間隔(発射レート)を超えた場合
                            if (_fireInterval > (1.0f / (_fireRate / 60.0f))) {
                                //射撃間隔カウントクリア
                                _fireInterval = 0;
                                //射撃処理
                                Fire();
                                //残弾数を1発減らす
                                LeftBullets--;
                            }
                        } else {
                            //セミオートの場合
                            InputController.FireState = false;
                            //射撃処理
                            Fire();
                            //残弾数を1発減らす
                            LeftBullets--;
                        }
                    } else {
                        //設定した射撃間隔を超えた場合(トリガー音重複再生防止)
                        if (_fireInterval > (1.0f / (_fireRate / 60.0f))) {
                            //射撃間隔をクリア
                            _fireInterval = 0;
                            //弾薬が空のトリガー音を再生
                            SoundManager.instance.PlaySE("Fire_Empty");
                        }
                    }
                }
            }

            //反動処理_視点の方向のみを変更することで反動を再現
            //反動カウントが設定値以下の場合
            if (_recoilCount.x <= _recoilValue.x) {
                //反動として視点を上方向に向かせる
                CameraController.TempEuler.x -= 60.0f * Time.deltaTime;
                _recoilCount.x += 60.0f * Time.deltaTime;
            }
            //反動収束カウントが設定値以下の場合
            if (_restoreCount.x <= _recoilValue.x) {
                //反動収束として視点を下方向に向かせる
                CameraController.TempEuler.x += 12.0f * Time.deltaTime;
                _restoreCount.x += 12.0f * Time.deltaTime;
            }

            //リロード処理
            Reload();
            //ズーム処理
            Zoom();
        }

    }

    //射撃処理
    private void Fire() {
        //反動・反動収束カウントをリセット
        _recoilCount = Vector2.zero;
        _restoreCount = Vector2.zero;
        //マズルフラッシュを有効化(すでに有効の場合は一度無効化した後)
        if (_muzzleFlash.activeSelf) {
            _muzzleFlash.SetActive(false);
        }
        _muzzleFlash.SetActive(true);
        //銃声を再生
        SoundManager.instance.PlaySE("Fire");
        //射撃アニメーションを再生
        GunAnimator.SetTrigger("Fire");

        //カメラの中央から正面方向へのRayを生成する
        Ray ray = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);
        //何かのColliderにRayがHitした場合
        if (Physics.Raycast(ray, out RaycastHit hitInfo)) {
            //弾痕を発生させる位置と向きを指定
            var holePos = hitInfo.point + (hitInfo.normal * 0.01f);
            var holeRot = Quaternion.FromToRotation(_bulletHoleDecal.transform.forward, hitInfo.normal);
            //弾痕のデカールをRayがHitしたオブジェクトの子オブジェクトとして生成
            Instantiate(_bulletHoleDecal, holePos, holeRot, hitInfo.transform);
            //弾痕のパーティクルを発生させる向きを指定
            var holeParticleRot = Quaternion.FromToRotation(_bulletHoleParticle.transform.forward, -ray.direction);
            //弾痕のパーティクルをRayがHitした位置に生成
            Instantiate(_bulletHoleParticle, holePos, holeParticleRot);

            //RayがHitしたオブジェクトのタグが"Target"なら、TargetオブジェクトのHPを減らす。
            if (hitInfo.collider.CompareTag("Target")) {
                var enemy = hitInfo.collider.gameObject.GetComponent<EnemyController>();
                enemy.EnemyHP -= 10;
            }

            //Rayが当たったオブジェクトのタグが"StartButton"なら、ゲーム開始処理。
            if (hitInfo.collider.CompareTag("StartButton")) {
                var startCon = hitInfo.collider.gameObject.GetComponent<LevelStartButtonController>();
                startCon.LevelStart();
            }

            //Rayが当たったオブジェクトのタグが"BackTitleButton"なら、タイトルに戻る処理。
            if (hitInfo.collider.CompareTag("BackTitleButton")) {
                var titleCon = hitInfo.collider.gameObject.GetComponent<BackTitleButtonController>();
                titleCon.OnClickTitleButton();
            }

            //Rayが当たったオブジェクトのタグが"RetryButton"なら、リトライ処理。
            if (hitInfo.collider.CompareTag("RetryButton")) {
                var titleCon = hitInfo.collider.gameObject.GetComponent<RetryButtonController>();
                titleCon.OnClickRetryButton();
            }

            //Rayが当たったオブジェクトのタグが"TriggerTarget"なら、動く足場停止処理。
            if (hitInfo.collider.CompareTag("TriggerTarget")) {
                var StageCon = hitInfo.collider.gameObject.GetComponentInParent<StageController>();
                StageCon.StopStage();
            }

        }
    }

    //リロード処理
    private void Reload() {
        //リロード中でなければ
        if (_isReloadEnd) {
            //リロードの入力があれば
            if (InputController.ReloadInput) {
                //残弾がある場合
                if (LeftBullets > 0) {
                    //リロードモーションを再生
                    GunAnimator.SetTrigger("Reload");
                    //リロード音を再生
                    SoundManager.instance.PlaySE("Reload");
                } else {
                    //残弾数0以下の場合はリロードモーションを変更
                    GunAnimator.SetTrigger("Reload_Empty");
                    //残弾0のリロード音を再生
                    SoundManager.instance.PlaySE("Reload_Empty");
                }
                //リロード終了フラグをクリア
                _isReloadEnd = false;
            }
        }
    }

    //リロードモーション終了時呼び出しイベント
    private void ReloadEnd() {
        //残弾数をマガジンサイズに戻す
        LeftBullets = MagazineSize;
        //リロード終了フラグをセット
        _isReloadEnd = true;
        //リロードの入力をクリア
        InputController.ReloadInput = false;
    }

    //ズーム処理
    private void Zoom() {
        //ズーム入力ががあれば
        if (InputController.ZoomState) {
            //FOVをズーム時の値に設定
            SetZoomFOV();
            //視点移動感度をズーム時の値に設定
            CameraController.LookSpeed = CameraController.ZoomRatio * CameraController.DefaultLookSpeed;
        } else {
            //FOVを通常時の値に設定
            SetNormalFOV();
            //視点移動感度を通常時の値に設定
            CameraController.LookSpeed = CameraController.DefaultLookSpeed;
        }
        //算出したFOV値を実際のFOVに反映
        _camComp.fieldOfView = _currentFOV;
    }

    //ズーム時FOV算出処理
    private void SetZoomFOV() {
        float reduceFOV = 500.0f * Time.deltaTime;
        if ((_currentFOV - reduceFOV) < _zoomFOV) {
            _currentFOV = _zoomFOV;
        } else {
            _currentFOV -= reduceFOV;
        }
    }

    //通常時FOV算出処理
    private void SetNormalFOV() {
        float addFOV = 500.0f * Time.deltaTime;
        if ((_currentFOV + addFOV) > _defaultFOV) {
            _currentFOV = _defaultFOV;
        } else {
            _currentFOV += addFOV;
        }
    }

}
