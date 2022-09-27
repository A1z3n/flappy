using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = System.Random;



public class pipeManager : MonoBehaviour
{

    private bool isPaused = false;
    private List<pipe> pipes;
    private int totalCount = 0;
    private float timer = 0.0f;
    private float dur = 1.5f;
    private GameObject pipePrefab;
    private float lastH = 0.0f;
    public float minH = -2.3f;
    public float maxH = 3.4f;
    public float deltaH = 3.0f;
    private int level;
    void Start()
    {
        pipePrefab = Resources.Load("pipe", typeof(GameObject)) as GameObject;
        pipes = new List<pipe>();
        for (int i = 0; i < 10; i++)
        {
            var p = Instantiate(pipePrefab, new Vector3(-100, -100, -100), Quaternion.identity);
            
            //p.script = p.obj.GetComponent<pipe>();
            pipes.Add(p.GetComponent<pipe>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused) return;
        if (totalCount > 0)
        {
            timer += Time.deltaTime;
            if (timer > dur)
            {
                timer = 0.0f;
                var p = GetFreePipe();
                //float h = lastH;
                var rnd = new Random(Time.frameCount);
                bool fnd = false;
                float h = lastH;
                do
                {
                    
                    h = lastH + deltaH*0.5f - (float)rnd.NextDouble()* deltaH;
                    if (h < maxH && h > minH)
                    {
                        fnd = true;
                    }
                } while (!fnd);

                lastH = h;
                p.Move(20.0f,h ,-10.0f,2.5f);
                //p.script
            }
        }
    }

    public void LoadLevel(int lvl)
    {
        switch (lvl)
        {
            case 0:
                totalCount = 20;
                dur = 1.5f;
                break;
        }

        level = lvl;
    }

    private pipe GetFreePipe()
    {
        
        foreach (var it in pipes)
        {
            if (!it.isMoving())
            {
                return it;
            }
        }
        var p = Instantiate(pipePrefab, new Vector3(-100, -100, -100), Quaternion.identity);
        
        var pp = p.GetComponent<pipe>();
        pipes.Add(pp);
        return pp;
    }

    public void Pause()
    {
        isPaused = true;
        foreach (var p in pipes)
        {
            p.Pause();
        }
    }

    public void Resume()
    {
        isPaused = false;
        foreach (var p in pipes)
        {
            p.Resume();
        }
    }

    public void Restart()
    {
        Resume();
        foreach (var p in pipes)
        {
            p.Reset();
        }
        LoadLevel(level);
    }

}
