using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    [System.Serializable]
    public class FrameAnimator
    {
        [Header("Animation Settings")]
        public string spriteFolderName; // Имя атласа в Resources
        public float framesPerSecond = 10f;
        public bool loop = true;
        public bool playOnStart = true;

        [Header("Frame Sequences")]
        public String[] animationFrames; // Последовательность кадров для проигрывания

        // Приватные переменные
        private Sprite[] sprites;
        private SpriteRenderer spriteRenderer;
        private float animationTimer = 0f;
        private int currentFrameIndex = 0;
        private bool isPlaying = false;
        private bool isInitialized = false;

        // События
        public System.Action OnAnimationComplete;
        public System.Action<int> OnFrameChanged;

        /// <summary>
        /// Инициализация аниматора
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
        /// Загрузка спрайтов из готового атласа в Resources
        /// </summary>
        private void LoadSprites()
        {
            if (string.IsNullOrEmpty(spriteFolderName))
            {
                Debug.LogWarning("FrameAnimator: spriteFolderName не задан!");
                return;
            }

            // Загружаем все спрайты из атласа (как в portal.cs)
            sprites = Resources.LoadAll<Sprite>(spriteFolderName);

            if (sprites == null || sprites.Length == 0)
            {
                Debug.LogWarning($"FrameAnimator: Спрайты не найдены в Resources/{spriteFolderName}");
                return;
            }

            // Если последовательность кадров не задана, используем все спрайты по порядку
            if (animationFrames == null || animationFrames.Length == 0)
            {
                Debug.LogWarning($"FrameAnimator: Спрайты не указаны");
            }

            Debug.Log($"FrameAnimator: Загружено {sprites.Length} спрайтов из атласа {spriteFolderName}");

            // Выводим имена всех загруженных спрайтов для отладки
            for (int i = 0; i < sprites.Length; i++)
            {
                Debug.Log($"Sprite {i}: {sprites[i].name}");
            }
        }

        /// <summary>
        /// Поиск спрайта по имени
        /// </summary>
        private Sprite FindSpriteByName(string spriteName)
        {
            if (sprites == null || string.IsNullOrEmpty(spriteName))
                return null;

            return sprites.FirstOrDefault(sprite => sprite.name == spriteName);
        }

        /// <summary>
        /// Обновление анимации (вызывать в Update)
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
        /// Переход к следующему кадру
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
        /// Установка конкретного кадра
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
                Debug.LogWarning($"FrameAnimator: Спрайт с именем '{spriteName}' не найден!");
            }
        }

        /// <summary>
        /// Запуск анимации
        /// </summary>
        public void Play()
        {
            isPlaying = true;
            animationTimer = 0f;
        }

        /// <summary>
        /// Остановка анимации
        /// </summary>
        public void Stop()
        {
            isPlaying = false;
            animationTimer = 0f;
        }

        /// <summary>
        /// Пауза анимации
        /// </summary>
        public void Pause()
        {
            isPlaying = false;
        }

        /// <summary>
        /// Перезапуск анимации с начала
        /// </summary>
        public void Restart()
        {
            currentFrameIndex = 0;
            animationTimer = 0f;
            SetFrame(0);
            Play();
        }

        /// <summary>
        /// Установка новой последовательности кадров
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
        /// Установка скорости анимации
        /// </summary>
        public void SetSpeed(float fps)
        {
            framesPerSecond = fps;
        }

        /// <summary>
        /// Смена атласа (перезагрузка спрайтов)
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

        // Свойства для получения состояния
        public bool IsPlaying => isPlaying;
        public bool IsInitialized => isInitialized;
        public int CurrentFrame => currentFrameIndex;
        public int TotalFrames => animationFrames?.Length ?? 0;
        public int TotalSprites => sprites?.Length ?? 0;
    }
}