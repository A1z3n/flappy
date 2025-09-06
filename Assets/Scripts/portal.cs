using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Assets.Scripts
{
    public class portal : pausableObject
    {
        private SpriteRenderer portalRenderer;
        private SpriteRenderer fxRenderer;
        private float timer = 0.0f;
        private int state = 0;
        public float startupTimer = 1.0f;
        private AudioSource audioSource;
        private Sprite[] portalSprites;
        private Sprite[] fxSprites;
        public float animationSpeed = 0.2f;
        public float fxAnimationSpeed = 0.1f;
        private float animationTimer = 0.0f;
        private int currentFrame = 0;
        private int fxFrame = 0;
        public AudioClip sound;
        void Start()
        {
            portalRenderer = GetComponent<SpriteRenderer>();
            fxRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            LoadPortalSprites();
            gameManager.GetInstance().GetScene().AddPausableObject(this);
            audioSource = GetComponent<AudioSource>();
        }

        private void LoadPortalSprites()
        {
            portalSprites = Resources.LoadAll<Sprite>("portal");
            fxSprites = Resources.LoadAll<Sprite>("portalfx");

            if (portalSprites is { Length: > 0 })
            {
                portalRenderer.sprite = portalSprites[0];
                currentFrame = portalSprites.Length - 1;
                portalRenderer.enabled = false;
            }
            if (fxSprites is { Length: > 0 })
            {
                fxRenderer.sprite = fxSprites[0];
                fxFrame = 0;
                fxRenderer.enabled = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            
            if (isPaused) return;
            
            timer += Time.deltaTime;
            
            
            switch (state)
            {
                case 0:
                    if (timer > startupTimer)
                    {
                        state++;
                        timer = 0.0f;
                        portalRenderer.enabled = true;
                        fxRenderer.enabled = true;
                        audioSource.PlayOneShot(sound);
                    }
                    break;
            
                case 1:
                    animationTimer += Time.deltaTime;

                    if (animationTimer >= animationSpeed)
                    {
                        animationTimer -= animationSpeed;
                        currentFrame--;
                        if (currentFrame == 0)
                        {
                            state++;
                            timer = 0;
                            animationTimer = 0;
                            fxRenderer.enabled = true;
                        }
                        portalRenderer.sprite = portalSprites[currentFrame];
                    }
                    break;
                case 2:
                    animationTimer += Time.deltaTime;
                    if (animationTimer >= fxAnimationSpeed)
                    {
                        animationTimer -= fxAnimationSpeed;
                        fxFrame++;
                        if (fxFrame > fxSprites.Length-1)
                        {
                            fxFrame = 0;
                        }
                        fxRenderer.sprite = fxSprites[fxFrame];
                    }
                    if (timer > 2)
                    {
                        timer = 0;
                        animationTimer = 0;
                        state++;
                    }
                    break;
                case 3:
                    animationTimer += Time.deltaTime;

                    if (animationTimer >= animationSpeed)
                    {
                        animationTimer -= animationSpeed;
                        currentFrame++;
                        if (currentFrame > portalSprites.Length-1)
                        {
                            currentFrame = portalSprites.Length-1;
                            state++;
                            timer = 0;
                            animationTimer = 0;
                            portalRenderer.enabled = false;
                            fxRenderer.enabled = false;
                            Pause();
                        }
                        portalRenderer.sprite = portalSprites[currentFrame];
                    }
                    break;
            }
            // Обновляем анимацию
        }

        public override void Restart() 
        {
            state = 0;
            timer = 0.0f;
            animationTimer = 0.0f;
            currentFrame = portalSprites.Length - 1;
            fxFrame = fxSprites.Length - 1;
            portalRenderer.sprite = portalSprites[currentFrame];
            fxRenderer.sprite = fxSprites[fxFrame];
            portalRenderer.enabled = false;
            fxRenderer.enabled = false;
        }

    }
}
