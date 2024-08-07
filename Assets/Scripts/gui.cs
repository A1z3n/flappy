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
    private GameObject goal;
    private GameObject win;
    private GameObject next;
    private TextMeshProUGUI scoreText;
    private float timer = 0.0f;
    private bool isStartGameAnim = false;
    private bool isStartMMAnim = false;
    private gameManager.eGameState state;
    void Start()
    {
        title = GameObject.Find("GUI/title");
        ready = GameObject.Find("GUI/ready");
        hint = GameObject.Find("GUI/hint");
        retry = GameObject.Find("GUI/replay");
        retry.GetComponent<Button>().onClick.AddListener(Restart);
        gameover = GameObject.Find("GUI/gameover");
        score = GameObject.Find("GUI/score");
        goal = GameObject.Find("GUI/goal");
        win = GameObject.Find("GUI/win");
        next = GameObject.Find("GUI/next");
        scoreText = score.GetComponent<TextMeshProUGUI>();
        score.SetActive(false);
        ready.SetActive(false);
        hint.SetActive(false);
        retry.SetActive(false);
        gameover.SetActive(false);
        goal.SetActive(false);
        win.SetActive(false);
        next.SetActive(false);
        next.GetComponent<Button>().onClick.AddListener(NextLevel);
        gameManager.GetInstance().SetGui(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (isStartGameAnim)
        {

        }
        else if (isStartMMAnim)
        {

        }

    }

    public void StartGameAnim()
    {
        isStartGameAnim = true;
        
        hint.SetActive(true);
        goal.SetActive(true);
        ready.SetActive(true);
        title.SetActive(false);
        retry.SetActive(false);
        gameover.SetActive(false);
        score.SetActive(true);
        win.SetActive(false);
        next.SetActive(false);
        state = gameManager.eGameState.kHint;
    }

    public void StartMMAnim()
    {
        isStartMMAnim = true;
        ready.SetActive(false);
        hint.SetActive(false);
        title.SetActive(true);
        retry.SetActive(false);
        gameover.SetActive(false);
        win.SetActive(false);
        next.SetActive(false);
        state = gameManager.eGameState.kMainMenu;
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
        state = gameManager.eGameState.kGame;
    }

    public void GameOver()
    {
        retry.SetActive(true);
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

    public void SetGoal(string text)
    {
        goal.GetComponent<TextMeshProUGUI>().SetText(text);
    }
}
