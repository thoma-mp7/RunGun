using UnityEngine;

//加速板処理
public class DashBoardController : MonoBehaviour {

    //加速板進入時イベント
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            //Playerのブーストフラグをセットする
            var playerCon = other.gameObject.GetComponent<PlayerController>();
            playerCon.IsBoost = true;
        }
    }

}
