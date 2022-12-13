using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ConveyorUtils;
public class SpawnerScript : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject myPrefab;
    public Material[] colors;
    public Mesh[] meshes;
    public Valve.VR.SteamVR_Skeleton_Pose[] poses;
    public List<GameObject> conveyorObjects = new List<GameObject>();
    public LinearSlider spawnrateslider;
    public float spawnfrequency = 3.0f;
    public float spawnvariance = 0.5f;
    public float spawntimer = 0.0f;
    public float nextspawn = 5.0f;
    public float idealnumberobjects = 10f;
    public float responsiveness = 1.0f;
    public float currentmodifier = 0.0f;
    int matindex;
    int meshindex;


    void Start()
    {
        //nextspawn = Random.Range(spawnInterval.x, spawnInterval.y);
    }
    private void OnEnable()
    {
        ConveyorObject.OnDestroyedConveyorObject += RemoveFromList;
        ScoreScript.OnGameStateChange += CleanUpObjects;
        spawnrateslider.OnHandleUpdate += SpawnRateChange;
    }


    void SpawnRateChange(float value) {
        spawnfrequency = (1-value) * 3;
    }

    private void OnDisable()
    {
        ConveyorObject.OnDestroyedConveyorObject -= RemoveFromList;
        ScoreScript.OnGameStateChange -= CleanUpObjects;
        spawnrateslider.OnHandleUpdate -= SpawnRateChange;
    }

    void RemoveFromList(GameObject gameobject)
    {
        conveyorObjects.Remove(gameobject);
    }

    void CleanUpObjects(ScoreScript.GameState state)
    {
        print(state);
        if(state != ScoreScript.GameState.Pause && state != ScoreScript.GameState.Running)
        {
            foreach (var item in conveyorObjects)
            {
                Destroy(item);
            }
        }
    }

    float getConveyorCapacity()
    {
        return (float)conveyorObjects.Count / idealnumberobjects;
    }
  

    float newdifficultycurve(float percentdone,float currentgrade, float percentmax)
    {
        print("percent done" +percentdone);
        print("currentgrade" +currentgrade);
        print("percent max" + percentmax);
        print((-1f * percentdone) - (1.3f * Mathf.Pow(currentgrade, 3f)) + (0.7f * Mathf.Pow(percentmax, 2f)) + spawnfrequency);
        return (-1f * percentdone) - (1.3f * Mathf.Pow(currentgrade, 3f)) + (0.7f * Mathf.Pow(percentmax, 2f) )+ spawnfrequency;
    }

    // Update is called once per frame
    void Update()
    {
        if(ScoreScript.state == ScoreScript.GameState.Running) { 
        spawntimer += Time.deltaTime;
            if (spawntimer > nextspawn) {
                matindex = Random.Range(0, colors.Length);
                meshindex = Random.Range(0, meshes.Length);

                spawntimer = 0;
                currentmodifier = newdifficultycurve(ScoreScript.getPercentDone(), ScoreScript.GetRecentScore()/100f, getConveyorCapacity());
                nextspawn = Random.Range(0.0f, 0.5f) + currentmodifier*responsiveness;
                if (nextspawn < 0.1f)
                    nextspawn = 0.1f;

                var spawnposition = transform.position + new Vector3(Random.Range(0.0f,spawnvariance)-spawnvariance/2, 0.0f, Random.Range(0.0f, spawnvariance) - spawnvariance / 2);
                var gameobject = Instantiate(myPrefab, spawnposition, transform.rotation);
                conveyorObjects.Add(gameobject);

                var objectmeshrenderer = gameobject.GetComponentInChildren(typeof(MeshRenderer), false) as MeshRenderer;
                var objectmeshfilter = gameobject.GetComponentInChildren(typeof(MeshFilter)) as MeshFilter;
                var objectmeshcollider = gameobject.GetComponentInChildren(typeof(MeshCollider)) as MeshCollider;

                var poser = gameobject.GetComponent<Valve.VR.SteamVR_Skeleton_Poser>();
                poser.skeletonMainPose = poses[meshindex];
                objectmeshrenderer.material = colors[matindex];
                objectmeshfilter.mesh = meshes[meshindex];
                objectmeshcollider.sharedMesh = meshes[meshindex];
            }
        }
    }



}
