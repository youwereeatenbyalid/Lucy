using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        var parentColliderObject = other.gameObject.transform.parent.gameObject;
        print(other.gameObject.name);
        if (other.gameObject.name == "ConveyorObjectModel") {
            Destroy(parentColliderObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
