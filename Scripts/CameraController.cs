using UnityEngine;

//カメラ視点移動処理
public class CameraController : MonoBehaviour {

    private GameObject _player;
    //視点移動感度変数
    static public float LookSpeed = 0;
    //通常の視点移動感度
    static public float DefaultLookSpeed = 3.0f;
    //ズーム時の視点移動感度倍率
    static public float ZoomRatio = 0.8f;
    //Playerに対するカメラの視点高さのオフセット
    static public float LookOffset = 0.7f;
    //視点移動計算用変数
    static public Vector2 TempEuler = Vector2.zero;

    void Awake() {
        _player = GameObject.Find("Player");
    }

    // Start is called before the first frame update
    void Start() {
        //感度を通常の感度に設定
        LookSpeed = DefaultLookSpeed;
    }

    // Update is called once per frame
    void Update() {

    }

    void LateUpdate() {
        //カメラ座標移動処理
        transform.position = _player.transform.position + new Vector3(0, LookOffset, 0);

        //カメラ視点移動処理
        TempEuler.x -= InputController.LookInput.y * LookSpeed * Time.deltaTime;
        TempEuler.y += InputController.LookInput.x * LookSpeed * Time.deltaTime;
        TempEuler.x = Mathf.Clamp(TempEuler.x, -89, 89);
        transform.eulerAngles = TempEuler;

    }
}
