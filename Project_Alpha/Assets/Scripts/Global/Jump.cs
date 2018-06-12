using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    public GameObject singleJump;
    private GameObject player;

    private void Update()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if(singleJump == null)
        {
            switch (gameObject.name)
            {
                case "Jump1":
                    singleJump = GameObject.Find("Jump_1");
                    break;

                case "Jump2":
                    singleJump = GameObject.Find("Jump_2");
                    break;

                case "Jump3":
                    singleJump = GameObject.Find("Jump_3");
                    break;

                default:
                    break;
            }

        }
    }

    public void Jumping ()
    {
        player.transform.position = singleJump.transform.position;
	}
}
