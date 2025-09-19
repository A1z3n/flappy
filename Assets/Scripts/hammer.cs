using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hammer : pausableObject
{
    private Transform tr;
    private Vector3 pos;
    private Vector3 startPos;
    private float timer = 0;
    public float gravity = 9.4f;
    public float startVelocity = 4.0f;
    public float moveSpeed = 1.0f;

    [Header("Animation")] public FrameAnimator attackAnimator = new FrameAnimator
    {
        spriteFolderName = "hammer",
        framesPerSecond = 8f,
        loop = true,
        animationFrames = new String[] { "hammer_0", "hammer_1", "hammer_2" }
    };

    void Start()
    {
        tr = GetComponent<Transform>();
        pos = tr.position;
        startPos = pos;
        attackAnimator.Initialize(GetComponent<SpriteRenderer>());
        gameManager.GetInstance().GetScene().AddPausableObject(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused) return;
        timer += Time.deltaTime;
        pos.x-=Time.deltaTime * moveSpeed;
        pos.y = startPos.y + startVelocity*timer - gravity*timer*timer*0.5f;
        tr.position = pos;
        attackAnimator.UpdateAnimation(Time.deltaTime);
    }

    public override void Restart()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "ground")
        {
            Destroy(gameObject);
        }
    }

}
