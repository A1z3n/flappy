using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TPipe
{
    public GameObject obj;
    public bool free = true;
    public Vector3 pos;
    public pipe script;
}
public class pipeManager : MonoBehaviour
{

    
    private List<TPipe> pipes;
    private int totalCount = 0;
    private float timer = 0.0f;
    private float dur = 1.0f;
    private GameObject pipePrefab;


    void Start()
    {
        pipePrefab = Resources.Load("pipe", typeof(GameObject)) as GameObject;
        pipes = new List<TPipe>();
        for (int i = 0; i < 10; i++)
        {
            TPipe p = new TPipe();
            p.obj = Instantiate(pipePrefab, new Vector3(-100, -100, -100), Quaternion.identity);
            p.free = true;
            p.pos = new Vector3(-100, -100, -100);
            p.script = p.obj.GetComponent<pipe>();
            pipes.Add(p);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (totalCount > 0)
        {
            timer += Time.deltaTime;
            if (timer > dur)
            {
                timer = 0.0f;
                var p = GetFreePipe();
                p.script.Move(20.0f,0.0f,-10.0f,2.5f);
                p.free = false;
                p.pos.x = 20.0f;
                p.pos.y = 0.0f;
                p.pos.z = 1.0f;
                //p.script
            }

            foreach (var p in pipes)
            {
                p.free = !p.script.isMoving();
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
    }

    private TPipe GetFreePipe()
    {
        
        foreach (var it in pipes)
        {
            if (it.free)
            {
                return it;
            }
        }
        TPipe p = new TPipe();
        p.obj = Instantiate(pipePrefab, new Vector3(-100, -100, -100), Quaternion.identity);
        p.free = true;
        p.pos = new Vector3(-100, -100, -100);
        pipes.Add(p);
        return p;
    }


}
