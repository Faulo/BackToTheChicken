using System;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Effects {
    [Serializable]
    public class EffectEvent : UnityEvent<GameObject> {
    }
}