using Cinemachine;
using UnityEngine;

namespace Runtime.Player {
    public class CinemachineAxisInput : MonoBehaviour, AxisState.IInputAxisProvider {
        public Vector2 input = Vector2.zero;
        public float GetAxisValue(int axis) {
            return input[axis];
        }
    }
}