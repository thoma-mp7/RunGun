using UnityEngine;
using UnityEngine.InputSystem;

//InputSystemの入力を処理するクラス
public class InputController : MonoBehaviour {

    //移動入力値格納用変数
    static public Vector2 MoveInput = Vector2.zero;
    //視点移動入力値格納用変数
    static public Vector2 LookInput = Vector2.zero;
    //ジャンプ入力状況格納用変数
    static public bool JumpInput = false;
    //リロード入力状況格納用変数
    static public bool ReloadInput = false;
    //オプション入力状況格納用変数
    static public bool OptionInput = false;
    //射撃入力状況格納用変数
    static public bool FireState = false;
    //射撃モード設定(フルorセミ)格納用変数[true:フル_false:セミ]
    static public bool FireMode = true;
    //スプリント入力状況格納用変数
    static public bool SplintState = false;
    //スプリントモード設定(切替or長押し)格納用変数[true:切替_false:長押し]
    static public bool SplintMode = true;
    //しゃがみ入力状況格納用変数
    static public bool CrouchState = false;
    //しゃがみモード設定(切替or長押し)格納用変数[true:切替_false:長押し]
    static public bool CrouchMode = true;
    //ズーム入力状況格納用変数
    static public bool ZoomState = false;
    //ズームモード設定(切替or長押し)格納用変数[true:切替_false:長押し]
    static public bool ZoomMode = false;

    //移動入力処理
    public void OnMove(InputAction.CallbackContext context) {
        MoveInput = context.ReadValue<Vector2>();
    }

    //視点移動入力処理
    public void OnLook(InputAction.CallbackContext context) {
        LookInput = context.ReadValue<Vector2>();
    }

    //ジャンプ入力処理
    public void OnJump(InputAction.CallbackContext context) {
        if (context.started) {
            JumpInput = true;
        }
    }

    //リロード入力処理
    public void OnReload(InputAction.CallbackContext context) {
        if (context.started) {
            ReloadInput = true;
        }
    }

    //オプション入力処理
    public void OnOption(InputAction.CallbackContext context) {
        if (context.started) {
            OptionInput = !OptionInput;
        }
    }

    //射撃入力処理
    public void OnFire(InputAction.CallbackContext context) {
        if (context.started) {
            FireState = true;
        } else if (context.canceled) {
            FireState = false;
        }
    }

    //射撃モード設定切替入力処理
    public void OnFirePattern(InputAction.CallbackContext context) {
        if (context.started) {
            //射撃モードをフルオートとセミオートで切り替える
            FireMode = !FireMode;
        }
    }

    //スプリント入力処理
    public void OnSplint(InputAction.CallbackContext context) {
        if (SplintMode) {
            //切替設定の処理
            if (context.started) {
                if (SplintState == true) {
                    SplintState = false;
                } else {
                    SplintState = true;
                }
            }
        } else {
            //長押し設定の処理
            if (context.started) {
                SplintState = true;
            } else if (context.performed) {
                SplintState = true;
            } else if (context.canceled) {
                SplintState = false;
            }
        }
    }

    //しゃがみ入力処理
    public void OnCrouch(InputAction.CallbackContext context) {
        if (CrouchMode) {
            //切替設定の処理
            if (context.started) {
                if (CrouchState == true) {
                    CrouchState = false;
                } else {
                    CrouchState = true;
                }
            }
        } else {
            //長押し設定の処理
            if (context.started) {
                CrouchState = true;
            } else if (context.performed) {
                CrouchState = true;
            } else if (context.canceled) {
                CrouchState = false;
            }
        }
    }

    //ズーム入力処理
    public void OnZoom(InputAction.CallbackContext context) {
        if (ZoomMode) {
            //切替設定の処理
            if (context.started) {
                if (ZoomState == true) {
                    ZoomState = false;
                } else {
                    ZoomState = true;
                }
            }
        } else {
            //長押し設定の処理
            if (context.started) {
                ZoomState = true;
            } else if (context.performed) {
                ZoomState = true;
            } else if (context.canceled) {
                ZoomState = false;
            }
        }
    }
}
