using UnityEngine;

namespace Runtime.Player {
    public class FeatherController : MonoBehaviour {
        [SerializeField]
        Rigidbody attachedRigidbody = default;
        [SerializeField]
        public bool isMain = false;
        [SerializeField]
        public FeatherController targetFeather = default;
        [SerializeField]
        AnimationCurve gravityOverDistance = default;
        void Awake() {
            OnValidate();
        }
        void OnValidate() {
            if (!attachedRigidbody) {
                TryGetComponent(out attachedRigidbody);
            }
        }
        void FixedUpdate() {
            if (targetFeather) {
                var distance = targetFeather.transform.position - transform.position;
                attachedRigidbody.AddForce(distance.normalized * gravityOverDistance.Evaluate(distance.magnitude) * Time.deltaTime, ForceMode.Force);
            }
        }
    }
}
