using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class TheAnimator : MonoBehaviour
{
    public Sprite[] animationSprite;
    public SpriteRenderer sprtRenderer;
    public Image image;
    public Shader shader;
    public float fpsMin = .01f;
    private float deltaTime = 0;
    private float frame = 0;
    private int lastFrame;
    public bool loop = false;
    public bool ignorePause = false;
    public bool useImage = false;
    public bool deactivate = false;
    public bool randomFramerate = false;
    [EnableIf("randomFramerate")]
    public float fpsMax = 10;
    private int startingFrameForLoop = 0;
    private float fps;
    
    private bool destroy = false;

    private void Awake()
    {
        lastFrame = animationSprite.Length - 1;
        if(sprtRenderer != null)
        {
            sprtRenderer.enabled = true;
        }
        if (image != null)
        {
            image.enabled = true;
        }
        
    }

    void Update ()
    {
        if(randomFramerate)
        {
            fps = Random.Range(fpsMin, fpsMax);
        }
        else
        {
            fps = fpsMin;
        }

        if(ignorePause)
        {
            deltaTime += Time.unscaledDeltaTime;
        }
        else
        {
            deltaTime += Time.deltaTime;
        }
        
        while(deltaTime >= fps)
        {
            deltaTime = 0;
            frame++;

            if(useImage)
            {
                image.sprite = animationSprite[(int)frame];
            }
            else
            {
                sprtRenderer.sprite = animationSprite[(int)frame];
                if (shader != null)
                {
                    sprtRenderer.material.shader = shader;
                }
            }
            
            //sprtRenderer.material.color = Color.black;
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

        if(destroy && !deactivate)
        {
            Destroy(this.gameObject);
        }
        else if (destroy && deactivate)
        {
            gameObject.SetActive(false);
        }
	}

    private void Loop(/*int[] strartAndEnd*/)
    {
        //startingFrameForLoop = strartAndEnd[0];
        //lastFrame = strartAndEnd[1];
        loop = true;
    }
}
