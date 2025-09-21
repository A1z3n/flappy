using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour
{
    private AudioSource music;
    private AudioSource win;
    private AudioSource lose;
    void Start()
    {
        music = GameObject.Find("sounds/music").GetComponent<AudioSource>();
        win = GameObject.Find("sounds/win").GetComponent<AudioSource>();
        lose = GameObject.Find("sounds/lose").GetComponent<AudioSource>();
        gameManager.GetInstance().SetSoundManager(this);
    }

    public void PlayMusic()
    {
        music.Play();
        win.Stop();
        lose.Stop();
    }

    public void StopMusic()
    {
        music.Stop();
    }

    public void PlayWinSound()
    {
        lose.Stop();
        win.Play();
        StopMusic();
    }

    public void PlayLoseSound()
    {
        win.Stop();
        lose.Play();
        StopMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
