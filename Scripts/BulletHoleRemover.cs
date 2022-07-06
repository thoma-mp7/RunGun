using UnityEngine;

//弾痕削除処理
public class BulletHoleRemover : MonoBehaviour {

    //弾痕発生から削除までの時間
    [SerializeField] private float _destroyTime = 15.0f;
    // Start is called before the first frame update
    void Start() {
        Destroy(gameObject, _destroyTime);
    }
}
