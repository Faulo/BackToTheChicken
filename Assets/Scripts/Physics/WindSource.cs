using UnityEngine;

namespace Runtime.Physics {
    public class WindSource : MonoBehaviour {
        [SerializeField]
        CapsuleCollider attachedCollider = default;
        [SerializeField, Range(0, 10000)]
        float force = 1000;
        [SerializeField, Range(0, 10000)]
        float torque = 10;
        [SerializeField, Range(0, 1)]
        float randomness = 0;
        [SerializeField]
        AnimationCurve strengthOverRadius = default;
        public Vector3 windCenter => attachedCollider
            ? transform.position + attachedCollider.center
            : transform.position;
        public Vector3 windDirection => attachedCollider
             ? attachedCollider.direction switch
             {
                 0 => Vector3.right,
                 1 => Vector3.up,
                 2 => Vector3.forward,
                 _ => throw new System.NotImplementedException()
             } : Vector3.zero;
        public Ray windRay => new Ray(windCenter, windDirection);
        public float windRadius => attachedCollider
            ? attachedCollider.radius
            : 0;
        public float windHeight => attachedCollider
            ? attachedCollider.height
            : 0;
        void OnValidate() {
            if (!attachedCollider) {
                TryGetComponent(out attachedCollider);
            }
            if (attachedCollider) {
                attachedCollider.direction = 1;
            }
        }
        void OnTriggerStay(Collider other) {
            if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent<WindRecipient>(out var recipient)) {
                recipient.AddForce(CalculateForce(recipient), CalculateTorque(recipient));
            }
        }
        Vector3 CalculateForce(WindRecipient recipient) {
            var distance = Vector3.Cross(windDirection, recipient.position - windCenter);
            float radius = distance.magnitude / windRadius;
            float multiplier = Time.deltaTime;
            multiplier *= force;
            multiplier *= strengthOverRadius.Evaluate(radius);
            var direction = windDirection;
            direction = Vector3.Lerp(direction, Random.insideUnitSphere, randomness);
            return direction * multiplier;
        }
        Vector3 CalculateTorque(WindRecipient recipient) {
            var distance = Vector3.Cross(windDirection, recipient.position - windCenter);
            float radius = distance.magnitude / windRadius;
            float multiplier = Time.deltaTime;
            multiplier *= torque;
            multiplier *= strengthOverRadius.Evaluate(radius);
            return distance * multiplier;
        }
        void OnDrawGizmosSelected() {
            Gizmos.color = Color.green;
            var extend = transform.rotation * windDirection * windHeight;
            Gizmos.DrawLine(windCenter - extend, windCenter + extend);
        }
    }
}