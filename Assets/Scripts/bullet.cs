using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class bullet : pausableObject
{
    private Transform tr;
    private Vector3 startPos;
    private Vector3 pos;
    public AudioClip bulletSound;
    private AudioSource audioSource;
    private bool shooted = false;
    private float width = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        pos = tr.position;
        startPos = pos;
        audioSource = GetComponent<AudioSource>();
        width = GetComponent<CircleCollider2D>().radius * 2;
        gameManager.GetInstance().GetScene().AddPausableObject(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused) return;
        if (shooted)
        {
            pos.x -= Time.deltaTime * 5.0f;
        }
        else
        {
            
            float cameraHeight = 2f * Camera.main.orthographicSize;
            float cameraWidth = cameraHeight * Camera.main.aspect;
            if (pos.x - width < Camera.main.transform.position.x + cameraWidth / 2)
            {
                Shot();
            }
        }

        tr.position = pos;
    }

    public override void Restart()
    {
        isPaused = true;
        pos = startPos;
        shooted = false;
        tr.position = pos;  
    }

    private void Shot()
    {
        if (!shooted)
        {
            shooted = true;
            if(audioSource && audioSource.enabled)
                audioSource.PlayOneShot(bulletSound);
        }
    }

}
