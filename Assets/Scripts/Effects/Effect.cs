using UnityEngine;

namespace Runtime.Effects {
    public abstract class Effect : ScriptableObject {
        public abstract void Invoke(GameObject context);
    }
}