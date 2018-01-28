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

    private List<GameObject> magnetsCreated;

    [SerializeField] private BulletLauncherController _bulletLauncherController;
    [SerializeField] private GameObject menuLevel;
    [SerializeField] private GameObject blurPlane;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject nextLevelButton;
    [SerializeField] float timeOnFloat;

    // Use this for initialization
    void Start()
    {
        _received = 0;
        _bullets = new List<GameObject>();
        _currentAngle = _bulletsShootingAngle.GetEnumerator();
        _sequenceState = SequenceState.Stopped;
        magnetsCreated = new List<GameObject>();
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
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
        if (_bulletsShootingAngle.Count == _received) Win();
    }

    public void Play()
    {
        _sequenceState = SequenceState.Running;
        _received = 0;
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

    public void Pause()
    {
        timeOnFloat = Time.timeScale;
        Time.timeScale = 0;
        menuLevel.SetActive(true);
        nextLevelButton.SetActive(false);
        blurPlane.SetActive(true);
    }

    public void UnPause()
    {
        Time.timeScale = timeOnFloat;
        nextLevelButton.SetActive(true);
        continueButton.SetActive(true);
        menuLevel.SetActive(false);
        blurPlane.SetActive(false);
    }

    public void Retry()
    {
        Stop();
        foreach (GameObject magnetCreated in magnetsCreated)
        {
            Destroy(magnetCreated);
        }
        magnetsCreated = new List<GameObject>();
        UnPause();
    }

    public void AddMagnet(GameObject magnet)
    {
        magnetsCreated.Add(magnet);
    }

    public void RemoveMagnet(GameObject magnet)
    {
        magnetsCreated.Remove(magnet);
        Destroy(magnet);
    }

    private void Win()
    {
        Stop();
        timeOnFloat = Time.timeScale;
        Time.timeScale = 0;
        menuLevel.SetActive(true);
        continueButton.SetActive(false);
        blurPlane.SetActive(true);
    }
}
