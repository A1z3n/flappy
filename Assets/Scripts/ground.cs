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
    public float posx;

    public void AddCount(int value)
    {
        count+=value;
    }
}
public class ground : MonoBehaviour
{


    
    
    // Start is called before the first frame update
    private List<sObject> list;
    public float speed = 2.5f;
    public float sizex = 1.68f;
    public float borderX = 18.0f;
    public string path = "ground";
    public int count = 7;
    private double shift = 0.0;

    void Start()
    {
        list = new List<sObject>();
        for (int i = 1; i <= count; i++)
        {
            var v = GameObject.Find(path + "/"+i);
            if(v!=null)
            {
                //v.GetComponent<Transform>().position = new Vector3(sizex * (i-1), v.GetComponent<Transform>().position.y,
                // v.GetComponent<Transform>().position.z);
                sObject s = new sObject();
                s.count = i;
                s.obj = v;
                s.posx = v.GetComponent<Transform>().position.x;
                //s.id = i;
                list.Add(s);
            }
        }
    }

        // Update is called once per frame
    void Update()
    {
        shift += Time.deltaTime * speed;
        int i = 0;
        foreach (var it in list)
        {   
            Vector3 pos = it.obj.GetComponent<Transform>().position;
            pos.x = (it.count-3)*sizex*4 - (float)shift;
            it.obj.GetComponent<Transform>().position = pos;
            if (pos.x < -borderX)
            {
                it.AddCount(count);
            }
            i++;
        }
    }
}
