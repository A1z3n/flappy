using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class player : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 pos;
    private float ang;
    private Transform tr;
    public float velocity = 0.0f;
    public float rotate_coeff = 5.0f;
    public float jump_speed = 4.0f;
    private Animator anim;
    private float jump_timer = 0.0f;
    public bool jump_anim = false;
    private float dieTimer = 2.0f;
    private bool isDead = false;
    private Vector3 startPos;
    private Rigidbody2D rb;
    private bool jump_state = false;
    public float max_velocity = 1.0f;
    //private Vector3 position;
    public float move_speed = 1.0f;
    private float borderTop = 5.0f;
    private float borderBottom = -3.8f;

    public enum ePlayerState {
        kPause,
        kPlay,
        kAnimation
    }
    private  ePlayerState state =  ePlayerState.kPause;

    void Start()
    {
        tr = GetComponent<Transform>();
        pos = tr.position;
        ang = 0;
        anim = GetComponent<Animator>();
        gameManager.GetInstance().SetPlayer(this);
        startPos = GetComponent<Transform>().position;
        pos = startPos;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (isDead)
        //{
        //    dieTimer -= Time.deltaTime;
        //    if (dieTimer < 0.0f)
        //    {
        //        GetComponent<SpriteRenderer>().color = Color.clear;
        //        GetComponent<Transform>().position = startPos;
        //        isDead = false;
        //    }
        //}

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

        if (jump_state) {

            rb.velocity += Vector2.up * jump_speed*Time.deltaTime;
            if (rb.velocity.y > max_velocity) {
                jump_state = false;
            }
        }

        ang = rb.velocity.y / rotate_coeff;
        velocity = rb.velocity.y;
        if (ang > 0.25 * Math.PI)
        {
            ang = 0.25f * (float)Math.PI;
        }
        else if (ang < -0.5 * Math.PI)
        {
            ang = -0.5f * (float)Math.PI;
        }


        GetComponent<Transform>().rotation = quaternion.RotateZ(ang);
        pos.x += Time.deltaTime * move_speed;
        pos.y = GetComponent<Transform>().position.y;
        GetComponent<Transform>().position = pos;
        if (!isDead) {
            if (pos.y > borderTop || pos.y < borderBottom) {
                gameManager.GetInstance().Die();
            }
        }
    }

    public void Click()
    {
        if (state == 0) return;
        Fly();
    }

    public void Fly()
    {
      //rb.AddForce(Vector2.up*jump_speed,ForceMode2D.Force);
        jump_anim = true;
        anim.SetInteger("state", 1);
        jump_timer = 0.0f;
        jump_state = true;
    }

    public void SetState(ePlayerState s)
    {
        state = s;
        switch (s) {
            case ePlayerState.kPause:
                rb.simulated = false;
                break;
            case ePlayerState.kPlay:
                rb.simulated = true;
                break;
            case ePlayerState.kAnimation:
                rb.simulated = false;
                jump_anim = true;
                break;

        }
    }

    public void Die()
    {
        SetState(ePlayerState.kPause);
        anim.SetInteger("state", 2);
        isDead = true;
        dieTimer = 2.0f;
    }

    public void Restart()
    {
        pos = startPos;
        GetComponent<Transform>().position = startPos;
        ang = 0.0f;
        GetComponent<Transform>().rotation = quaternion.RotateZ(ang);
        jump_state = false;
        velocity = 0.0f;
        jump_anim = false;
        isDead = false;
        GetComponent<SpriteRenderer>().color = Color.white;
        SetState(ePlayerState.kPause);
    }

    public void Win() 
    {
        SetState(ePlayerState.kPause);
        anim.SetInteger("state", 2);
    }


    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "pipe_score")
        {
            gameManager.GetInstance().AddScore();
        }
        else if (coll.gameObject.tag == "pipe_final")
        {
            gameManager.GetInstance().Win();
        }
        else if (coll.gameObject.tag == "portal_trigger") {
            gameManager.GetInstance().RunAnimation("portal");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "pipe" || collision.gameObject.tag == "ground")
        {
            gameManager.GetInstance().Die();
        }
    }
}
