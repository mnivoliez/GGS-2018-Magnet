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
    private float timerMouseDown;
    private bool isMouseDown;


    void Update()
    {
        if (isMouseDown)
        {
            timerMouseDown += Time.deltaTime;
        }
    }


    void OnMouseDown()
    {
        dist = Camera.main.WorldToScreenPoint(transform.position);
        posX = Input.mousePosition.x - dist.x;
        posY = Input.mousePosition.y - dist.y;
        if (!isMouseDown) isMouseDown = true;
        timerMouseDown = 0f;
    }

    void OnMouseUp()
    {
        if (isMouseDown) isMouseDown = false;
        if (timerMouseDown < 0.2f)
        {
            NextType();
			magnetEffect.DirectionForce = this.directionForce;

        }
		timerMouseDown = 0f;
    }

    void OnMouseDrag()
    {
        Vector3 curPos = new Vector3(Input.mousePosition.x - posX, Input.mousePosition.y - posY, dist.z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
        transform.position = worldPos;
    }


	// Use this for initialization
	void Start () {
		magnetEffect = transform.Find(MagnetChild.AreaEffect.ToString()).gameObject.GetComponent<MagnetEffect>();
		magnetEffect.DirectionForce = this.directionForce;
		magnetEffect.Intensity = this.intensity;
		timerMouseDown = 0f;
        isMouseDown = false;
	}

	public void NextType() {
		directionForce = 
			directionForce == DirectionForce.Attract ? 
				DirectionForce.Repulse : DirectionForce.Attract;
	}
}
