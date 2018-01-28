using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutoManager : MonoBehaviour {

	[SerializeField] private GameObject tuto1;
	[SerializeField] private GameObject tuto2;
	[SerializeField] private GameObject tuto31;
	[SerializeField] private GameObject tuto32;
	[SerializeField] private GameObject tuto33;
	[SerializeField] private GameObject tuto4;
	[SerializeField] private GameObject tuto5;

	private List<GameObject> tutos;

	// Use this for initialization
	void Start () {
		tutos = new List<GameObject>();
		
		tutos.Add(tuto1);
		tutos.Add(tuto2);
		tutos.Add(tuto31);
		tutos.Add(tuto32);
		tutos.Add(tuto33);
		tutos.Add(tuto4);
		tutos.Add(tuto5);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Next()
	{
		if (tutos.Count > 1)
		{
			tutos[0].SetActive(false);
			tutos.Remove(tutos[0]);
		} else {
			SceneManager.LoadScene("MainMenu");
		}
	}
}
