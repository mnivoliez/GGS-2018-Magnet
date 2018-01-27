using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	private int _launch;
	private int _received;

	private int _shotsNumber;

	[SerializeField] private float _timeout;
	private float _effectiveTimeout;
	private bool _startTimeout;

	// Use this for initialization
	void Start () {
		_launch = 0;
		_received = 0;
		_shotsNumber = 0;
		_effectiveTimeout = 0f;
		_startTimeout = false;
	}

	void FixedUpdate() {
		if(_startTimeout) {
			_effectiveTimeout += Time.fixedDeltaTime;			
		}
		if(_received != _shotsNumber && _effectiveTimeout > _timeout) {
			Lose();
		}
		if(_received == _shotsNumber && _effectiveTimeout < _timeout) {
			Win();
		}
	}
	
	public void PlanShotsNumber (int number) {
		_shotsNumber = number;
	}

	public void LaunchedBullet() {
		_launch++;
		if (_launch == _shotsNumber) {
			_startTimeout = true;
		}
	}

	public void ReceivedBullet () {
		_received++;
	}

	private void Win() {
		Debug.Log("winner");
		
	}

	private void Lose() {
		Debug.Log("loser");
	}
}
