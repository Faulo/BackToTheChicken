using UnityEngine;

namespace Runtime.Physics {
    public class WindParticles : MonoBehaviour {
        [SerializeField]
        WindSource attachedWind = default;
        [SerializeField]
        ParticleSystem attachedParticles = default;
        [SerializeField]
        ParticleSystemForceField attachedForceField = default;

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
            if (attachedParticles) {
                var shape = attachedParticles.shape;
                shape.shapeType = ParticleSystemShapeType.Hemisphere;
                shape.arc = 360;
                shape.radius = attachedWind.windRadius;
                shape.position = Vector3.down * attachedWind.windHeight / 2;
                shape.rotation = new Vector3(90, 0, 0);
            }
            if (attachedForceField) {
                attachedForceField.shape = ParticleSystemForceFieldShape.Cylinder;
                attachedForceField.startRange = 0;
                attachedForceField.endRange = attachedWind.windRadius;
                attachedForceField.length = attachedWind.windHeight + attachedWind.windRadius;
                attachedForceField.directionX = attachedWind.windDirection.x;
                attachedForceField.directionY = attachedWind.windDirection.y;
                attachedForceField.directionZ = attachedWind.windDirection.z;
            }
        }
    }
}