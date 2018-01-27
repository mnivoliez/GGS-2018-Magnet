using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLauncherController : MonoBehaviour {
	[SerializeField] private GameManager _gameManager;
	[SerializeField] private Transform _bulletOutput;
	[SerializeField] private Rigidbody _bulletPrefab;
	[SerializeField] [Range(-90, 90)] private List<float> _angles;

	[SerializeField] private float _intensity;

	[SerializeField] private float _bulletRate;
	private float _timeSinceLastShot;

	// Use this for initialization
	void Start () {
		_timeSinceLastShot = 0f;
		_gameManager.PlanShotsNumber(_angles.Count);
		transform.rotation = Quaternion.Euler(0, 0, -90);
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate() {
		if (_angles.Count > 0 && _timeSinceLastShot > _bulletRate) {
			_timeSinceLastShot = 0f;
			LaunchBullet();
		} else {
			_timeSinceLastShot += Time.fixedDeltaTime;
		}
	}

	private void LaunchBullet () {
		transform.rotation = Quaternion.Euler(0, 0, _angles[0] -90);
		Rigidbody bullet = Instantiate(_bulletPrefab, _bulletOutput.position, Quaternion.identity);
		Vector3 direction = _bulletOutput.transform.position - transform.position;
		direction.Normalize();
		bullet.AddForce(direction * _intensity, ForceMode.Force);
		_angles.RemoveAt(0);
		_gameManager.LaunchedBullet();
	}
}
