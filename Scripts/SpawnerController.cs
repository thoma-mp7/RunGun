using UnityEngine;

//ターゲットのスポーン処理
public class SpawnerController : MonoBehaviour {

    private GameObject _targets;
    private bool _isFirstSpawn = true;

    // Start is called before the first frame update
    void Start() {
        _targets = transform.Find("SpawnTargets").gameObject;
        //ターゲットを無効化する
        _targets.SetActive(false);
    }


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            //初回のみ
            if (_isFirstSpawn) {
                _isFirstSpawn = false;
                _targets.SetActive(true);
            }
        }
    }
}
