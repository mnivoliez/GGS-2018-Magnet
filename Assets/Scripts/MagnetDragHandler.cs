using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetDragHandler : MonoBehaviour
{

    private Vector3 dist;
    private float posX;
    private float posY;
    private float timerMouseDown;
    private bool isMouseDown;
    private MagnetEffect magnetEffect;

    void Start()
    {
        timerMouseDown = 0f;
        isMouseDown = false;
        magnetEffect = GetComponent<MagnetEffect>();
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
            //NextType();
        }
		timerMouseDown = 0f;
    }

    void OnMouseDrag()
    {
        Vector3 curPos = new Vector3(Input.mousePosition.x - posX, Input.mousePosition.y - posY, dist.z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
        transform.parent.position = worldPos;
    }

}
