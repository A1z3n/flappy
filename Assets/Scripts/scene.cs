using System.Collections;
using System.Collections.Generic;
using Mono.CompilerServices.SymbolWriter;
using UnityEditor.SearchService;
using UnityEngine;

public class scene : MonoBehaviour
{
    private bool clicked = false;
    private int totalCount = 0;
    void Start()
    {
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

    public void LoadLevel(int lvl) {
        var pipes =  GameObject.Find("pipes");
        totalCount = pipes.transform.childCount;
    }

    public int GetTotalCount() {
        return totalCount;
    }

    public void Pause()
    {
       
    }

    public void Resume()
    {
        
    }

    public void Restart()
    {
       
    }



}
