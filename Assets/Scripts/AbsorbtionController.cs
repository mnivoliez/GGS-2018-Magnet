using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorbtionController : MonoBehaviour {
	
	private List<GameObject> objectsInAbsorbtion;
	private GameManager gameManager;
	[SerializeField]private float rangeDestroy = 0.05f;

	// Use this for initialization
	void Start () {
		objectsInAbsorbtion = new List<GameObject>();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (objectsInAbsorbtion.Count > 0)
		{
			List<GameObject> objectsToDestroy = new List<GameObject>();

			foreach (GameObject objectToDestroy in objectsInAbsorbtion)
			{
				if (objectToDestroy != null && (objectToDestroy.transform.position - transform.position).magnitude <= rangeDestroy)
				{
					gameManager.ReceivedBullet();
					objectsToDestroy.Add(objectToDestroy);
				}
			}
			
			if (objectsToDestroy.Count > 0)
			{
				foreach (GameObject objectToDestroy in objectsToDestroy)
				{
					if (objectToDestroy != null) {
						objectsInAbsorbtion.Remove(objectToDestroy);
						Destroy(objectToDestroy);
					}
					
				}
			}
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		GameObject objectCollided = collider.gameObject;
		if (objectCollided.tag == "Bullet")
		{
			Rigidbody body = objectCollided.GetComponent<Rigidbody>();
			body.velocity = (transform.position - objectCollided.transform.position).normalized * 2.5f;
			objectsInAbsorbtion.Add(objectCollided);
			body.useGravity = false;
		}
	}
}
