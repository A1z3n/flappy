using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Assets.Scripts;
using UnityEngine;

public class pipe : pausableObject
{
    private Vector3 pos;
    public float range = 0.5f;
    public float startTime = 0.0f;
    private float timer = 0.0f;
    private Vector3 startPos;
    public bool reverse = false;
    private bool startReverse = false;
    [Header("Type 1 - vertical, 2 - horizontal")]
    public int animation_type = 0;
    [Header("Move 1 - Up, 2 - Down")]
    public int moveType = 0;
    [Header("Up")]
    public float duration = 1.0f;
    [Header("Down")]
    public float duration2 = 0.5f;
    public bool once = false;
    private int start_animation_type = 0;
    private bool activated = false;
    private float width = 0.0f;
    private double angle = 0.0;
    private float sinA = 0.0f;
    private float cosA = 1.0f;
    void Start() {
        pos = transform.position;
        startPos = pos;
        startReverse = reverse;
        start_animation_type = animation_type;
        gameManager.GetInstance().AddPipe(this);
        gameManager.GetInstance().GetScene().AddPausableObject(this);
        width = GetComponentInChildren<SpriteRenderer>().size.x;
        angle = transform.rotation.z*Math.PI/180.0;
        sinA = (float)Math.Sin(angle); 
        cosA = (float)Math.Cos(angle);
    }
    
    
    void Update() {
        if (isPaused) return;
        float cameraHeight = 2f * Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;
        float cameraLeftEdge = Camera.main.transform.position.x - cameraWidth / 2;
        float cameraRightEdge = Camera.main.transform.position.x + cameraWidth / 2;

        // Активируем объект если он входит в поле зрения камеры справа
        if (pos.x - width < cameraRightEdge && !activated)
        {
            activated = true;
        }

        // Деактивируем объект если он полностью вышел за левую границу камеры
        if (activated && pos.x + width < cameraLeftEdge)
        {
            activated = false;
        }
        if(activated){
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

            switch (animation_type)
            {

                // 1 - vertical move
                // 2 - horizontal move
                case 1:
                    if (!reverse)
                    {
                        float p = Time.deltaTime / dur;
                        pos.x += sinA * p;
                        pos.y += cosA * p;
                        if ( (pos - startPos).magnitude > range)
                        {
                            if (once)
                            {
                                animation_type = 0;
                            }

                            reverse = true;
                        }
                    }
                    else
                    {

                        float p = Time.deltaTime / dur;
                        pos.x -= sinA * p;
                        pos.y -= cosA * p;

                        if ((pos - startPos).magnitude > range)
                        {
                            if (once)
                            {
                                animation_type = 0;
                            }

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
                            if (once)
                            {
                                animation_type = 0;
                            }

                            reverse = true;
                        }
                    }
                    else
                    {
                        pos.x -= Time.deltaTime / dur;
                        if (pos.x < startPos.x - range)
                        {
                            if (once)
                            {
                                animation_type = 0;
                            }

                            reverse = false;
                        }
                    }

                    break;
                default:
                    break;
            }
        }

        transform.position = pos;

    }


    public override void Restart() {
        pos = startPos;
        isPaused = true;
        timer = 0.0f;
        reverse = startReverse;
        transform.position = pos;
        animation_type = start_animation_type;
    }

}
