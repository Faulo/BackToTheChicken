using UnityEngine;

public class MovementTest : MonoBehaviour {

    [HideInInspector] public new Rigidbody rigidbody;

    public bool useGravity = true;

    // Update is called once per frame
    void Update() {
        transform.Translate(Vector3.forward * Time.deltaTime);
    }

    void Awake() {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        rigidbody.useGravity = false;
        if (useGravity) {
            rigidbody.AddForce(Physics.gravity * (rigidbody.mass * rigidbody.mass) * 0.01f);
        }
    }
}
