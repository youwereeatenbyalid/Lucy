using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConveyorUtils;
public class ConveyorObject : MonoBehaviour
{

    public delegate void DestroyConveyorObject(GameObject conveyorobject);

    public static event DestroyConveyorObject OnDestroyedConveyorObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDestroy()
    {
        if (OnDestroyedConveyorObject != null)
            OnDestroyedConveyorObject(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
