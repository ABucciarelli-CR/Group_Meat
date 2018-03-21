using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToBeContinued : MonoBehaviour
{
    public int movement = 1;

    public bool startMoving = false;

    void Update ()
    {
        if(startMoving)
        {
            if(gameObject.transform.position.x > 200)
            {
                gameObject.transform.position = new Vector2(gameObject.transform.position.x - movement * Time.unscaledDeltaTime, gameObject.transform.position.y);
            }
            else
            {
                startMoving = false;
            }
        }
	}

    private void Activation()
    {
        startMoving = true;
        //StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(4);
        startMoving = true;
    }
}
