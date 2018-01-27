using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMagnet : MonoBehaviour {

	[SerializeField] private GameObject prefabMagnet;

	// Use this for initialization
	void Start () {
		
	}
	


	void OnMouseDown(){
		Instantiate(prefabMagnet);
	}
}
