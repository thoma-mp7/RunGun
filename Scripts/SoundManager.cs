using UnityEngine;

//ゲーム音を一括管理クラス
public class SoundManager : MonoBehaviour {

    //ゲーム音を一括で管理するクラスのためシングルトンにする
    public static SoundManager instance;

    [SerializeField] private AudioSource _normalAudioSource;
    [SerializeField] private AudioSource _bgmAudioSource;
    [SerializeField] private AudioSource _walkAudioSource;
    [SerializeField] private AudioSource _runAudioSource;
    [SerializeField] private AudioSource _reloadAudioSource;
    [SerializeField] private AudioSource _reloadEmptyAudioSource;

    [SerializeField] private AudioClip _bgmSE;
    [SerializeField] private AudioClip _walkSE;
    [SerializeField] private AudioClip _runSE;
    [SerializeField] private AudioClip _reloadSE;
    [SerializeField] private AudioClip _reloadEmptySE;
    [SerializeField] private AudioClip _fireSE;
    [SerializeField] private AudioClip _fireEmptySE;
    [SerializeField] private AudioClip _jumpSE;
    [SerializeField] private AudioClip _destroySE;
    [SerializeField] private AudioClip _startSE;
    [SerializeField] private AudioClip _countSE;




    void Awake() {
        // シングルトンかつ、シーン遷移しても破棄されないようにする
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void PlaySE(string seName) {
        switch (seName) {
            case "BGM":
                _bgmAudioSource.Play();
                break;
            case "Walk":
                _walkAudioSource.Play();
                break;
            case "Run":
                _runAudioSource.Play();
                break;
            case "Reload":
                _reloadAudioSource.PlayOneShot(_reloadSE);
                break;
            case "Reload_Empty":
                _reloadEmptyAudioSource.PlayOneShot(_reloadEmptySE);
                break;
            case "Fire":
                _normalAudioSource.PlayOneShot(_fireSE);
                break;
            case "Fire_Empty":
                _normalAudioSource.PlayOneShot(_fireEmptySE);
                break;
            case "Jump":
                _normalAudioSource.PlayOneShot(_jumpSE);
                break;
            case "Destroy":
                _normalAudioSource.PlayOneShot(_destroySE);
                break;
            case "Start":
                _normalAudioSource.PlayOneShot(_startSE);
                break;
            case "Count":
                _normalAudioSource.PlayOneShot(_countSE);
                break;
        }
    }

    public void StopSE(string seName) {
        switch (seName) {
            case "BGM":
                _bgmAudioSource.Stop();
                break;
            case "Walk":
                _walkAudioSource.Stop();
                break;
            case "Run":
                _runAudioSource.Stop();
                break;
        }
    }

    public void StopAllSE() {
        _normalAudioSource.Stop();
        _bgmAudioSource.Stop();
        _walkAudioSource.Stop();
        _runAudioSource.Stop();
        _reloadAudioSource.Stop();
        _reloadEmptyAudioSource.Stop();
    }

    public bool IsPlayingSE(string seName) {
        switch (seName) {
            case "BGM":
                return _bgmAudioSource.isPlaying;
            case "Walk":
                return _walkAudioSource.isPlaying;
            case "Run":
                return _runAudioSource.isPlaying;
            default:
                return false;
        }
    }

}
