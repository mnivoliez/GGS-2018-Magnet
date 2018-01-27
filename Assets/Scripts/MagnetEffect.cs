using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DirectionForce
{
	Attract, Repulse
};

public class MagnetEffect : MonoBehaviour {

	private List<GameObject> objectsInRange;
	private DirectionForce directionForce;
	private float intensity;
	private Material attractMaterial;
	private Material repulseMaterial;
	private MeshRenderer meshRend;
	private Material[] mats;

	// Use this for initialization
	void Start () {
		objectsInRange = new List<GameObject>();
		meshRend = GetComponent<MeshRenderer>();
		attractMaterial = Resources.Load("RedTransparent") as Material;
		repulseMaterial = Resources.Load("BlueTransparent") as Material;
		mats = GetComponent<Renderer>().materials;
		mats[0] = attractMaterial;
		GetComponent<Renderer>().materials = mats;
	}


	void Update()
	{
		if (mats[0].name != "RedTransparent" && directionForce == DirectionForce.Attract)
		{
			mats[0] = attractMaterial;
			GetComponent<Renderer>().materials = mats;
		}
		else if (mats[0].name != "BlueTransparent" && directionForce == DirectionForce.Repulse)
		{
			mats[0] = repulseMaterial;
			GetComponent<Renderer>().materials = mats;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		foreach (GameObject objectInRange in objectsInRange)
		{
			Rigidbody body = objectInRange.GetComponent<Rigidbody>();
			Vector3 vectorForce;
			if (directionForce == DirectionForce.Attract)
			{
				vectorForce = (transform.position - objectInRange.transform.position).normalized;
			} else {
				vectorForce = (objectInRange.transform.position - transform.position).normalized;
			}

			body.AddForce(vectorForce * intensity);
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
