using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(AudioSource))]
public class DuckOnPause : MonoBehaviour
{
    AudioSource source;
    float decreaseto = 0.7f;
    float original;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.ignoreListenerPause = true;
        original = source.dopplerLevel;
    }

    // Update is called once per frame
    void Update()
    {
        if (AudioListener.pause)
        {
            source.volume = original * decreaseto;
        }
        else
        {
            source.volume = original;
        }
    }
}
