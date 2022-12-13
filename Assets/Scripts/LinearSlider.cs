using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
public class LinearSlider : LinearMapping
{

    public delegate void HandleUpdate(float value);
    public event HandleUpdate OnHandleUpdate;
    private float oldvalue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (oldvalue != value)
        {
            oldvalue = value;
            if (OnHandleUpdate != null)
                OnHandleUpdate(oldvalue);
        }
    }
}
