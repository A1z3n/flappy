using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class pipe : MonoBehaviour
{
    private Vector3 pos;
    private bool isPaused = true;
    public float dur = 1.0f;
    public int animation_type = 0;
    public float range = 0.5f;
    public float startTime = 0.0f;
    private float timer = 0.0f;
    private Vector3 startPos;
    private bool reverse = false;
    void Start() {
        pos = transform.position;
        startPos = pos;
        gameManager.GetInstance().AddPipe(this);
    }
    
    
    void Update() {
        if (isPaused) return;
        timer += Time.deltaTime;
        if (timer < startTime) {
            return;
        }
        switch (animation_type) {
                
            // 1 - vertical move
            // 2 - horizontal move
            case 1:
                if (!reverse) {
                    pos.y += Time.deltaTime / dur;
                    if (pos.y > startPos.y + range) {
                        reverse = true;
                    }
                }
                else {
                    pos.y -= Time.deltaTime / dur;
                    if (pos.y < startPos.y - range)
                    {
                        reverse = false;
                    }
                }

                break;
            case 2:
                if (!reverse)
                {
                    pos.x = startPos.x + Time.deltaTime / dur;
                    if (pos.x > startPos.x + range)
                    {
                        reverse = true;
                    }
                }
                else
                {
                    pos.x -= Time.deltaTime / dur;
                    if (pos.x < startPos.x - range)
                    {
                        reverse = false;
                    }
                }
                break;
            default:
                break;
        }

        transform.position = pos;

    }

  

    public void Pause()
    {
        isPaused = true;
    }

    public void Resume()
    {
        isPaused = false;
    }

    public void Restart() {
        pos = startPos;
        isPaused = true;
    }

}
