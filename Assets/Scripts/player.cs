using System;
using Assets.Scripts;
using Unity.Mathematics;
using UnityEngine;

public class player : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 pos;
    private float ang;
    // Кешированные компоненты
    private Transform tr;
    //private Animator anim;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    public float velocity = 0.0f;
    public float rotate_coeff = 5.0f;
    public float jump_speed = 4.0f;
    private float jump_timer = 0.0f;
    public bool jump_anim = false;
    private Vector3 startPos;
    private bool jump_state = false;
    public float max_velocity = 1.0f;
    //private Vector3 position;
    public float move_speed = 1.0f;
    private float borderTop = 5.0f;
    private float borderBottom = -3.8f;
    public AudioClip wingSound;
    public AudioClip dieSound;
    public AudioClip hitSound;
    public AudioClip pointSound;
    public AudioClip swooshingSound;
    public float smoothTime = 0.5f;
    private bool disappear = false;
    private bool appear = false;
    private bool appeared = true;
    private Vector3 portalPosition;
    private Sprite[] animSprites;
    public String animationName = "flappy";
    public int dieAnimation = 1;
    public int idleAnimation = 1;
    public int[] flyAnimation = {0,1,2,0};
    public bool immortal_cheat = false;

    public enum ePlayerState {
        kPause,
        kPlay,
        kDead,
        kReady,
        kAnimation
    }

    private enum eAnimState {
        kIdle,
        kFly,
        kDie
    }

    private  ePlayerState state =  ePlayerState.kPause;
    private eAnimState animState = eAnimState.kIdle;

    void Start()
    {
        tr = GetComponent<Transform>();
        //anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        pos = tr.position;
        startPos = tr.position;
        gameManager.GetInstance().SetPlayer(this);
        rb.velocity = Vector2.zero;

        animSprites = Resources.LoadAll<Sprite>(animationName);

        /*if (gameManager.GetInstance().GetCurrentLevel() == 2)
        {
            appeared = false;
            appear = true;
        }*/
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
        if (disappear)
        {
            tr.position = Vector3.MoveTowards(tr.position, portalPosition, Time.deltaTime * 2.0f);
            tr.localScale = Vector3.MoveTowards(tr.localScale, Vector3.zero, Time.deltaTime*3.0f);
        }
        else if (appear)
        {

        }
        Animate(Time.deltaTime);
        if (state == ePlayerState.kDead || state == ePlayerState.kPause || state == ePlayerState.kReady) return;
        
        if (jump_anim)
        {
            jump_timer += Time.deltaTime;
            //velocity -= Time.deltaTime*jump_speed;
            if (jump_timer > 0.5f)
            {
                jump_anim = false;
                jump_timer = 0.0f;
                animState = eAnimState.kIdle;
                //anim.SetInteger("state", 0);
            }
        }

        ang = rb.velocity.y / rotate_coeff;
        velocity = rb.velocity.y;
        
        if (ang > 0.25f * Mathf.PI)
            ang = 0.25f * Mathf.PI;
        else if (ang < -0.5f * Mathf.PI)
            ang = -0.5f * Mathf.PI;

        tr.rotation = Quaternion.Euler(0, 0, ang * Mathf.Rad2Deg);
        pos.x += Time.deltaTime * move_speed;
        pos.y = tr.position.y;
        tr.position = pos;
        
        if (state != ePlayerState.kDead)
        {
            if (pos.y > borderTop || pos.y < borderBottom)
                gameManager.GetInstance().Die();
        }
        
        if (jump_state)
        {
            rb.velocity = Vector2.up * jump_speed;
            jump_state = false;
        }
    }


    public void Ready() {
        SetState(ePlayerState.kPlay);
        Fly();
    }

    public void Click()
    {
        if (state == ePlayerState.kPause) return;
        Fly();
    }

    public void Fly()
    {
        jump_anim = true;
        //anim.SetInteger("state", 1);
        animState = eAnimState.kFly;
        jump_timer = 0.0f;
        jump_state = true;
        audioSource.PlayOneShot(wingSound);
    }

    public void SetState(ePlayerState s)
    {
        state = s;
        switch (s) {
            case ePlayerState.kPause:
            case ePlayerState.kDead:
            case ePlayerState.kReady:
                rb.simulated = false;
                //rb.isKinematic = true;
                break;
            case ePlayerState.kPlay:
                rb.simulated = true;
                //rb.isKinematic = false;
                break;
            case ePlayerState.kAnimation:
                rb.simulated = false;
                //rb.isKinematic = true;
                jump_anim = true;
                break;

        }
    }

    public void Die()
    {
        //anim.SetInteger("state", 2);
        animState = eAnimState.kDie;
        SetState(ePlayerState.kDead);
        audioSource.PlayOneShot(dieSound);
        audioSource.PlayOneShot(hitSound);
    }

    public void Restart()
    {
        pos = startPos;
        tr.position = startPos;
        ang = 0.0f;
        tr.rotation = Quaternion.identity;
        jump_state = false;
        velocity = 0.0f;
        jump_anim = false;
        SetState(ePlayerState.kReady);
        spriteRenderer.color = Color.white;
        rb.velocity = Vector2.zero;
        animState = eAnimState.kIdle;
    }
        
    public void Win() 
    {
        SetState(ePlayerState.kPause);
        //if(anim)
          //  anim.SetInteger("state", 2);
        if (gameManager.GetInstance().GetScene().mEvent=="portal")
        {
            disappear = true;
            var p = GameObject.Find("portal");
            if (p)
            {
                portalPosition = p.GetComponent<Transform>().position;
                p.GetComponent<portal>().Resume();
            }
        }
    }


    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "pipe_score")
        {
            audioSource.PlayOneShot(pointSound);
            gameManager.GetInstance().AddScore();
        }
        else if (coll.gameObject.tag == "pipe_final")
        {
            gameManager.GetInstance().Win();
        }
        else if (coll.gameObject.tag == "portal_trigger") {
            gameManager.GetInstance().RunAnimation("portal");
        }
        else if (coll.gameObject.tag is "ground" or "enemy")
        {
            if(!immortal_cheat)
                gameManager.GetInstance().Die("");//TODO: fix
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "pipe")
        {
            if(!immortal_cheat)
                gameManager.GetInstance().Die();
        }
        else if (collision.gameObject.tag == "pipe_final")
        {
            if (!immortal_cheat)
            {
                if (gameManager.GetInstance().GetScene().mEvent == "dog")
                {
                    gameManager.GetInstance().Die("dog");
                }
                else
                {
                    gameManager.GetInstance().Die();
                }
            }
        }
        else if ( collision.gameObject.tag is "ground" or "enemy")
        {
            if(!immortal_cheat)
                gameManager.GetInstance().Die("");//TODO: fix
        }
    }

    private void Animate(float dt)
    {
        switch (animState)
        {
            case eAnimState.kIdle:                
                if (animSprites is { Length: > 0 })
                {
                    spriteRenderer.sprite = animSprites[idleAnimation];
                }
                break;
            case eAnimState.kFly:
                if (animSprites is { Length: > 0 })
                {
                    int frame = (int)((Time.time * 10) % flyAnimation.Length);
                    spriteRenderer.sprite = animSprites[flyAnimation[frame]];
                }
                break;
            case eAnimState.kDie:
                if (animSprites is { Length: > 0 })
                {
                    spriteRenderer.sprite = animSprites[dieAnimation];
                }
                break;
        }
    }
}
