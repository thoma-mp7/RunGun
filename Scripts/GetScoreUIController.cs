using UnityEngine;

//獲得スコアUI制御クラス
public class GetScoreUIController : MonoBehaviour {

    private RectTransform _getScoreRectTfm;
    private Transform _mainCamTfm;
    //UIが消えるまでの時間
    [SerializeField] private float _destroyTime = 4.0f;
    // Start is called before the first frame update
    void Start() {
        _getScoreRectTfm = GetComponent<RectTransform>();
        _mainCamTfm = GameObject.FindGameObjectWithTag("MainCamera").transform;
        //生成されて指定した時間後にこのスコアUIを削除
        Destroy(gameObject, _destroyTime);
    }

    // Update is called once per frame
    void Update() {
        //常にスコアUIをカメラの方向に向かせる
        _getScoreRectTfm.LookAt(_mainCamTfm);
    }
}
