using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketScript :  ConveyorCollider
{


    public Material fullmatch;
    public Material halfmatch;
    public Material nomatch;
    public Material neutral;

    public Valve.VR.InteractionSystem.PlaySound fullmatch_sound;
    public Valve.VR.InteractionSystem.PlaySound halfmatch_sound;
    public Valve.VR.InteractionSystem.PlaySound nomatch_sound;
    public GameObject reference;
    public GameObject backwall;

    float collidecooldown = 1.0f;
    bool iscooldown = false;
    float cooldowntimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (meshtype) {
            reference.GetComponent<MeshFilter>().mesh = meshtype;
        }
        if(materialtype)
        {
            reference.GetComponent<MeshRenderer>().material = materialtype;
            backwall.GetComponent<MeshRenderer>().material = materialtype;
        }
        if (!meshtype && !materialtype)
            reference.SetActive(false);
    }

    public override void OnTriggerEnter(Collider other)  {
        GameObject parentColliderObject;
        if (other.gameObject.transform.parent != null) { 
            parentColliderObject = other.gameObject.transform.parent.gameObject;
        }
        else
        {
            print("no parent found");
            return;
        }

        print(other.gameObject.name);
        if (other.gameObject.name == "ConveyorObjectModel") {
            if (MeshMatch(other.gameObject) && MaterialMatch(other.gameObject))
            {
                OnMatch(1.0f, fullmatch);
                fullmatch_sound.Play();
            }
            else if (MeshMatch(other.gameObject) || MaterialMatch(other.gameObject))
            {
                OnMatch(0.5f, halfmatch);
                halfmatch_sound.Play();
            }
            else
            {
                OnMatch(0.0f,nomatch);
                nomatch_sound.Play();
            }
            Destroy(parentColliderObject);
        }
    }

    public void OnMatch(float score,Material type)
    {
        this.GetComponent<MeshRenderer>().material = type;
        iscooldown = true;
        cooldowntimer = 0;
        OnMatch(score);
    }

    // Update is called once per frame
    void Update()
    {

        if(iscooldown && cooldowntimer < collidecooldown)
        {
            cooldowntimer += Time.deltaTime;
        }else if (iscooldown)
        {
            this.GetComponent<MeshRenderer>().material = neutral;
            iscooldown = false;
        }
    }
}
