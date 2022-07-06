using UnityEngine;

//ターゲット制御処理
public class TargetController : MonoBehaviour {
    [SerializeField] private float _moveSpeed = 6.0f;
    [SerializeField] private float _moveTime = 3.0f;
    //繰り返し移動させる or 1度だけ移動させるモード切替
    [SerializeField] private bool _spawnMode = false;
    private Rigidbody _rb;
    private float _moveCount = 0;
    private Vector3 _startPositon = new Vector3(0, 0, 0);


    void Start() {
        //初期地点を保存
        _startPositon = transform.position;
        _rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update() {
        if (_moveCount < _moveTime) {
            _moveCount += Time.deltaTime;
        } else {
            if (_spawnMode) {
                gameObject.SetActive(false);
            }
            _moveCount = 0;
            transform.position = _startPositon;
        }
    }
    void FixedUpdate() {
        _rb.velocity = transform.forward * _moveSpeed;
    }
}
