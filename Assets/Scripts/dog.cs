using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class dog : MonoBehaviour
{
    private SpriteRenderer _renderer;

    private Transform tr;

    private int state = 0;

    public float destY = -1.13f;
    private float startY;

    public Sprite spr1;
    public Sprite spr2;
    private float timer;
    private bool isFrame1;
    public AudioClip laughSound;
    public AudioClip winSound;
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        tr = GetComponent<Transform>();
        startY = tr.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case 1:
                float y = tr.position.y + Time.deltaTime;
                if (y > startY+destY)
                {
                    state = 2;
                    y = startY+destY;
                }
                tr.position = new Vector3(tr.position.x, y, tr.position.z);
                break;
        }
        timer += Time.deltaTime;

        if (timer >= 0.3f)
        {
            timer -= 0.3f;
            isFrame1 = !isFrame1;
            _renderer.sprite = isFrame1 ? spr1 : spr2;
        }
    }

    public void Animate()
    {
        state = 1;
        GetComponent<AudioSource>().PlayOneShot(laughSound);
    }

    public void Reset()
    {
        state = 0;
        tr.position = new Vector3(tr.position.x, startY, tr.position.z);
    }

    public void Win()
    {
        GetComponent<AudioSource>().PlayOneShot(winSound);
    }
}
