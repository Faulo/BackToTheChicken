using UnityEngine;

namespace Runtime.Physics {
    public class WindRecipient : MonoBehaviour {
        [SerializeField]
        Rigidbody attachedRigidbody = default;
        [SerializeField, Range(0, 100)]
        float maximumDrag = 10;
        [SerializeField]
        AnimationCurve dragOverWetness = new AnimationCurve();
        [SerializeField, Range(0, 1)]
        public float wetness = 0;
        public Vector3 position => transform.position;
        void OnValidate() {
            if (!attachedRigidbody) {
                TryGetComponent(out attachedRigidbody);
            }
        }
        public void AddForce(Vector3 force, Vector3 torque) {
            //Debug.Log($"Adding {force} to {this}");
            attachedRigidbody.AddForce(force, ForceMode.Force);
            attachedRigidbody.AddTorque(torque, ForceMode.Force);
        }
        void FixedUpdate() {
            wetness = Mathf.Clamp01(wetness);
            attachedRigidbody.drag = dragOverWetness.Evaluate(wetness) * maximumDrag;
        }
    }
}