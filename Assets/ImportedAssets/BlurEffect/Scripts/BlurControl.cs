using UnityEngine;
using System.Collections;

public class BlurControl : MonoBehaviour {
	
	[SerializeField] private float value = 20f; 
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.GetComponent<Renderer>().material.SetFloat("_blurSizeXY",value);
	}
	
}
