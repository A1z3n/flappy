using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    [System.Serializable]
    public class FrameAnimator
    {
        [Header("Animation Settings")]
        public string spriteFolderName; // ��� ������ � Resources
        public float framesPerSecond = 10f;
        public bool loop = true;
        public bool playOnStart = true;

        [Header("Frame Sequences")]
        public String[] animationFrames; // ������������������ ������ ��� ������������

        // ��������� ����������
        private Sprite[] sprites;
        private SpriteRenderer spriteRenderer;
        private float animationTimer = 0f;
        private int currentFrameIndex = 0;
        private bool isPlaying = false;
        private bool isInitialized = false;

        // �������
        public System.Action OnAnimationComplete;
        public System.Action<int> OnFrameChanged;

        /// <summary>
        /// ������������� ���������
        /// </summary>
        public void Initialize(SpriteRenderer renderer, string atlasName = null)
        {
            spriteRenderer = renderer;

            if (!string.IsNullOrEmpty(atlasName))
                spriteFolderName = atlasName;

            LoadSprites();

            if (playOnStart)
                Play();

            isInitialized = true;
        }

        /// <summary>
        /// �������� �������� �� �������� ������ � Resources
        /// </summary>
        private void LoadSprites()
        {
            if (string.IsNullOrEmpty(spriteFolderName))
            {
                Debug.LogWarning("FrameAnimator: spriteFolderName �� �����!");
                return;
            }

            // ��������� ��� ������� �� ������ (��� � portal.cs)
            sprites = Resources.LoadAll<Sprite>(spriteFolderName);

            if (sprites == null || sprites.Length == 0)
            {
                Debug.LogWarning($"FrameAnimator: ������� �� ������� � Resources/{spriteFolderName}");
                return;
            }

            // ���� ������������������ ������ �� ������, ���������� ��� ������� �� �������
            if (animationFrames == null || animationFrames.Length == 0)
            {
                Debug.LogWarning($"FrameAnimator: ������� �� �������");
            }

            Debug.Log($"FrameAnimator: ��������� {sprites.Length} �������� �� ������ {spriteFolderName}");

            // ������� ����� ���� ����������� �������� ��� �������
            for (int i = 0; i < sprites.Length; i++)
            {
                Debug.Log($"Sprite {i}: {sprites[i].name}");
            }
        }

        /// <summary>
        /// ����� ������� �� �����
        /// </summary>
        private Sprite FindSpriteByName(string spriteName)
        {
            if (sprites == null || string.IsNullOrEmpty(spriteName))
                return null;

            return sprites.FirstOrDefault(sprite => sprite.name == spriteName);
        }

        /// <summary>
        /// ���������� �������� (�������� � Update)
        /// </summary>
        public void UpdateAnimation(float deltaTime)
        {
            if (!isInitialized || !isPlaying || sprites == null || animationFrames == null || animationFrames.Length == 0)
                return;

            animationTimer += deltaTime;

            float frameTime = 1f / framesPerSecond;

            if (animationTimer >= frameTime)
            {
                animationTimer -= frameTime;
                NextFrame();
            }
        }

        /// <summary>
        /// ������� � ���������� �����
        /// </summary>
        private void NextFrame()
        {
            currentFrameIndex++;

            if (currentFrameIndex >= animationFrames.Length)
            {
                if (loop)
                {
                    currentFrameIndex = 0;
                }
                else
                {
                    currentFrameIndex = animationFrames.Length - 1;
                    isPlaying = false;
                    OnAnimationComplete?.Invoke();
                    return;
                }
            }

            SetFrame(currentFrameIndex);
        }

        /// <summary>
        /// ��������� ����������� �����
        /// </summary>
        private void SetFrame(int frameIndex)
        {
            if (frameIndex < 0 || frameIndex >= animationFrames.Length)
                return;

            string spriteName = animationFrames[frameIndex];
            Sprite targetSprite = FindSpriteByName(spriteName);

            if (targetSprite != null)
            {
                spriteRenderer.sprite = targetSprite;
                OnFrameChanged?.Invoke(frameIndex);
            }
            else
            {
                Debug.LogWarning($"FrameAnimator: ������ � ������ '{spriteName}' �� ������!");
            }
        }

        /// <summary>
        /// ������ ��������
        /// </summary>
        public void Play()
        {
            isPlaying = true;
            animationTimer = 0f;
        }

        /// <summary>
        /// ��������� ��������
        /// </summary>
        public void Stop()
        {
            isPlaying = false;
            animationTimer = 0f;
        }

        /// <summary>
        /// ����� ��������
        /// </summary>
        public void Pause()
        {
            isPlaying = false;
        }

        /// <summary>
        /// ���������� �������� � ������
        /// </summary>
        public void Restart()
        {
            currentFrameIndex = 0;
            animationTimer = 0f;
            SetFrame(0);
            Play();
        }

        /// <summary>
        /// ��������� ����� ������������������ ������
        /// </summary>
        public void SetAnimationSequence(String[] newFrames)
        {
            animationFrames = newFrames;
            currentFrameIndex = 0;
            animationTimer = 0f;
            if (isPlaying)
                SetFrame(0);
        }

        /// <summary>
        /// ��������� �������� ��������
        /// </summary>
        public void SetSpeed(float fps)
        {
            framesPerSecond = fps;
        }

        /// <summary>
        /// ����� ������ (������������ ��������)
        /// </summary>
        public void ChangeAtlas(string newAtlasName)
        {
            spriteFolderName = newAtlasName;
            LoadSprites();
            if (isInitialized && isPlaying)
            {
                SetFrame(currentFrameIndex);
            }
        }

        // �������� ��� ��������� ���������
        public bool IsPlaying => isPlaying;
        public bool IsInitialized => isInitialized;
        public int CurrentFrame => currentFrameIndex;
        public int TotalFrames => animationFrames?.Length ?? 0;
        public int TotalSprites => sprites?.Length ?? 0;
    }
}