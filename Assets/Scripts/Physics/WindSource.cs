using System;
using UnityEngine;

namespace Runtime.Physics {
    public class WindSource : MonoBehaviour {
        public event Action<Collider> onColliderEnter;

        [SerializeField]
        CapsuleCollider attachedCollider = default;
        [SerializeField, Range(-1, 1)]
        public float strength = 1;
        [SerializeField]
        public Vector3 direction = Vector3.up;
        [SerializeField, Range(0, 10000)]
        public float force = 1000;
        [SerializeField, Range(0, 10000)]
        float torque = 10;
        [SerializeField, Range(0, 1)]
        float randomness = 0;
        [SerializeField, Range(-1, 1)]
        float wetness = 0;
        [SerializeField]
        AnimationCurve strengthOverRadius = default;
        public Vector3 windCenter => attachedCollider
            ? transform.position + (transform.rotation * attachedCollider.center)
            : transform.position;
        public Vector3 windDirection => transform.rotation * direction;
        public Ray windRay => new Ray(windCenter, windDirection);
        public float radius {
            get => attachedCollider
                ? attachedCollider.radius
                : 0;
            set => attachedCollider.radius = value;
        }
        public float windHeight => attachedCollider
            ? attachedCollider.height
            : 0;
        void Awake() {
            OnValidate();
        }
        void OnValidate() {
            if (!attachedCollider) {
                TryGetComponent(out attachedCollider);
            }
        }
        void OnTriggerEnter(Collider other) {
            onColliderEnter?.Invoke(other);
        }
        void OnTriggerStay(Collider other) {
            if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent<WindRecipient>(out var recipient)) {
                recipient.AddForce(CalculateForce(recipient), CalculateTorque(recipient));
                recipient.wetness += CalculateWetness(recipient);
            }
        }
        Vector3 CalculateForce(WindRecipient recipient) {
            var distance = recipient.position - windCenter;
            float radius = distance.magnitude / (this.radius + windHeight);
            float multiplier = Time.deltaTime;
            multiplier *= force;
            multiplier *= strengthOverRadius.Evaluate(radius);
            var direction = windDirection;
            direction = Vector3.Lerp(direction, UnityEngine.Random.insideUnitSphere, randomness);
            return direction * multiplier;
        }
        Vector3 CalculateTorque(WindRecipient recipient) {
            var distance = recipient.position - windCenter;
            float radius = distance.magnitude / (this.radius + windHeight);
            float multiplier = Time.deltaTime;
            multiplier *= torque;
            multiplier *= strengthOverRadius.Evaluate(radius);
            return distance * multiplier;
        }
        float CalculateWetness(WindRecipient recipient) {
            var distance = recipient.position - windCenter;
            float radius = distance.magnitude / (this.radius + windHeight);
            float multiplier = Time.deltaTime;
            multiplier *= radius;
            return wetness * multiplier;
        }
        void OnDrawGizmos() {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(windCenter, windCenter + windDirection);
        }
    }
}