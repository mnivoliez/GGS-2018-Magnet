using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DirectionForce
{
    Attract, Repulse
};

public class MagnetEffect : MonoBehaviour
{

    private List<GameObject> objectsInRange;
    private DirectionForce directionForce;
    private float intensity;
    private Material attractAreaMaterial;
    private Material attractCoreMaterial;
    private Material repulseAreaMaterial;
    private Material repulseCoreMaterial;
    private MeshRenderer meshRend;
    private Material[] matsArea;
    private Material[] matsCore;


    // Use this for initialization
    void Start()
    {
        objectsInRange = new List<GameObject>();
        meshRend = GetComponent<MeshRenderer>();
        attractAreaMaterial = Resources.Load("Red_nain_transparent") as Material;
        attractCoreMaterial = Resources.Load("Red_nain") as Material;
        repulseAreaMaterial = Resources.Load("Blue_nain_transparent") as Material;
        repulseCoreMaterial = Resources.Load("Blue_nain") as Material;
        matsArea = GetComponent<Renderer>().materials;
        matsArea[0] = attractAreaMaterial;
        GetComponent<Renderer>().materials = matsArea;
        matsCore = transform.parent.Find("Core").GetComponent<Renderer>().materials;
        matsCore[0] = attractCoreMaterial;
        transform.parent.Find("Core").GetComponent<Renderer>().materials = matsCore;
    }


    void Update()
    {
        if (matsArea[0].name != "Red_nain_transparent" && directionForce == DirectionForce.Attract)
        {
            matsArea[0] = attractAreaMaterial;
            GetComponent<Renderer>().materials = matsArea;
            matsCore[0] = attractCoreMaterial;
            transform.parent.Find("Core").GetComponent<Renderer>().materials = matsCore;
        }
        else if (matsArea[0].name != "Blue_nain_transparent" && directionForce == DirectionForce.Repulse)
        {
            matsArea[0] = repulseAreaMaterial;
            GetComponent<Renderer>().materials = matsArea;
            matsCore[0] = repulseCoreMaterial;
            transform.parent.Find("Core").GetComponent<Renderer>().materials = matsCore;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (GameObject objectInRange in objectsInRange)
        {
            if (objectInRange != null)
            {
                Rigidbody body = objectInRange.GetComponent<Rigidbody>();
                Vector3 vectorForce;
                if (directionForce == DirectionForce.Attract)
                {
                    vectorForce = (transform.position - objectInRange.transform.position).normalized;
                }
                else
                {
                    vectorForce = (objectInRange.transform.position - transform.position).normalized;
                }

                body.AddForce(vectorForce * intensity);
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        GameObject objectCollided = collider.gameObject;
        if (objectCollided.tag == "Bullet")
        {

            objectsInRange.Add(objectCollided);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        GameObject objectExited = collider.gameObject;

        if (objectsInRange.Contains(objectExited))
        {
            objectsInRange.Remove(objectExited);
        }
    }



    public DirectionForce DirectionForce
    {
        get
        {
            return directionForce;
        }
        set
        {
            directionForce = value;
        }
    }

    public float Intensity
    {
        get
        {
            return intensity;
        }
        set
        {
            intensity = value;
        }
    }
}
