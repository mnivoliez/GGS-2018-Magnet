using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MagnetChild {
	AreaEffect,
	Core
}
public class MagnetControllerNewBehaviourScript : MonoBehaviour {

	private MagnetEffect magnetEffect;


	// Use this for initialization
	void Start () {
		magnetEffect = transform.Find(MagnetChild.AreaEffect.ToString()).gameObject.GetComponent<MagnetEffect>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
