using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scene : MonoBehaviour
{
    private int state = 0;
    void Start()
    {
        gameManager.GetInstance().SetScene(this);
        state = 0;
    }

    // Update is called once per frame
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
                    gameManager.GetInstance().GetGui().StartGame();
                    gameManager.GetInstance().GetPlayer().SetState(1);
                    //state = s;
                    break;
            }
        }
    }
}
