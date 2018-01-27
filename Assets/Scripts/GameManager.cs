using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	private int _launch;
	private int _received;

	private int _shotsNumber;

	// Use this for initialization
	void Start () {
		_launch = 0;
		_received = 0;
		_shotsNumber = 0;
	}

	void FixedUpdate() {
		if(_received == _shotsNumber) {
			Win();
		}
	}
	
	public void PlanShotsNumber (int number) {
		_shotsNumber = number;
	}

	public void LaunchedBullet() {
		_launch++;
	}

	public void ReceivedBullet () {
		_received++;
	}

	private void Win() {
		Debug.Log("winner");

	}
}
