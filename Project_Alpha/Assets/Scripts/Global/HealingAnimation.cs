using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingAnimation : MonoBehaviour
{
    public Sprite[] healing;
    public SpriteRenderer sprtRenderer;
    private float fps = .01f;
    private float deltaTime = 0;
    private float frame = 0;

    private bool destroy = false;
    
	void Update ()
    {
        deltaTime += Time.deltaTime;
        while(deltaTime >= fps)
        {
            deltaTime = 0;
            frame++;

            sprtRenderer.sprite = healing[(int)frame];
        }

        if(frame >= healing.Length - 1)
        {
            destroy = true;
        }

        if(destroy)
        {
            Destroy(this.gameObject);
        }
	}
}
