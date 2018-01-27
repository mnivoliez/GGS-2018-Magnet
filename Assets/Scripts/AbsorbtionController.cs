using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorbtionController : MonoBehaviour {
	
	private List<GameObject> objectsInAbsorbtion;
	[SerializeField]private float rangeDestroy = 0.05f;

	[SerializeField] private GameManager _gameManager;

	// Use this for initialization
	void Start () {
		objectsInAbsorbtion = new List<GameObject>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (objectsInAbsorbtion.Count > 0)
		{
			List<GameObject> objectsToDestroy = new List<GameObject>();

			foreach (GameObject objectToDestroy in objectsInAbsorbtion)
			{
				if ((objectToDestroy.transform.position - transform.position).magnitude <= rangeDestroy)
				{
					objectsToDestroy.Add(objectToDestroy);
					_gameManager.ReceivedBullet();
				}
			}
			
			if (objectsToDestroy.Count > 0)
			{
				foreach (GameObject objectToDestroy in objectsToDestroy)
				{
					objectsInAbsorbtion.Remove(objectToDestroy);
					Destroy(objectToDestroy);
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
			body.useGravity = false;
			body.velocity = (transform.position - objectCollided.transform.position).normalized;
			objectsInAbsorbtion.Add(objectCollided);
		}
	}
}
