using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class gui : MonoBehaviour
{
    private GameObject title;
    private GameObject ready;
    private GameObject hint;
    private GameObject gameover;
    private GameObject retry;
    private GameObject score;
    private GameObject time;
    private GameObject win;
    private GameObject next;
    private GameObject finalScore;
    private GameObject fps;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI timerText;
    private TextMeshProUGUI finalScoreText;
    private TextMeshProUGUI fpsText;
    private float timer = 0.0f;
    private int timerInt = 0;
    private int state = 0;

    // FPS calculation variables
    private float fpsUpdateInterval = 0.5f; // Update FPS every 0.5 seconds
    private float fpsLastInterval = 0;
    private int fpsFrames = 0;

    void Start()
    {
        Debug.Log("loading title");
        title = GameObject.Find("GUI/title");
        Debug.Log("loading ready");
        ready = GameObject.Find("GUI/ready");
        Debug.Log("loading hint");
        hint = GameObject.Find("GUI/hint");
        Debug.Log("loading replay");
        retry = GameObject.Find("GUI/replay");
        retry.GetComponent<Button>().onClick.AddListener(Restart);
        Debug.Log("loading gameover");
        gameover = GameObject.Find("GUI/gameover");
        Debug.Log("loading score");
        score = GameObject.Find("GUI/score");
        Debug.Log("loading timer");
        time = GameObject.Find("GUI/timer");
        Debug.Log("loading win");
        win = GameObject.Find("GUI/win");
        Debug.Log("loading next");
        next = GameObject.Find("GUI/next");
        Debug.Log("loading finalScore");
        //finalScore = GameObject.Find("GUI/finalScore");
        scoreText = score.GetComponent<TextMeshProUGUI>();
        fps = GameObject.Find("GUI/fps");
        if(fps)
            fpsText = fps.GetComponent<TextMeshProUGUI>();
        if(time)
            timerText = time.GetComponent<TextMeshProUGUI>();
        //finalScoreText = finalScore.GetComponent<TextMeshProUGUI>();
        score.SetActive(false);
        ready.SetActive(false);
        hint.SetActive(false);
        retry.SetActive(false);
        gameover.SetActive(false);
        win.SetActive(false);
        //next.SetActive(false);
        //finalScore.SetActive(false);
        next.GetComponent<Button>().onClick.AddListener(NextLevel);
        gameManager.GetInstance().SetGui(this);

        // Initialize FPS calculation
        fpsLastInterval = Time.realtimeSinceStartup;
        fpsFrames = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Update FPS calculation
        //UpdateFPS();

        switch (state)
        {
            case 1:
                //main menu
                break;
            case 2:
                //hint
                break;
            case 3:
                //game
                {
                    timer += Time.deltaTime;
                    int t = (int)timer;
                    if (timerInt != t)
                    {
                        timerInt = t;
                        SetTimer(t);
                    }
                }
                break;
            case 4:
                timer += Time.deltaTime;
                if (timer > 1.0f)
                {
                    retry.SetActive(true);
                }
                //gameover
                break;
        }
    }

    private void UpdateFPS()
    {
        fpsFrames++;
        float timeNow = Time.realtimeSinceStartup;

        if (timeNow > fpsLastInterval + fpsUpdateInterval)
        {
            // Calculate FPS
            float currentFps = fpsFrames / (timeNow - fpsLastInterval);
            
            // Update FPS text
            fpsText.SetText($"FPS: {currentFps:F0}");

            // Reset counters
            fpsFrames = 0;
            fpsLastInterval = timeNow;
        }
    }

    public void StartGameAnim()
    {
        
        hint.SetActive(true);
        ready.SetActive(true);
        title.SetActive(false);
        retry.SetActive(false);
        gameover.SetActive(false);
        score.SetActive(true);
        win.SetActive(false);
        next.SetActive(false);
        //finalScore.SetActive(false);
        state = 2;
        timer = 0;

    }

    public void StartMMAnim()
    {
        ready.SetActive(false);
        hint.SetActive(false);
        title.SetActive(true);
        retry.SetActive(false);
        gameover.SetActive(false);
        win.SetActive(false);
        next.SetActive(false);
        //finalScore.SetActive(false);
        state = 1;
        timer = 0;
    }

    public void StartGame()
    {
        ready.SetActive(false);
        hint.SetActive(false);
        title.SetActive(false);
        retry.SetActive(false);
        gameover.SetActive(false);
        win.SetActive(false);
        next.SetActive(false);
        //finalScore.SetActive(false);
        state = 3;
        timer = 0.0f;
        timerInt = 0;
        SetTimer(0);
    }

    public void GameOver()
    {
        state = 4;
        timer = 0;
        //finalScore.SetActive(true);

        //string text = "score: " + gameManager.GetInstance().GetScore();
        //finalScoreText.SetText(text);
        gameover.SetActive(true);
    }

    public void Win() {
        win.SetActive(true);
        next.SetActive(true);
        SetScore(gameManager.GetInstance().GetScene().GetTotalCount());
    }
    public void Restart()
    {
        gameManager.GetInstance().Restart();
    }

    public void NextLevel() {
        gameManager.GetInstance().NextLevel();
    }

    public void SetScore(int s)
    {
        string text = "" + s;
        scoreText.SetText(text);
    }
    public void SetTimer(int s)
    {
        if (timerText)
        {
            string text = "" + s;
            timerText.SetText(text);
        }
    }

}
