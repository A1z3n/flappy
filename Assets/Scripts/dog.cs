using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class dog : MonoBehaviour
{
    private SpriteRenderer renderer;

    private Transform transform;

    private int state = 0;

    private float destY = -2.5f;
    private float startY;

    public Sprite spr1;
    public Sprite spr2;
    private float timer;
    private bool isFrame1;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        transform = GetComponent<Transform>();
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case 1:
                float y = transform.position.y + Time.deltaTime;
                if (y > destY)
                {
                    state = 2;
                    y = destY;
                }
                transform.position = new Vector3(transform.position.x, y, transform.position.z);
                break;
        }
        timer += Time.deltaTime;

        if (timer >= 0.3f)
        {
            timer -= 0.3f;
            isFrame1 = !isFrame1;
            renderer.sprite = isFrame1 ? spr1 : spr2;
        }
    }

    public void Animate()
    {
        state = 1;
    }

    public void Reset()
    {
        state = 0;
        transform.position = new Vector3(transform.position.x, startY, transform.position.z);
    }
}
