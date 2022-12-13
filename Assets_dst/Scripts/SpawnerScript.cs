using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject myPrefab;
    public Material[] cubecolor;
    public float spawnInterval = 1.0f;
    float spawntimer = 0.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        spawntimer += Time.deltaTime;
        if (spawntimer > spawnInterval) {
            float selection = Random.value;
            spawntimer = 0;
            var gameobject = Instantiate(myPrefab, transform.position, transform.rotation);
            var objectmeshrenderer = gameobject.GetComponentInChildren(typeof(MeshRenderer), false) as MeshRenderer;
        }

    }
}
