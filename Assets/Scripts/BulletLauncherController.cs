using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletLauncherStatus
{
    AwaintingAngle,
    Moving,
    AwaintingFiringOrder,
    WaitingForFireRate,
}
public class BulletLauncherController : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Transform _bulletOutput;
    [SerializeField] private Rigidbody _bulletPrefab;

    private BulletLauncherStatus _status;
    private float _targetAngle;

    [SerializeField] private float _intensity;

    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _fireRate;
    private float _timeSinceLastFire;

    private float _initialAngle = -90;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }


    // Use this for initialization
    void Start()
    {
        _status = BulletLauncherStatus.AwaintingAngle;
        transform.rotation = Quaternion.Euler(0, 0, _initialAngle);
        _timeSinceLastFire = 0f;

    }

    void FixedUpdate()
    {
        if (_status == BulletLauncherStatus.Moving)
        {
            float computedAngle = (_targetAngle + _initialAngle) * Time.fixedDeltaTime * _rotationSpeed;
            if (transform.rotation.z >= _targetAngle + _initialAngle)
            {
                transform.rotation = Quaternion.Euler(0, 0, _targetAngle + _initialAngle);
                _status = BulletLauncherStatus.WaitingForFireRate;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z + computedAngle);
            }

        }
        if (_status != BulletLauncherStatus.AwaintingFiringOrder)
        {
            _timeSinceLastFire += Time.fixedDeltaTime;
        }

        if (_status == BulletLauncherStatus.WaitingForFireRate && _timeSinceLastFire >= _fireRate)
        {
            _timeSinceLastFire = 0f;
            _status = BulletLauncherStatus.AwaintingFiringOrder;
        }
    }

    public void RotateTo(float angle)
    {
        _targetAngle = angle;
        _status = BulletLauncherStatus.Moving;
    }

    public GameObject LaunchBullet()
    {
        Rigidbody bullet = Instantiate(_bulletPrefab, _bulletOutput.position, Quaternion.identity);
        Vector3 direction = _bulletOutput.transform.position - transform.position;
        direction.Normalize();
        bullet.AddForce(direction * _intensity, ForceMode.Force);
        _audioSource.Play();
        _status = BulletLauncherStatus.AwaintingAngle;
        return bullet.gameObject;
    }

    public BulletLauncherStatus WhatIsTheStatus()
    {
        return _status;
    }

    public void Reset()
    {
        _status = BulletLauncherStatus.AwaintingAngle;
        transform.rotation = Quaternion.Euler(0, 0, _initialAngle);
        _timeSinceLastFire = 0f;

    }
}
