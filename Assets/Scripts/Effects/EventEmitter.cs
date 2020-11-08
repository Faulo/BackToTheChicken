using UnityEngine;

namespace Runtime.Effects {
    public class EventEmitter : MonoBehaviour {
        [SerializeField]
        EffectEvent onStart = default;

        void Start() {
            onStart.Invoke(gameObject);
        }
    }
}