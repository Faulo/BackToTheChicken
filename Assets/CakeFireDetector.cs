using System.Collections;
using System.Collections.Generic;
using Runtime.Physics;
using UnityEngine;

public class CakeFireDetector : MonoBehaviour
{
    [SerializeField]
    GameObject firePrefab;

    void OnCollisionEnter(Collision collision) {
        Debug.Log(collision.gameObject);
        if (collision.gameObject.GetComponent<WindRecipient>() != null) {
            var fire = Instantiate(firePrefab, Vector3.zero, Quaternion.identity);
            fire.gameObject.transform.parent = collision.gameObject.transform;
            fire.transform.localPosition = Vector3.zero;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
