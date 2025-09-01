
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class scene : MonoBehaviour
{
    private bool clicked = false;
    private List<pausableObject> pausableObjects;
    void Awake()
    {
        gameManager.GetInstance().SetScene(this);
        pausableObjects = new List<pausableObject>();
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
        
    }

    public int GetTotalCount() {
        var pipes = GameObject.Find("pipes");
        return pipes.transform.childCount;
    }

    public void Pause()
    {
        foreach (var p in pausableObjects)
        {
            p.Pause();
        }
    }

    public void Resume()
    {
        foreach (var p in pausableObjects)
        {
            p.Resume();
        }
    }

    public void Restart()
    {
        foreach (var p in pausableObjects)
        {
            p.Restart();
        }
    }

    public void AddPausableObject(pausableObject p)
    {
        pausableObjects.Add(p);
    }


}
