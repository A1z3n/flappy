using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    private float cameraPos = -10;
    public float shift = 6.0f;
    private Vector3 startPos;
    private bool anim = false;
    private float destPos = 0;
    private float beginPos = 0;
    private float dur = 0;
    private float timer = 0.0f;
    void Start()
    {
        player = GameObject.Find("player");
        Vector3 pos = transform.position;
        shift = pos.x-player.transform.position.x;
        cameraPos = player.transform.position.x + shift;
        startPos = pos;
    }

    // Update is called once per frame
    void Update() {
        if (!anim)
        {
            cameraPos = player.transform.position.x + shift;
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
