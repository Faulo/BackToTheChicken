using UnityEngine;

namespace Runtime.Physics {
    public class CakeFireDetector : MonoBehaviour {
        [SerializeField]
        GameObject firePrefab = default;

        void OnTriggerEnter(Collider collider) {
            if (collider.attachedRigidbody && collider.attachedRigidbody.TryGetComponent<WindRecipient>(out var recipient)) {
                Instantiate(firePrefab, recipient.transform);
            }
        }
    }
}
