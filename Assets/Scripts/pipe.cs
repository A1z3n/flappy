using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class pipe : MonoBehaviour
{
    private bool move = false;
    private float speed = 0.0f;
    private Vector3 pos;
    private float destx = 0.0f;
    private bool isPaused = false;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (move && !isPaused)
        {
            pos.x -= speed * Time.deltaTime;
            if (pos.x < destx)
            {
                move = false;
            }
            GetComponent<Transform>().position = pos;
        }
    }

    public bool isMoving()
    {
        return move;
    }

    public void Move(float startX, float startY, float destX, float pSpeed)
    {
        speed = pSpeed;
        move = true;
        pos.x = startX;
        pos.y = startY;
        pos.z = 1.0f;
        destx = destX;
    }

    public void Reset()
    {
        pos.x = -100;
        pos.y = -100;
        pos.z = -100;
        GetComponent<Transform>().position = pos;
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void Resume()
    {
        isPaused = false;
    }

}
