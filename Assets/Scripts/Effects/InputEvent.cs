using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Effects {
    public class InputEvent : MonoBehaviour {
        [SerializeField]
        InputAction action = new InputAction();
        [SerializeField]
        EffectEvent onAct = new EffectEvent();

        void OnEnable() {
            action.performed += PerformedListener;
            action.Enable();
        }
        void OnDisable() {
            action.Disable();
            action.performed -= PerformedListener;
        }
        void PerformedListener(InputAction.CallbackContext context) {
            onAct.Invoke(gameObject);
        }
    }
}