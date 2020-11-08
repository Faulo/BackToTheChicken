using UnityEngine;

namespace Runtime {
    public class RotateAroundAxis : MonoBehaviour {
        [SerializeField]
        public float degreePerSecond;
        [SerializeField]
        public Vector3 axis;
        // Start is called before the first frame update
        void Start() {

        }

        void Update() {
            transform.Rotate(axis, degreePerSecond * Time.deltaTime);
        }
    }
}