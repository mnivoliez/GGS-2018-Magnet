using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

	private Rigidbody body;
	[SerializeField] private Vector3 direction;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody>();
		body.AddForce(direction.normalized * 100);
	}
	
	// Update is called once per frame
	void Update () {
		//transform.position = new Vector3(transform.position.x + 0.05f, transform.position.y, transform.position.z);
	}
}
