using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAppear : MonoBehaviour
{
    public string appearingText;
    public TextMesh text;
    public float textVelocity = .05f;
    public float textPause = .5f;

    public bool active = false;
    public bool textInDoing = false;

    private void Awake()
    {
        text = gameObject.GetComponent<TextMesh>();
        appearingText = text.text;
        text.text = "";
    }

    void Update ()
    {
		if(active)
        {
            active = false;
            textInDoing = true;
            StartCoroutine(TextAppearing());
        }
	}

    private void StartText()
    {
        active = true;
    }

    IEnumerator TextAppearing()
    {
        int i = 0;
        while (i < appearingText.Length)
        {
            text.text += appearingText[i++];

            if (appearingText[i-1] == ',')
            {
                yield return new WaitForSeconds(textPause);
            }
            else
            {
                yield return new WaitForSeconds(textVelocity);
            }
        }
    }
}
