using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class hammerBro : pausableObject
{
    private SpriteRenderer _spriteRenderer;
    private Transform tr;
    private Vector3 startPos;
    private Vector3 pos;
    private float timer = 0.0f;
    private float jumpTimer = 0.0f;
    private float width = 0.0f;
    private Rigidbody2D rb;
    private bool activated = false;
    public GameObject hammerPrefab;
    public float startTime = 0.1f;
    public float reloadTime = 0.4f;
    public float jumpStartTime = 0.5f;
    public float jumpForce = 3.0f;

    [Header("Attack Animation")] public FrameAnimator attackAnimator = new FrameAnimator
    {
        spriteFolderName = "hammerBro",
        framesPerSecond = 8f,
        loop = true,
        animationFrames = new String[] { "attack_0", "attack_1" }
    };
    [Header("Idle Animation")]
    public FrameAnimator idleAnimator = new FrameAnimator
    {
        spriteFolderName = "hammerBro",
        framesPerSecond = 8f,
        loop = true,
        animationFrames = new String[] { "idle_0", "idle_1" }
    };
    private FrameAnimator currentAnimator;
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        width = GetComponent<BoxCollider2D>().size.x;
        idleAnimator.Initialize(_spriteRenderer);
        attackAnimator.Initialize(_spriteRenderer);
        attackAnimator.OnAnimationComplete = () =>
        {
            SwitchToAnimation(idleAnimator);
            timer = 0;
        };

        // Устанавливаем текущий аниматор
        currentAnimator = idleAnimator;
        pos = tr.position;
        startPos = pos; 
        rb.drag = 0f; // Убираем сопротивление
        rb.angularDrag = 0f; // Убираем угловое сопротивление
        gameManager.GetInstance().GetScene().AddPausableObject(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused)
        {
            //rb.velocity = Vector2.zero;
            rb.simulated = false;
            return;
        }
        float cameraHeight = 2f * Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;
        float cameraLeftEdge = Camera.main.transform.position.x - cameraWidth / 2;
        float cameraRightEdge = Camera.main.transform.position.x + cameraWidth / 2;
        pos = tr.position;
        // Активируем объект если он входит в поле зрения камеры справа
        if (pos.x - width < cameraRightEdge && !activated)
        {
            activated = true;
            rb.simulated = true;
        }

        // Деактивируем объект если он полностью вышел за левую границу камеры
        if (activated && pos.x + width < cameraLeftEdge)
        {
            rb.simulated = false;
            activated = false;
            //rb.velocity = Vector2.zero;
            currentAnimator?.Stop();
        }

        if (activated)
        {
            timer += Time.deltaTime;
            if (timer > startTime + reloadTime)
            {
                Attack();
            }
            jumpTimer += Time.deltaTime;
            if (jumpTimer > jumpStartTime)
            {
                if (Math.Abs(rb.velocity.y) < 0.01f) // Проверяем, что объект на земле
                {
                    rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                }
                jumpTimer = 0.0f;
            }

            if (currentAnimator != null)
            {
                currentAnimator.UpdateAnimation(Time.deltaTime);
            }
        }
    }

    public override void Restart()
    {
        pos = startPos;
        tr.position = pos;
        timer = 0;
        activated = false;
        SwitchToAnimation(idleAnimator);
    }

    private void SwitchToAnimation(FrameAnimator newAnimator)
    {
        if (currentAnimator != null)
            currentAnimator.Stop();

        currentAnimator = newAnimator;
        currentAnimator.Restart();
    }

    private void Attack()
    {
        if (activated)
        {
            timer = startTime;
            SwitchToAnimation(attackAnimator);
            //GameObject hammerPrefab = Resources.Load("hammer") as GameObject;
            if (hammerPrefab != null)
            {
                GameObject hammerObj = Instantiate(hammerPrefab);
                hammerObj.transform.position = new Vector3(pos.x - 0.5f, pos.y, pos.z);
                hammerObj.GetComponent<hammer>().Resume();
            }
            else
            {
                Debug.LogError("Hammer prefab не найден в Resources/hammer");
            }
        }
    }

}
