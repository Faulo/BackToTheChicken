using UnityEngine;

namespace Runtime.Physics {
    public class WindParticles : MonoBehaviour {
        [SerializeField]
        WindSource attachedWind = default;
        [SerializeField]
        ParticleSystem attachedParticles = default;
        [SerializeField]
        ParticleSystemForceField attachedForceField = default;
        [SerializeField, Range(0, 100)]
        float particleEmissionRate = 5;
        [SerializeField]
        AnimationCurve particlesOverStrength = new AnimationCurve();
        [SerializeField, Range(0, 100)]
        float forceFieldStrength = 1;
        [SerializeField]
        AnimationCurve forceFieldOverStrength = new AnimationCurve();

        void Awake() {
            OnValidate();
        }
        void OnValidate() {
            if (!attachedWind) {
                attachedWind = GetComponentInParent<WindSource>();
            }
            if (!attachedParticles) {
                TryGetComponent(out attachedParticles);
            }
            if (!attachedForceField) {
                TryGetComponent(out attachedForceField);
            }
            UpdateParticles();
            UpdateForceField();
        }
        void Update() {
            UpdateParticles();
            UpdateForceField();
        }
        void UpdateParticles() {
            if (attachedWind && attachedParticles) {
                var shape = attachedParticles.shape;
                shape.shapeType = ParticleSystemShapeType.Hemisphere;
                shape.arc = 360;
                shape.rotation = new Vector3(90, 0, 0);
                var emission = attachedParticles.emission;
                emission.rateOverTime = particleEmissionRate * particlesOverStrength.Evaluate(Mathf.Abs(attachedWind.strength));
            }
        }
        void UpdateForceField() {
            if (attachedWind && attachedForceField) {
                attachedForceField.shape = ParticleSystemForceFieldShape.Cylinder;
                attachedForceField.startRange = 0;
                attachedForceField.endRange = attachedWind.radius * 2;
                attachedForceField.length = attachedWind.windHeight + attachedWind.radius;
                var force = attachedWind.windDirection * forceFieldStrength * forceFieldOverStrength.Evaluate(Mathf.Abs(attachedWind.strength));
                attachedForceField.directionX = force.x;
                attachedForceField.directionY = force.y;
                attachedForceField.directionZ = force.z;
            }
        }
    }
}