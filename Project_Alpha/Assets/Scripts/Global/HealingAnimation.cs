using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingAnimation : MonoBehaviour
{
    public Sprite[] healing;
    public SpriteRenderer sprtRenderer;
    public float fps = .01f;
    private float deltaTime = 0;
    private float frame = 0;
    private int lastFrame;
    public bool loop = false;
    private int startingFrameForLoop = 0;
    

    private bool destroy = false;

    private void Awake()
    {
        lastFrame = healing.Length - 1;
    }

    void Update ()
    {
        deltaTime += Time.deltaTime;
        while(deltaTime >= fps)
        {
            deltaTime = 0;
            frame++;

            sprtRenderer.sprite = healing[(int)frame];
        }

        if(frame >= lastFrame)
        {
            if(loop)
            {
                frame = startingFrameForLoop;
            }
            else
            {
                destroy = true;
            }
        }

        if(destroy)
        {
            Destroy(this.gameObject);
        }
	}

    private void Loop(/*int[] strartAndEnd*/)
    {
        //startingFrameForLoop = strartAndEnd[0];
        //lastFrame = strartAndEnd[1];
        loop = true;
    }
}
