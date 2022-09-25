using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scene : MonoBehaviour
{
    private pipeManager PipeManager;
    private bool clicked = false;
    void Start()
    {
        gameManager.GetInstance().SetScene(this);
        PipeManager = GameObject.Find("pipeManager").GetComponent<pipeManager>();
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
        PipeManager.LoadLevel(lvl);
    }

    public void Pause()
    {
        PipeManager.Pause();
    }

    public void Resume()
    {
        PipeManager.Resume();
    }

    public void Restart()
    {
        PipeManager.Restart();
    }
}
