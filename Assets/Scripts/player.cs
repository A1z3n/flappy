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
    public float speed = 2.5f;
    public float g = 0.015f;
    private Transform tr;
    public float velocity = 0.0f;
    public float rotate_speed = 3.0f;
    public float jump_speed = 0.012f;
    public float velocity_max = 0.05f;
    private Animator anim;
    private bool clicked = false;
    private float jump_timer = 0.0f;
    public bool jump_anim = false;
    private int state = 0;
    public float groundDieY = -3.71f;

    void Start()
    {
        tr = GetComponent<Transform>();
        pos = tr.position;
        ang = 0;
        anim = GetComponent<Animator>();
        gameManager.GetInstance().SetPlayer(this);
    }

    // Update is called once per frame
    void Update()
    {

        if (state == 0) return;
        //pos.x += speed * Time.deltaTime;
        bool touched = false;
        bool touchup = false;
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                touched = true;
                break;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                touchup = true;
                break;
            }
        }

        if (clicked && (Input.GetButtonUp("Jump") || touchup))
        {
            clicked = false;
        }

        if (!clicked && (Input.GetButtonDown("Jump") || touched))
        {
            velocity -= jump_speed;
            if (velocity < -0.01f)
            {
                velocity = -0.01f;
            }
            jump_anim = true;
            clicked = true;
            anim.SetBool("fly", true);
            jump_timer = 0.0f;
        }

        if (jump_anim)
        {
            jump_timer += Time.deltaTime;
            if (jump_timer > 0.5f)
            {
                jump_anim = false;
                jump_timer = 0.0f;
                anim.SetBool("fly", false);
            }
        }

        velocity += Time.deltaTime*g;
        if (velocity > velocity_max)
        {
            velocity = velocity_max;
        }
        else if (velocity < -velocity_max)
        {
            velocity = -velocity_max;
        }
        pos.y -= velocity;
        if (pos.y < groundDieY)
        {
            pos.y = groundDieY;
            gameManager.GetInstance().Die();
        }
        GetComponent<Transform>().position = pos;
        GetComponent<Transform>().rotation = quaternion.RotateZ(ang);
       

    }

    public void SetState(int s)
    {
        state = s;
    }
}
