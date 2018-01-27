using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum SequenceState
{
    Running,
    Stopped
}

public class GameManager : MonoBehaviour
{
    private List<GameObject> _bullets;
    private int _received;

    [SerializeField] [Range(-90, 90)] private List<float> _bulletsShootingAngle;
    private List<float>.Enumerator _currentAngle;

    private SequenceState _sequenceState;

    [SerializeField] private BulletLauncherController _bulletLauncherController;

    // Use this for initialization
    void Start()
    {
        _received = 0;
        _bullets = new List<GameObject>();
        _currentAngle = _bulletsShootingAngle.GetEnumerator();
        _sequenceState = SequenceState.Stopped;
    }

    void FixedUpdate()
    {
        if (_sequenceState == SequenceState.Running)
        {
            switch (_bulletLauncherController.WhatIsTheStatus())
            {
                case BulletLauncherStatus.AwaintingAngle:
                    if (_currentAngle.MoveNext())
                        _bulletLauncherController.RotateTo(_currentAngle.Current);

                    break;
                case BulletLauncherStatus.AwaintingFiringOrder:
                    _bullets.Add(_bulletLauncherController.LaunchBullet());
                    break;
                case BulletLauncherStatus.Moving:
                    break;
            }
        }
    }

    public void ReceivedBullet()
    {
        _received++;
        if (_bullets.Count == 0 && _bulletsShootingAngle.Count == _received) Win();
    }

    public void Play()
    {
        _sequenceState = SequenceState.Running;
    }

    public void Stop()
    {
        foreach (var b in _bullets)
        {
            Destroy(b);
        }
        _sequenceState = SequenceState.Stopped;
        _bulletLauncherController.Reset();
        _currentAngle = _bulletsShootingAngle.GetEnumerator();
    }

    private void Win()
    {
        Debug.Log("winner");

    }
}
