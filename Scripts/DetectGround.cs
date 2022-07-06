using UnityEngine;

//接地検知処理
public class DetectGround : MonoBehaviour {

    //接地状態変数
    static public bool IsGround = true;

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("MovingFloor")) {
            IsGround = true;
            //接地の瞬間に着地音を再生
            SoundManager.instance.PlaySE("Land");
        }
    }

    private void OnCollisionStay(Collision other) {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("MovingFloor")) {
            IsGround = true;
        }
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("MovingFloor")) {
            IsGround = false;
        }
    }

}
