using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class goomba : pausableObject
{
    private SpriteRenderer _spriteRenderer;
    private Transform tr;
    private Vector3 startPos;
    private Vector3 pos;
    private float width = 0.0f;
    private bool activated = false;

    [Header("Movement")]
    public float moveSpeed = 2f; // ���������� �������� ��������

    [Header("Animation")]
    public FrameAnimator walkAnimator = new FrameAnimator
    {
        spriteFolderName = "goomba", // ��� ������ � Resources/goomba
        framesPerSecond = 8f,
        loop = true,
        animationFrames = new String[] { "walk_0", "walk_1" } // ������ ��� ����� ��� ������
    };


    private FrameAnimator currentAnimator;
    private Rigidbody2D rb;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        // �������������� ���������
        walkAnimator.Initialize(_spriteRenderer);

        // ������������� ������� ��������
        currentAnimator = walkAnimator;
        pos = tr.position;
        startPos = pos;
        width = GetComponent<CircleCollider2D>().radius * 2;

        gameManager.GetInstance().GetScene().AddPausableObject(this);
        // ��������� Rigidbody2D ��� ���������� ��������
        //rb.gravityScale = 0f; // ��������� ���������� ���� �� �����
        rb.drag = 0f; // ������� �������������
        rb.angularDrag = 0f; // ������� ������� �������������
    }

    void Update()
    {
        if (isPaused)
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false;
            return;
        }

        // ��������� ������� �������
        pos = tr.position;

        float cameraHeight = 2f * Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;
        float cameraLeftEdge = Camera.main.transform.position.x - cameraWidth / 2;
        float cameraRightEdge = Camera.main.transform.position.x + cameraWidth / 2;

        // ���������� ������ ���� �� ������ � ���� ������ ������ ������
        if (pos.x - width < cameraRightEdge && !activated)
        {
            Walk();
        }

        // ������������ ������ ���� �� ��������� ����� �� ����� ������� ������
        if (activated && pos.x + width < cameraLeftEdge)
        {
            activated = false;
            rb.velocity = Vector2.zero;
            currentAnimator?.Stop();
        }

        if (currentAnimator != null)
        {
            currentAnimator.UpdateAnimation(Time.deltaTime);
        }

        rb.simulated = activated;

        if (activated)
        {
            // ������������ ���������� ��������
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }
    }

    public void Walk()
    {
        if (!activated)
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            SwitchToAnimation(walkAnimator);
            activated = true;
        }
    }

    private void SwitchToAnimation(FrameAnimator newAnimator)
    {
        if (currentAnimator != null)
            currentAnimator.Stop();

        currentAnimator = newAnimator;
        currentAnimator.Restart();
    }

    public override void Restart()
    {
        base.Restart();
        tr.position = startPos;
        pos = startPos;
        activated = false;
        rb.velocity = Vector2.zero;
    }
}