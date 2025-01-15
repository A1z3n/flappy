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
    private camera mainCamera;
    private bool isDead = false;
    private int loadedLevel = 1;
    private bool loading = false;
    private List<pipe> pipes;
    private int score = 0;
    // Start is called before the first frame update

    private gameManager()
    {
        grounds = new List<ground>();
        gameState = eGameState.kStart;
        changeState(eGameState.kMainMenu);
        currentLevel = PlayerPrefs.GetInt("level");
        currentLevel = 2;
        pipes = new List<pipe>();
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
            Gui.SetGoal("level: " + currentLevel + " goal: " + Scene.GetTotalCount());
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
                if (!loading)
                {
                    Gui.SetGoal("level: " + currentLevel + " goal: " + Scene.GetTotalCount());
                    Gui.StartGameAnim();
                }
                break;
            case eGameState.kGame:
                StartGame();
                break;
            case eGameState.kGameOver:
                //Gui.StartMMAnim();
                Pause();
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

    public void Die(string trigger = "") {
        isDead = true;
        Player.Die();
        changeState(eGameState.kGameOver);
        Pause();
        if (trigger == "dog")
        {
            Camera.main.GetComponent<camera>().MoveAnim(-1.5f,1.0f);
            var dog = GameObject.Find("dog").GetComponent<global::dog>();
            dog.Animate();
        }
    }

    public void Restart() {
        isDead = false;
        Gui.StartGame();
        Player.Restart();
        Scene.Restart();
        foreach (var g in grounds)
        {
            g.Restart();
        }
        foreach (var p in pipes)
        {
            p.Restart();
        }
        score = 0;
        Gui.SetScore(score);
        changeState(eGameState.kHint);
        Camera.main.GetComponent<camera>().CancelAnimations();
        if (currentLevel == 2)
        {
            var dog = GameObject.Find("dog").GetComponent<global::dog>();
            if (dog)
                dog.Reset();
        }

    }

    public void StartGame() {
        isDead = false;
       // Scene.LoadLevel(currentLevel);
        Gui.StartGame();
        Player.Ready();
        Resume();
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
        Player.Win();
        Pause();
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
            grounds.Clear();
            pipes.Clear();
        }
        loadedLevel = lvl;
        Scene.LoadLevel(lvl);
        Pause();
    }

    public void RunAnimation(string name) {
        if (name == "portal") {
            var portal = GameObject.Find("portal");
            //if(portal!=null)
            
            portal.GetComponent<SpriteRenderer>().color = new Color(255.0f,255.0f,255.0f,255.0f);
            Player.SetState(player.ePlayerState.kAnimation);
        }
    }

    public void AddPipe(pipe p) {
        pipes.Add(p);
    }

    public void Pause()
    {
        Scene.Pause();
        foreach (var p in pipes) {
            p.Pause();
        }
        foreach (var g in grounds)
        {
            g.Pause();
        }
    }

    public void Resume()
    {
        Scene.Resume();
        foreach (var p in pipes)
        {
            p.Resume();
        }
        foreach (var g in grounds)
        {
            g.Resume();
        }
    }


    public int GetScore()
    {
        return score;
    }
}
