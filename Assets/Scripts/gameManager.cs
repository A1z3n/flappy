using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public enum eGameState
    {
        kStart,
        kMainMenu,
        kHint,
        kGame,
        kGameOver
    }
    private static readonly gameManager instance = new gameManager();
    private eGameState gameState;
    private gui Gui;
    private player Player;
    private scene Scene;
    private List<ground> grounds;
    private int currentLevel = 0;

    private int score = 0;
    // Start is called before the first frame update

    private gameManager()
    {
        grounds = new List<ground>();
        gameState = eGameState.kStart;
        changeState(eGameState.kMainMenu);
    }

    public static gameManager GetInstance()
    {
        return instance;
    }

    public void SetGui(gui g)
    {
        Gui = g;
    }

    public gui GetGui()
    {
        return Gui;
    }

    public void SetPlayer(player p)
    {
        Player = p;
    }

    public player GetPlayer()
    {
        return Player;
    }
    public void SetScene(scene s)
    {
        Scene = s;
    }

    public scene GetScene()
    {
        return Scene;
    }

    public void AddGround(ground g)
    {
        grounds.Add(g);
    }
    public void changeState(eGameState state)
    {
        switch (state)
        {
            case eGameState.kStart:
                break;
            case eGameState.kMainMenu:

                break;
            case eGameState.kHint:
                Gui.StartGameAnim();
                break;
            case eGameState.kGame:
                Player.SetState(1);
                Player.Fly();
                Gui.StartGame();
                Scene.LoadLevel(currentLevel);
                break;
            case eGameState.kGameOver:
                //Gui.StartMMAnim();
                Gui.GameOver();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        gameState = state;
    }

    public eGameState GetGameState()
    {
        return gameState;
    }

    public void Die()
    {
        Player.Die();
        changeState(eGameState.kGameOver);
        foreach (var g in grounds)
        {
            g.Pause();
        }
        Scene.Pause();
    }

    public void Restart()
    {
        Gui.StartGame();
        Player.Restart();
        Scene.Restart();
        foreach (var g in grounds)
        {
            g.Resume();
        }
        changeState(eGameState.kGame);
        score = 0;
    }

    public void StartGame()
    {
        Gui.StartGame();
        Player.SetState(1);
        Player.Fly();
        Scene.LoadLevel(currentLevel);
    }

    public void AddScore()
    {
        score++;
        Gui.SetScore(score);
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void Click()
    {
        switch (gameState)
        {
            case eGameState.kStart:
                break;
            case eGameState.kMainMenu:
                changeState(eGameState.kHint);
                break;
            case eGameState.kHint:
                changeState(eGameState.kGame);
                break;
            case eGameState.kGame:
                Player.Click();
                break;
            case eGameState.kGameOver:
                //Restart();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
            
    }
}
