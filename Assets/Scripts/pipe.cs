using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Assets.Scripts;
using UnityEngine;

public class pipe : pausableObject
{
    private Vector3 pos;
    private bool isPaused = true;
    public float duration = 1.0f;
    public int animation_type = 0;
    public float range = 0.5f;
    public float startTime = 0.0f;
    private float timer = 0.0f;
    private Vector3 startPos;
    public bool reverse = false;
    private bool startReverse = false;
    public int moveType = 0;
    public float duration2 = 0.5f;
    void Start() {
        pos = transform.position;
        startPos = pos;
        startReverse = reverse;
        gameManager.GetInstance().AddPipe(this);
        gameManager.GetInstance().GetScene().AddPausableObject(this);
    }
    
    
    void Update() {
        if (isPaused) return;
        timer += Time.deltaTime;
        if (timer < startTime) {
            return;
        }

        float dur = duration;
        if (moveType == 1 && reverse) {
            dur = duration2;
        }
        else if (moveType == 2 && !reverse) {
            dur = duration2;
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
                    pos.x += Time.deltaTime / dur;
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


    public override void Restart() {
        pos = startPos;
        isPaused = true;
        timer = 0.0f;
        reverse = startReverse;
        transform.position = pos;
    }

}
