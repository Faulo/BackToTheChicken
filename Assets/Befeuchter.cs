using System.Collections;
using System.Collections.Generic;
using Runtime.Physics;
using UnityEngine;

public class Befeuchter : MonoBehaviour
{
    [SerializeField]
    float wetPerSecond = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other) {
        var comp = other.GetComponentInParent<WindRecipient>();
        if (comp != null) {
            comp.wetness -= wetPerSecond * Time.deltaTime;
        }
    }
}
