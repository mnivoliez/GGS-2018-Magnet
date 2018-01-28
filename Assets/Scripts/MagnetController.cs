using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MagnetChild {
	AreaEffect,
	Core
}
public class MagnetController : MonoBehaviour {

	private MagnetEffect magnetEffect;
	[SerializeField] private DirectionForce directionForce;
	[SerializeField] private float intensity = 2;

	private Vector3 dist;
    private float posX;
    private float posY;
    private float posZ;
    private float timerMouseDown;
    private bool isMouseDown;
    public bool isOnTrashcan;


    // Use this for initialization
	void Start () {
		magnetEffect = transform.Find(MagnetChild.AreaEffect.ToString()).gameObject.GetComponent<MagnetEffect>();
		magnetEffect.DirectionForce = this.directionForce;
		magnetEffect.Intensity = this.intensity;
		timerMouseDown = 0f;
        isMouseDown = false;
        isOnTrashcan = false;
	}


    void Update()
    {
        if (isMouseDown)
        {
            timerMouseDown += Time.deltaTime;
        }
    }


    void OnMouseDown()
    {
        Debug.Log(GameObject.Find("GameManager").GetComponent<GameManager>().sequenceState);
        if (GameObject.Find("GameManager").GetComponent<GameManager>().sequenceState == SequenceState.Stopped)
        {
            dist = Camera.main.WorldToScreenPoint(transform.position);
            posX = Input.mousePosition.x - dist.x;
            posY = Input.mousePosition.y - dist.y;
            posZ = GameObject.Find("UI").transform.position.z;
            if (!isMouseDown) isMouseDown = true;
            timerMouseDown = 0f;
        }
        
    }

    void OnMouseUp()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().sequenceState == SequenceState.Stopped)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            if (isOnTrashcan)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().RemoveMagnet(gameObject);
            } else {
                if (isMouseDown) isMouseDown = false;
                if (timerMouseDown < 0.2f)
                {
                    NextType();
                    magnetEffect.DirectionForce = this.directionForce;
                }
                timerMouseDown = 0f;
            }
        }
    }

    void OnMouseDrag()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().sequenceState == SequenceState.Stopped)
        {
            Vector3 curPos = new Vector3(Input.mousePosition.x - posX, Input.mousePosition.y - posY, dist.z);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
            transform.position = new Vector3(worldPos.x, worldPos.y, posZ);
        }
    }


	public void NextType() {
		directionForce = 
			directionForce == DirectionForce.Attract ? 
				DirectionForce.Repulse : DirectionForce.Attract;
	}
}
