using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

	private Rigidbody body;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody>();
		body.AddForce(transform.rotation * Vector3.right * 100);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
