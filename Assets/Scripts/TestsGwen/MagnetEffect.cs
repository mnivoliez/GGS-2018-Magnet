using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum DirectionForce
{
	Attract, Repulse
};

public class MagnetEffect : MonoBehaviour {

	private List<GameObject> objectsInRange;
	[SerializeField] private DirectionForce directionForce;
	[SerializeField] private float intensity = 2;

	// Use this for initialization
	void Start () {
		objectsInRange = new List<GameObject>();
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

	public void NextType() {
		directionForce = 
			directionForce == DirectionForce.Attract ? 
				DirectionForce.Repulse : DirectionForce.Attract;
	}
}
