using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CotcSdk;

public enum SequenceState
{
    Running,
    Stopped
}

public class GameManager : MonoBehaviour
{

    private Cloud Cloud;
    [SerializeField]
    private Text leaderboardContent;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _playSound;
    [SerializeField] private AudioClip _winSound;

    private List<GameObject> _bullets;
    private int _received;

    [SerializeField] [Range(-90, 90)] private List<float> _bulletsShootingAngle;
    private List<float>.Enumerator _currentAngle;

    private SequenceState _sequenceState;

    private List<GameObject> magnetsCreated;
    public int penalitiesMagnets;

    [SerializeField] private BulletLauncherController _bulletLauncherController;
    [SerializeField] private GameObject menuLevel;
    [SerializeField] private GameObject blurPlane;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject nextLevelButton;
    [SerializeField] private GameObject quadRunning;
    [SerializeField] float timeOnFloat;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        // Link with the CotC Game Object
		var cb = FindObjectOfType<CotcGameObject>();
		if (cb == null) {
			Debug.LogError("Please put a Clan of the Cloud prefab in your scene!");
			return;
		}
		// Log unhandled exceptions (.Done block without .Catch -- not called if there is any .Then)
		Promise.UnhandledException += (object sender, ExceptionEventArgs e) => {
			Debug.LogError("Unhandled exception: " + e.Exception.ToString());
		};
		// Initiate getting the main Cloud object
		cb.GetCloud().Done(cloud => {
			Cloud = cloud;
			// Retry failed HTTP requests once
			Cloud.HttpRequestFailedHandler = (HttpRequestFailedEventArgs e) => {
				if (e.UserData == null) {
					e.UserData = new object();
					e.RetryIn(1000);
				}
				else
					e.Abort();
			};
			Debug.Log("Setup done");
		});
    }

    // Use this for initialization
    void Start()
    {
        _received = 0;
        _bullets = new List<GameObject>();
        _currentAngle = _bulletsShootingAngle.GetEnumerator();
        _sequenceState = SequenceState.Stopped;
        magnetsCreated = new List<GameObject>();
        quadRunning = GameObject.Find("QuadRunning");
        GameObject.Find("Play").GetComponent<Button>().interactable = true;
        GameObject.Find("Stop").GetComponent<Button>().interactable = false;
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }

        penalitiesMagnets = 0;
    }

    void Update()
    {
        string penalitiesMessage = "Penalities : " + penalitiesMagnets;
        GameObject.Find("Penalities").GetComponent<Text>().text = penalitiesMessage;
    }

    void FixedUpdate()
    {
        if (_sequenceState == SequenceState.Running)
        {
            if (quadRunning.activeSelf == true)
            {
                quadRunning.SetActive(false);
            }

            GameObject.Find("Play").GetComponent<Button>().interactable = false;
            GameObject.Find("Stop").GetComponent<Button>().interactable = true;

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
        else
        {
            if (quadRunning.activeSelf == false)
            {
                quadRunning.SetActive(true);
                GameObject.Find("Play").GetComponent<Button>().interactable = true;
                GameObject.Find("Stop").GetComponent<Button>().interactable = false;
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
        _audioSource.clip = _playSound;
        _audioSource.Play();
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
        penalitiesMagnets = 0;
        UnPause();
    }

    public void AddMagnet(GameObject magnet)
    {
        magnetsCreated.Add(magnet);
        penalitiesMagnets += 50;
    }

    public void RemoveMagnet(GameObject magnet)
    {
        magnetsCreated.Remove(magnet);
        Destroy(magnet);
        penalitiesMagnets -= 50;
    }

    private void Win()
    {
        Stop();
        _audioSource.clip = _winSound;
        _audioSource.Play();
        timeOnFloat = Time.timeScale;
        Time.timeScale = 0;
        menuLevel.SetActive(true);
        continueButton.SetActive(false);
        blurPlane.SetActive(true);
        GameState.GetInstance().Gamer.Scores.Domain("private")
            .Post(penalitiesMagnets, GetComponent<LevelManager>().GetCurrentLevel(), ScoreOrder.LowToHigh)
            .Done(postScoreRes => {
                Debug.Log("Post score: " + postScoreRes.ToString());
                GameState.GetInstance().Gamer.Scores.Domain("private")
                    .List(GetComponent<LevelManager>().GetCurrentLevel(), 10, -1)
                    .Done(centeredScoresRes => {
                        //leaderboardContent.text = "";
                        foreach(var score in centeredScoresRes) {
                            leaderboardContent.text += score.Rank + "\t" + score.GamerInfo["profile"]["displayName"] + "\t" + score.Value + "\n";
                            Debug.Log(score.Rank + ". " + score.GamerInfo["profile"]["displayName"] + ": " + score.Value);
                        }
                    }, ex => {
                        // The exception should always be CotcException
                        CotcException error = (CotcException)ex;
                        Debug.LogError("Could not get centered scores: " + error.ErrorCode + " (" + error.ErrorInformation + ")");
                    });
            }, ex => {
                // The exception should always be CotcException
                CotcException error = (CotcException)ex;
                Debug.LogError("Could not post score: " + error.ErrorCode + " (" + error.ErrorInformation + ")");
            });
        
    }

    public SequenceState sequenceState
    {
        get
        {
            return _sequenceState;
        }
        set
        {
            _sequenceState = value;
        }
    }
}
