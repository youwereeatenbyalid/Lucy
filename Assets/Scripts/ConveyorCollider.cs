using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class ConveyorCollider : MonoBehaviour
{
    public Material materialtype;
    public Mesh meshtype;

    public delegate void RecievedObject(float value);

    public static event RecievedObject OnRecievedObject;

    public AudioSource deleteSoundSource;

    public bool emptyMeshValid = true;
    public bool emptyMatValid = true;




    // Start is called before the first frame update
    void Start()
    {
        deleteSoundSource = GetComponent<AudioSource>();
    }
    public virtual void OnTriggerEnter(Collider other)
    {
        GameObject parentColliderObject;
        if (other.gameObject.transform.parent != null)
        {
            parentColliderObject = other.gameObject.transform.parent.gameObject;
        }
        else
        {
            print("no parent found");
            return;
        }

        print(other.gameObject.name);
        if (other.gameObject.name == "ConveyorObjectModel")
        {
            if (MeshMatch(other.gameObject) && MaterialMatch(other.gameObject))
            {
                OnMatch(1.0f);
            }
            else if (MeshMatch(other.gameObject) || MaterialMatch(other.gameObject))
            {
                OnMatch(0.5f);
            }
            else
            {
                OnMatch(0.0f);
            }
            Destroy(parentColliderObject);
            deleteSoundSource.Play();
        }
    }

    public void OnMatch(float score)
    {
        print("Match: " + score);
        if (OnRecievedObject != null)
        {
            OnRecievedObject(score);
        }
    }

    protected bool MeshMatch(GameObject gameObject)
    {
        if (meshtype)
        {
            MeshFilter objectmeshfilter = gameObject.GetComponent<MeshFilter>();
            if (objectmeshfilter != null)
            {
                return objectmeshfilter.sharedMesh == meshtype;
            }
            return false;
        }
        return emptyMeshValid;
    }

    protected bool MaterialMatch(GameObject gameObject)
    {
        if (materialtype)
        {

            MeshRenderer objectmeshRenderer = gameObject.GetComponent<MeshRenderer>();
            if (objectmeshRenderer != null)
            {
                return objectmeshRenderer.sharedMaterial.name == materialtype.name;
            }
            return false;
        }
        return emptyMatValid;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
