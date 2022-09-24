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
        kGame,
        kGameOver
    }
    private static readonly gameManager instance = new gameManager();
    private eGameState gameState;
    private gui Gui;
    private player Player;
    private scene Scene;

    private bool bStartGame;
    // Start is called before the first frame update

    private gameManager()
    {
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
    public void changeState(eGameState state)
    {
        switch (state)
        {
            case eGameState.kStart:
                break;
            case eGameState.kMainMenu:

                break;
            case eGameState.kGame:
                Gui.StartGameAnim();
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
    }
}
