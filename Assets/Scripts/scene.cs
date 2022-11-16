using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class scene : MonoBehaviour
{
    private pipeManager PipeManager;
    private ground Ground;
    private bool clicked = false;
    void Start()
    {
        PipeManager = GameObject.Find("pipeManager").GetComponent<pipeManager>();
        Ground = GameObject.Find("bg").GetComponent<ground>();
        gameManager.GetInstance().SetScene(this);
    }
    
    void Update()
    {
        bool touched = false;
        bool touchup = false;
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                touched = true;
                break;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                touchup = true;
                break;
            }
        }

        if (clicked && (Input.GetButtonUp("Jump") || touchup))
        {
            clicked = false;
        }

        if (!clicked && (Input.GetButtonDown("Jump") || touched))
        {
            gameManager.GetInstance().Click();
        }

    }

    public void LoadLevel(int lvl)
    {
        switch (lvl)
        {
            case 1:
                PipeManager.LoadLevel(10,3.0f,1.5f);
                SetMoveSpeed(2.5f);
                break;
            case 2:
                PipeManager.LoadLevel(15, 3.5f, 1.45f);
                SetMoveSpeed(2.6f);
                break;
            case 3:
                PipeManager.LoadLevel(20, 4.0f, 1.4f);
                SetMoveSpeed(2.7f);
                break;
        }
        
        
    }

    public void Pause()
    {
        PipeManager.Pause();
        Ground.Pause();
    }

    public void Resume()
    {
        PipeManager.Resume();
        Ground.Resume();
    }

    public void Restart()
    {
        PipeManager.Restart();
        Ground.Restart();
    }

    public pipeManager GetPipeManager()
    {
        return PipeManager;
    }

    public void SetMoveSpeed(float s) {
        PipeManager.speed = s;
        //Ground.speed = s;
    }
}
