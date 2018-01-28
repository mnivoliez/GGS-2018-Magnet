using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMagnet : MonoBehaviour {

	[SerializeField] private GameObject prefabMagnet;

	// Use this for initialization
	void Start () {
		
	}
	


	void OnMouseDown(){
		if (GameObject.Find("GameManager").GetComponent<GameManager>().sequenceState == SequenceState.Stopped)
		{
			GameObject magnet = Instantiate(prefabMagnet);
			magnet.transform.position = new Vector3(Random.Range(-31f, 31f), Random.Range(-8f, 16f), 0);
			GameObject.Find("GameManager").GetComponent<GameManager>().AddMagnet(magnet);
		}
	}
}
