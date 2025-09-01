
using System.Collections.Generic;
using Assets.Scripts;
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
public class ground : pausableObject
{
    private List<sObject> list;
    public float speed = 1.0f;
    public float sizex = 1.68f;
    public float borderX = 8.0f;
    public string path = "bg";
    private int count = 6;
    private double shift = 0.0;
    private Transform cameraTransform; // Кешируем камеру
    
    void Start() {
        // Кешируем камеру один раз
        cameraTransform = Camera.main.transform;
        
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

        gameManager.GetInstance().AddGround(this);
        gameManager.GetInstance().GetScene().AddPausableObject(this);
    }

        // Update is called once per frame
    void Update()
    {
        if (isPaused) return;
        
        // Используем кешированную ссылку
        float cx = cameraTransform.position.x;
        shift -= speed * Time.deltaTime;
        
        foreach (var it in list)
        {   
            Vector3 pos = it.obj.transform.position;
            pos.x = (float)shift + it.startpos.x + it.i * count * sizex * 4.0f;
            it.obj.transform.position = pos;
            if (cx - pos.x > borderX)
            {
                it.AddCount(count);
            }
        }
    }


    public override void Restart() {
        Pause();
        foreach (var it in list) {
            it.Reset();
        }

        shift = 0.0;
    }
}
