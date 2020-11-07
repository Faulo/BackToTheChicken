using UnityEngine;

namespace Runtime {
    public class RotateAroundAxis : MonoBehaviour {
        [SerializeField]
        float degreePerSecond;
        [SerializeField]
        Vector3 axis;
        // Start is called before the first frame update
        void Start() {

        }

        void Update() {
            transform.RotateAround(axis, degreePerSecond * Time.deltaTime);
        }
    }
}