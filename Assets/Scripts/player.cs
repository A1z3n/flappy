using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;

public class player : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 pos;
    private float ang;
    private float dest_ang = 0.0f;
    private float start_ang = 0.0f;
    public float speed = 2.5f;
    public float g = 0.015f;
    private Transform tr;
    public float velocity = 0.0f;
    public float rotate_speed = 3.0f;
    public float jump_speed = 0.015f;
    public float velocity_min = -0.015f;
    public float velocity_max = 0.05f;
    private Animator anim;
    private float jump_timer = 0.0f;
    public bool jump_anim = false;
    private int state = 0;
    public float groundDieY = -3.71f;
    private float dieTimer = 2.0f;
    private bool isDead = false;
    private Vector3 startPos;
    void Start()
    {
        tr = GetComponent<Transform>();
        pos = tr.position;
        ang = 0;
        anim = GetComponent<Animator>();
        gameManager.GetInstance().SetPlayer(this);
        startPos = GetComponent<Transform>().position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            dieTimer -= Time.deltaTime;
            if (dieTimer < 0.0f)
            {
                GetComponent<SpriteRenderer>().color = Color.clear;
                GetComponent<Transform>().position = startPos;
                isDead = false;
            }
        }
        if (state == 0) return;
        if (jump_anim)
        {
            jump_timer += Time.deltaTime;
            //velocity -= Time.deltaTime*jump_speed;
            if (jump_timer > 0.5f)
            {
                jump_anim = false;
                jump_timer = 0.0f;
                anim.SetInteger("state", 0);
            }
        }
        velocity += Time.deltaTime * g;
        

        if (velocity > velocity_max)
        {
            velocity = velocity_max;
        }
        else if (velocity < velocity_min)
        {
            velocity = velocity_min;
        }
        pos.y -= velocity;
        ang = -velocity* rotate_speed / velocity_max;
        if (ang > 0.5 * Math.PI)
        {
            ang = 0.5f * (float)Math.PI;
        }
        else if (ang < -0.25 * Math.PI)
        {
            ang = -0.25f * (float)Math.PI;
        }
        if (pos.y < groundDieY)
        {
            pos.y = groundDieY;
            gameManager.GetInstance().Die();
        }
        GetComponent<Transform>().position = pos;
        
        GetComponent<Transform>().rotation = quaternion.RotateZ(ang);
       

    }

    public void Click()
    {
        if (state == 0) return;
        Fly();
    }
    public void Fly()
    {
        velocity -= jump_speed;
        jump_anim = true;
        //clicked = true;
        anim.SetInteger("state", 1);
        jump_timer = 0.0f;
    }
    public void SetState(int s)
    {
        state = s;
    }

    public void Die()
    {
        state = 0;
        anim.SetInteger("state", 2);
        isDead = true;
        dieTimer = 2.0f;
    }

    public void Restart()
    {
        pos = startPos;
        ang = 0.0f;
        velocity = 0.0f;
        jump_anim = false;
        isDead = false;
        GetComponent<SpriteRenderer>().color = Color.white;
        state = 0;
    }
}
