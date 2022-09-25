using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scene : MonoBehaviour
{
    private bool isRandomPipesMode = false;
    private pipeManager PipeManager;
    void Start()
    {
        gameManager.GetInstance().SetScene(this);
        PipeManager = GameObject.Find("pipeManager").GetComponent<pipeManager>();
    }
    
    void Update()
    {
        bool touched = false;
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Ended)
            {
                touched = true;
                break;
            }
        }

        if (Input.GetButtonUp("Jump") || Input.GetMouseButtonUp(0) || touched)
        {
         
            var s = gameManager.GetInstance().GetGameState();
            switch (s)
            {
                case gameManager.eGameState.kMainMenu:
                    gameManager.GetInstance().changeState(gameManager.eGameState.kGame);
                    //state = s;
                    break;
                case gameManager.eGameState.kGame:
                    gameManager.GetInstance().StartGame();
                    //state = s;
                    break;
            }
        }
        
    }

    public void LoadLevel(int lvl)
    {
        PipeManager.LoadLevel(lvl);
    }
    
    
}
