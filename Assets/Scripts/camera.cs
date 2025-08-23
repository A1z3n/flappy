using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    private float cameraPos = -10;
    private float shift = 6.0f;
    private Vector3 startPos;
    private bool anim = false;
    private float destPos = 0;
    private float beginPos = 0;
    private float dur = 0;
    private float timer = 0.0f;
    private float posFix = 0.0f; 
    void Start()
    {
        player = GameObject.Find("player");
        Vector3 pos = transform.position;
        CalculateShift();
        //shift = pos.x - player.transform.position.x;
        cameraPos = posFix;
        startPos = player.transform.position;

    }

    // Update is called once per frame
    void Update() {
        if (!anim)
        {
            cameraPos = player.transform.position.x - startPos.x  + posFix;
        }
        else
        {
            timer += Time.deltaTime;
            float p = timer / dur;
            if(p<1.0f)
                cameraPos = beginPos + (destPos-beginPos)*p;
            else
            {
                cameraPos = destPos;
            }
        }
        transform.position = new Vector3(cameraPos, transform.position.y, transform.position.z);

    }

    private void CalculateShift()
    {
        Camera cam = GetComponent<Camera>();
        if (cam == null) cam = Camera.main;

        if (cam != null && cam.orthographic)
        {
            // Для ортографической камеры
            float cameraHeight = 2f * cam.orthographicSize;
            float cameraWidth = cameraHeight * cam.aspect;

            // Shift равен половине ширины экрана (центрирование по левому краю)
            posFix = cameraWidth / 2f;
        }
        else if (cam != null)
        {
            // Для перспективной камеры
            Vector3 leftEdge = cam.ViewportToWorldPoint(new Vector3(0, 0.5f, cam.nearClipPlane));
            Vector3 center = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, cam.nearClipPlane));

            posFix = center.x - leftEdge.x;
        }
    }
    public void MoveAnim(float dx,float time)
    {
        beginPos = cameraPos;
        destPos = cameraPos + dx;
        dur = time;
        anim = true;
        timer = 0;
    }

    public void CancelAnimations()
    {
        anim = false;
    }


}
