using UnityEngine;

//ジャンプパッド処理
public class JumpPadController : MonoBehaviour {

    //ジャンプパッドのジャンプ力
    [SerializeField] private float _jumpPadPower = 500.0f;

    //ジャンプパッド進入時
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            //Y方向にImpulseでAddForceする
            var playerRb = other.gameObject.GetComponent<Rigidbody>();
            playerRb.AddForce(0, _jumpPadPower, 0, ForceMode.Impulse);
        }
    }
}
