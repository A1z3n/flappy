
using UnityEngine;

public class scene : MonoBehaviour
{
    private bool clicked = false;
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
        
    }

    public int GetTotalCount() {
        var pipes = GameObject.Find("pipes");
        return pipes.transform.childCount;
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
