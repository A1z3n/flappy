using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gui : MonoBehaviour
{
    private GameObject title;
    private GameObject ready;
    private GameObject hint;
    private GameObject gameover;
    private GameObject retry;
    private float timer = 0.0f;
    private bool isStartGameAnim = false;
    private bool isStartMMAnim = false;
    void Start()
    {
        title = GameObject.Find("GUI/title");
        ready = GameObject.Find("GUI/ready");
        hint = GameObject.Find("GUI/hint");
        retry = GameObject.Find("GUI/replay");
        retry.GetComponent<Button>().onClick.AddListener(Restart);
        gameover = GameObject.Find("GUI/gameover");
        ready.SetActive(false);
        hint.SetActive(false);
        retry.SetActive(false);
        gameover.SetActive(false);
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
        ready.SetActive(true);
        title.SetActive(false);
        retry.SetActive(false);
        gameover.SetActive(false);

    }

    public void StartMMAnim()
    {
        isStartMMAnim = true;
        ready.SetActive(false);
        hint.SetActive(false);
        title.SetActive(true);
        retry.SetActive(false);
        gameover.SetActive(false);
    }

    public void StartGame()
    {
        ready.SetActive(false);
        hint.SetActive(false);
        title.SetActive(false);
        retry.SetActive(false);
        gameover.SetActive(false);
    }

    public void GameOver()
    {
        retry.SetActive(true);
        gameover.SetActive(true);
    }

    public void Restart()
    {
        gameManager.GetInstance().Restart();
    }
}
