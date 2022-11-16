using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
public class sObject
{
    public int count;
    public GameObject obj;
    public Vector3 pos;
    public Vector3 startpos;
    public float sizex;
    public int i = 0;

    public void AddCount(int value)
    {
        pos = obj.transform.position;
        i++;
        pos.x =  startpos.x + i* sizex * count * 4.0f;
        obj.transform.position = pos;
    }

    public void Reset() {
        pos = startpos;
        i = 0;
        obj.transform.position = pos;
    }
}
public class ground : MonoBehaviour
{
    private List<sObject> list;
    public float speed = 1.0f;
    public float sizex = 1.68f;
    public float borderX = 8.0f;
    public string path = "bg";
    private int count = 6;
    private bool paused = false;
    private double shift = 0.0;
    void Start() {
        var root = GameObject.Find(path);
        count = root.transform.childCount;
        list = new List<sObject>();
        for (int i = 1; i <= count; i++)
        {
            var v = GameObject.Find(path + "/"+i);
            if(v!=null)
            {
                sObject s = new sObject();
                s.count = count;
                s.obj = v;
                s.pos = v.transform.position;
                s.startpos = s.pos;
                s.sizex = sizex;
                list.Add(s);
            }
        }

        paused = true;
       
        gameManager.GetInstance().AddGround(this);
    }

        // Update is called once per frame
    void Update()
    {
        if (paused) return;
        int i = 0;
        float cx = Camera.main.transform.position.x;
        shift -= speed*Time.deltaTime;
        foreach (var it in list)
        {   
            Vector3 pos = it.obj.transform.position;
            pos.x = (float)shift +it.startpos.x + it.i*count*sizex*4.0f;
            it.obj.transform.position = pos;
            if (cx - pos.x> borderX)
            {
                it.AddCount(count);
            }
            i++;
        }
    }

    public void Pause()
    {
        paused = true;
    }

    public void Resume()
    {
        paused = false;
    }

    public void Restart() {
        Pause();
        foreach (var it in list) {
            it.Reset();
        }

        shift = 0.0;
    }
}
