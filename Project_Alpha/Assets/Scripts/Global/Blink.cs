using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{

    public bool blinking = false;
    public int frameToBlink = 2;

    private int blinkedFrame = 0;

    private Color savedNormalColor;
    private GameObject savedGameObjectToBlink;

    void Update ()
    {
		if(blinking)
        {
            if(blinkedFrame <= frameToBlink)
            {
                blinkedFrame++;
            }
            else
            {
                savedGameObjectToBlink.GetComponent<SpriteRenderer>().color = savedNormalColor;

                blinkedFrame = 0;
                blinking = false;
            }
        }
	}

    public void DoBlink(Color blinkColor, Color normalColor, int blinkTime, GameObject gameObjectToBlink)
    {
        blinking = true;
        frameToBlink = blinkTime;

        savedNormalColor = normalColor;
        savedGameObjectToBlink = gameObjectToBlink;

        gameObjectToBlink.GetComponent<SpriteRenderer>().color = blinkColor;
    }
}
