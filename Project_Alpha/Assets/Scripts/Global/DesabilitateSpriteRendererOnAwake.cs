using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesabilitateSpriteRendererOnAwake : MonoBehaviour
{
    
	void Awake ()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
	}
}
