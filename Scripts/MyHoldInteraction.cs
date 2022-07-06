using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.InputSystem.Editor;
#endif

#if UNITY_EDITOR
[InitializeOnLoad]
#endif

//InputSystemの長押しのInteractionクラス
public class MyHoldInteraction : IInputInteraction {
#if UNITY_EDITOR
    static MyHoldInteraction() {
        Initialize();
    }
#endif

    [RuntimeInitializeOnLoadMethod]
    static void Initialize() {
        InputSystem.RegisterInteraction<MyHoldInteraction>();
    }

    //0.1sec毎に入力状態を確認する
    public void Process(ref InputInteractionContext context) {

        switch (context.phase) {
            case InputActionPhase.Waiting:
                context.Started();
                context.SetTimeout(0.1f);
                break;

            case InputActionPhase.Started:
                if (context.timerHasExpired) {
                    context.PerformedAndStayStarted();
                    context.SetTimeout(0.1f);
                } else {
                    context.Canceled();
                }
                break;
        }
    }

    public void Reset() {

    }

}
