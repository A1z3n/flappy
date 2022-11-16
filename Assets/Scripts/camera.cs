using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.iOS;
using UnityEngine;

public class camera : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    private float cameraPos = -10;
    private float shift = 0.0f;
    private Vector3 startPos;

    void Start()
    {
        player = GameObject.Find("player");
        Vector3 pos = transform.position;
        shift = pos.x-player.transform.position.x;
        cameraPos = player.transform.position.x + shift;
        startPos = pos;
    }

    // Update is called once per frame
    void Update() {

        cameraPos = player.transform.position.x + shift;
        transform.position = new Vector3(cameraPos, transform.position.y, transform.position.z);
    }


}
