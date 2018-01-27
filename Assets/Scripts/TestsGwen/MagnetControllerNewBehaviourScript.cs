using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MagnetChild {
	AreaEffect,
	Core
}
public class MagnetControllerNewBehaviourScript : MonoBehaviour {

	private MagnetEffect magnetEffect;
	[SerializeField] private DirectionForce directionForce;
	[SerializeField] private float intensity = 2;

	// Use this for initialization
	void Start () {
		magnetEffect = transform.Find(MagnetChild.AreaEffect.ToString()).gameObject.GetComponent<MagnetEffect>();
		magnetEffect.DirectionForce = this.directionForce;
		magnetEffect.Intensity = this.intensity;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
