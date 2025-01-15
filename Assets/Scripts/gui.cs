using System.Collections;
using System.Collections.Generic;
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
    private GameObject goal;
    private GameObject win;
    private GameObject next;
    private GameObject finalScore;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI timerText;
    private TextMeshProUGUI finalScoreText;
    private float timer = 0.0f;
    private int timerInt = 0;
    private int state = 0;
    void Start()
    {
        title = GameObject.Find("GUI/title");
        ready = GameObject.Find("GUI/ready");
        hint = GameObject.Find("GUI/hint");
        retry = GameObject.Find("GUI/replay");
        retry.GetComponent<Button>().onClick.AddListener(Restart);
        gameover = GameObject.Find("GUI/gameover");
        score = GameObject.Find("GUI/score");
        time = GameObject.Find("GUI/timer");
        goal = GameObject.Find("GUI/goal");
        win = GameObject.Find("GUI/win");
        next = GameObject.Find("GUI/next");
        finalScore = GameObject.Find("GUI/finalScore");
        scoreText = score.GetComponent<TextMeshProUGUI>();
        timerText = time.GetComponent<TextMeshProUGUI>();
        finalScoreText = finalScore.GetComponent<TextMeshProUGUI>();
        score.SetActive(false);
        ready.SetActive(false);
        hint.SetActive(false);
        retry.SetActive(false);
        gameover.SetActive(false);
        goal.SetActive(false);
        win.SetActive(false);
        next.SetActive(false);
        finalScore.SetActive(false);
        next.GetComponent<Button>().onClick.AddListener(NextLevel);
        gameManager.GetInstance().SetGui(this);
    }

    // Update is called once per frame
    void Update()
    {
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

    public void StartGameAnim()
    {
        
        hint.SetActive(true);
        goal.SetActive(true);
        ready.SetActive(true);
        title.SetActive(false);
        retry.SetActive(false);
        gameover.SetActive(false);
        score.SetActive(true);
        win.SetActive(false);
        next.SetActive(false);
        finalScore.SetActive(false);
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
        finalScore.SetActive(false);
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
        goal.SetActive(false);
        win.SetActive(false);
        next.SetActive(false);
        finalScore.SetActive(false);
        state = 3;
        timer = 0.0f;
        timerInt = 0;
        SetTimer(0);
    }

    public void GameOver()
    {
        state = 4;
        timer = 0;
        finalScore.SetActive(true);

        string text = "score: " + gameManager.GetInstance().GetScore();
        finalScoreText.SetText(text);
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
        string text = "" + s;
        timerText.SetText(text);
    }

    public void SetGoal(string text)
    {
        goal.GetComponent<TextMeshProUGUI>().SetText(text);
    }
}
