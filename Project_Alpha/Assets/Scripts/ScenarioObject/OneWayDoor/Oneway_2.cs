using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oneway_2 : MonoBehaviour {
    public GameObject phisicalDoor;
    public GameObject colliderWithPlayer;
    private bool onlyOne;
    // Use this for initialization
    void Start () {
        phisicalDoor.SetActive(false);
        colliderWithPlayer.GetComponent<SpriteRenderer>().enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        onlyOne = colliderWithPlayer.GetComponent<OneWay>().thisDoorClosed;
        if (onlyOne)
        {
            phisicalDoor.SetActive(true);
            colliderWithPlayer.SetActive(false);
        }
    }
}
