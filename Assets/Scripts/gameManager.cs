using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    public enum eGameState
    {
        kStart,
        kMainMenu,
        kHint,
        kGame,
        kGameOver,
        kWin
    }
    private static readonly gameManager instance = new gameManager();
    private eGameState gameState;
    private gui Gui;
    private player Player;
    private scene Scene;
    private List<ground> grounds;
    private int currentLevel = 1;
    private pipeManager PipeManager;
    private camera mainCamera;
    private bool isDead = false;
    private int loadedLevel = 1;
    private bool loading = false;

    private int score = 0;
    // Start is called before the first frame update

    private gameManager()
    {
        grounds = new List<ground>();
        gameState = eGameState.kStart;
        changeState(eGameState.kMainMenu);
        //currentLevel = PlayerPrefs.GetInt("level");
        currentLevel = 1;

        mainCamera = Camera.main.GetComponent<camera>();
    }

    public static gameManager GetInstance()
    {
        return instance;
    }

    public void SetGui(gui g)
    {
        Gui = g;
        if (loading) {

            Gui.SetGoal("level: " + currentLevel + " goal: " + PipeManager.GetTotalCount());
            Gui.StartGameAnim();
            loading = false;
        }
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
        PipeManager = s.GetPipeManager();
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

                LoadLevel(currentLevel);
                //PipeManager.Pause();
                if (!loading) {
                    Gui.SetGoal("level: " + currentLevel + " goal: " + PipeManager.GetTotalCount());
                    Gui.StartGameAnim();
                }

                break;
            case eGameState.kGame:
                Scene.Resume();
                //PipeManager.Resume();
                Player.SetState(1);
                Player.Fly();
                Gui.StartGame();
                break;
            case eGameState.kGameOver:
                //Gui.StartMMAnim();
                Gui.GameOver();
                break;
            case eGameState.kWin:
                Gui.Win();
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

    public void Die() {
        isDead = true;
        Player.Die();
        changeState(eGameState.kGameOver);
        foreach (var g in grounds)
        {
            g.Pause();
        }
        Scene.Pause();
    }

    public void Restart() {
        isDead = false;
        Gui.StartGame();
        Player.Restart();
        Scene.Restart();
        //foreach (var g in grounds)
        //{
        //    g.Resume();
        //}
        score = 0;
        Gui.SetScore(score);
        changeState(eGameState.kHint);

    }

    public void StartGame() {
        isDead = false;
        Scene.LoadLevel(currentLevel);
        Gui.StartGame();
        Player.SetState(1);
        Player.Fly();
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
            case eGameState.kWin:
                //NextLevel();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
            
    }

    public void Win() {
        if (isDead) return;
        changeState(eGameState.kWin);
        Scene.Pause();
        Player.Win();
        foreach (var g in grounds)
        {
            g.Pause();
        }
    }

    public void NextLevel() {
        currentLevel++;
        PlayerPrefs.SetInt("level", currentLevel);
        Restart();
    }

    public void LoadLevel(int lvl) {
        if (loadedLevel != lvl) {
            SceneManager.LoadScene(lvl-1);
            loading = true;
        }
        loadedLevel = lvl;
        Scene.LoadLevel(lvl);
    }
}
