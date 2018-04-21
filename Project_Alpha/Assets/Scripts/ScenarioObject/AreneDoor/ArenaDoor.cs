using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheArenaDoor
{
    public class ArenaDoor : MonoBehaviour
    {
        public GameObject phisicalDoor;
        public GameObject colliderWithPlayer;
        public bool becomeVisible = true;

        private GlobalVariables globalVariables;

        private bool onlyOne = true;

        private ArenaDoorExitCollider doorExitCollider;

        // Use this for initialization
        void Start()
        {
            globalVariables = GameObject.Find("GameManager").GetComponent<GlobalVariables>();

            doorExitCollider = colliderWithPlayer.GetComponent<ArenaDoorExitCollider>();

            phisicalDoor.SetActive(false);
            colliderWithPlayer.GetComponent<SpriteRenderer>().enabled = false;

        }

        // Update is called once per frame
        void Update()
        {
			if (doorExitCollider.thisDoorClosed && onlyOne)
			{
                onlyOne = false;
                //globalVariables.enemyDead = 0;
                phisicalDoor.SetActive (true);
                if(!becomeVisible)
                {
                    phisicalDoor.GetComponent<SpriteRenderer>().enabled = false;
                }
				colliderWithPlayer.SetActive (false);
            }
            /*
            if(!globalVariables.closeDoor)
            {
                onlyOne = true;
                phisicalDoor.SetActive(false);
            }*/
        }
    }
}
