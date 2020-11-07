using UnityEngine;

namespace Runtime.Physics {
    public class WindRecipient : MonoBehaviour {
        [SerializeField]
        Rigidbody attachedRigidbody = default;
        public Vector3 position => transform.position;
        void OnValidate() {
            if (!attachedRigidbody) {
                TryGetComponent(out attachedRigidbody);
            }
        }
        public void AddForce(Vector3 force, Vector3 torque) {
            attachedRigidbody.AddForce(force, ForceMode.Force);
            attachedRigidbody.AddTorque(torque, ForceMode.Force);
        }
    }
}