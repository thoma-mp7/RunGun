using UnityEngine;

//Player制御クラス
public class PlayerController : MonoBehaviour {

    private GameObject _cam;
    private GameObject _gun;
    private GunController _gunCon;
    private Rigidbody _rb;
    private Vector3 _moveFront = Vector3.zero;
    //接地時の速度格納変数
    private float _moveSpeedGround = 5.0f;
    //リスポーン位置格納変数
    private Vector3 _respawnPosition = new Vector3(0, 0, 0);
    //ブースト中フラグ
    public bool IsBoost = false;
    //ブースト時間カウント用変数
    private float _boostTimeCount = 0;

    //歩きの速度
    [SerializeField] private float _normalSpeed = 5.0f;
    //スプリントの速度
    [SerializeField] private float _splintSpeed = 8.0f;
    //ブースト中の速度
    [SerializeField] private float _boostSpeed = 15.0f;
    //ブースト時間
    [SerializeField] private float _boostTime = 0.5f;
    //空中での速度
    [SerializeField] private float _moveSpeedAir = 300.0f;
    //ジャンプ力
    [SerializeField] private float _jumpPower = 5.0f;
    //追加の重力
    [SerializeField] private float _addGravityPower = -500.0f;
    //落下時のペナルティ時間
    [SerializeField] private float _fallTime = 2.0f;

    void Awake() {
        _cam = GameObject.Find("MainCamera");
        _gun = GameObject.FindGameObjectWithTag("MyGun");
        _gunCon = _gun.GetComponent<GunController>();
        _rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start() {
        //リスポーン位置に初期位置を格納
        _respawnPosition = transform.position;

    }

    // Update is called once per frame
    void Update() {
        //カメラの前方向のX-Z平面の単位ベクトルを取得
        var camFront = Vector3.Scale(_cam.transform.forward, new Vector3(1, 0, 1)).normalized;
        //カメラの右方向のX-Z平面の単位ベクトルを取得
        var camRight = Vector3.Scale(_cam.transform.right, new Vector3(1, 0, 1)).normalized;
        //移動入力とカメラの方向から進行方向の単位ベクトルを取得
        _moveFront = (camFront * InputController.MoveInput.y + camRight * InputController.MoveInput.x).normalized;
        //スプリント処理
        Splint();
        //しゃがみ処理
        Crouch();
        //移動アニメーション処理
        WalkMotionSet();
        //リスポーン処理
        Respawn();
    }

    void FixedUpdate() {
        //移動処理
        Move();
        //ジャンプ処理
        Jump();
        //追加重力処理
        Gravity();
    }

    //移動処理
    private void Move() {
        //接地時
        if (DetectGround.IsGround == true) {
            //移動入力がない場合
            if (_moveFront == new Vector3(0, 0, 0)) {
                //足音を停止
                SoundManager.instance.StopSE("Walk");
                SoundManager.instance.StopSE("Run");
            }
            //足音を再生
            if (_moveSpeedGround == _normalSpeed) {
                if (!SoundManager.instance.IsPlayingSE("Walk")) {
                    SoundManager.instance.PlaySE("Walk");
                }
                SoundManager.instance.StopSE("Run");
            } else {
                if (!SoundManager.instance.IsPlayingSE("Run")) {
                    SoundManager.instance.PlaySE("Run");
                }
                SoundManager.instance.StopSE("Walk");
            }
            //接地時の移動
            _rb.velocity = new Vector3(_moveFront.x * _moveSpeedGround, _rb.velocity.y, _moveFront.z * _moveSpeedGround);
        } else {
            //足音を停止
            SoundManager.instance.StopSE("Walk");
            SoundManager.instance.StopSE("Run");
            //空中での移動
            _rb.AddForce(new Vector3(_moveFront.x * _moveSpeedAir, 0, _moveFront.z * _moveSpeedAir));
        }
    }

    //ジャンプ処理
    private void Jump() {
        //ジャンプ入力時
        if (InputController.JumpInput == true) {
            //しゃがみ状態の場合
            if (InputController.CrouchState == true) {
                //視点を立ちの高さに設定
                SetNormalPosition();
                InputController.CrouchState = false;
                //接地時
            } else if (DetectGround.IsGround == true) {
                //Y軸方向以外の速度は維持したままY軸の速度のみ変更
                _rb.velocity = new Vector3(_rb.velocity.x, _jumpPower, _rb.velocity.z);
                //ジャンプのSEを再生
                SoundManager.instance.PlaySE("Jump");
            } else {
                //空中では処理なし
            }
            InputController.JumpInput = false;
        }
    }

    //追加重力処理(通常の重力に加えて軽快な動作にするため)
    private void Gravity() {
        //空中の場合
        if (DetectGround.IsGround == false) {
            //追加重力を加える
            _rb.AddForce(new Vector3(0, _addGravityPower, 0));
        }
    }

    //スプリント処理
    private void Splint() {
        //前方向の入力がない場合
        if (InputController.MoveInput.y <= 0) {
            //スプリント解除
            InputController.SplintState = false;
        }
        //ブースト状態の場合
        if (IsBoost) {
            //接地時の速度をブーストの速度にする
            _moveSpeedGround = _boostSpeed;
            //ブースト中のカウントを加算
            _boostTimeCount += Time.deltaTime;
            //ブースト時間をカウントが超えた場合
            if (_boostTimeCount > _boostTime) {
                IsBoost = false;
                _boostTimeCount = 0;
            }
        } else {
            //ブースト状態以外の場合
            //スプリント入力ありの場合
            if (InputController.SplintState == true) {
                //しゃがみ状態の場合
                if (InputController.CrouchState == true) {
                    //スプリント解除
                    InputController.SplintState = false;
                } else {
                    //しゃがみ状態以外の場合
                    //接地時の速度をスプリントの速度にする
                    _moveSpeedGround = _splintSpeed;
                }
            } else {
                //スプリント入力なしの場合
                //接地時の速度を歩きの速度にする
                _moveSpeedGround = _normalSpeed;
            }
        }
    }

    //しゃがみ処理
    private void Crouch() {
        //しゃがみ入力ありの場合
        if (InputController.CrouchState == true) {
            //視点をしゃがもの高さに設定
            SetCrouchPosition();
        } else {
            //視点を立ちの高さに設定
            SetNormalPosition();
        }
    }

    //視点高さしゃがみ設定処理
    private void SetCrouchPosition() {
        //視点のオフセットが0になるまで徐々に下げる
        if (CameraController.LookOffset > 0) {
            CameraController.LookOffset -= 6.0f * Time.deltaTime;
        } else {
            CameraController.LookOffset = 0;
        }
    }

    //視点高さしゃがみ設定処理
    private void SetNormalPosition() {
        //視点のオフセットが1になるまで徐々に上げる
        if (CameraController.LookOffset < 1.0f) {
            CameraController.LookOffset += 6.0f * Time.deltaTime;
        } else {
            CameraController.LookOffset = 1.0f;
        }
    }

    //歩きモーション設定処理
    private void WalkMotionSet() {
        //初期値に"Walk"アニメーションを設定
        int tempMotion = 1;
        //移動の入力がない場合は"Idle"アニメーションに遷移
        if (InputController.MoveInput == new Vector2(0, 0)) {
            tempMotion = 0;
        }
        //Splintが動作中の場合は"Run"アニメーションに遷移
        if (InputController.SplintState == true) {
            tempMotion = 2;
        }
        //移動アニメーションに反映
        _gunCon.GunAnimator.SetInteger("Walk_Run", tempMotion);
    }

    //リスポーン処理
    private void Respawn() {
        //直近で通過したチェックポイントにリスポーン
        //Y座標-6以下(マップから落下)でリスポーン
        if (transform.position.y < -6) {
            transform.position = _respawnPosition;
            //残り時間を減らす
            GameDirector.LeftTime -= _fallTime;
        }
    }

    //チェックポイント設定処理
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("CheckPoint")) {
            //チェックポイント通過でチェックポイントの位置をリスポーン地点として保存
            _respawnPosition = other.gameObject.transform.position;
        }
    }

    //動くステージ対応処理
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("MovingFloor")) {
            //動くステージの子オブジェクトとすることで滑らなくする
            var emptyParent = other.transform.parent;
            transform.SetParent(emptyParent);
        }
    }

    //動くステージ対応処理
    void OnCollisionExit(Collision other) {
        if (other.gameObject.CompareTag("MovingFloor")) {
            //動くステージの子オブジェクトを解除
            transform.parent = null;
        }
    }


}
