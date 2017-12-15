using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheArenaDoor
{
    public class ArenaDoor : MonoBehaviour
    {
        public GameObject phisicalDoor;
        public GameObject colliderWithPlayer;


		private ArenaDoorExitCollider doorExitCollider;

        // Use this for initialization
        void Start()
        {
			doorExitCollider = colliderWithPlayer.GetComponent<ArenaDoorExitCollider>();

            phisicalDoor.SetActive(false);
            colliderWithPlayer.GetComponent<SpriteRenderer>().enabled = false;

        }

        // Update is called once per frame
        void Update()
        {
			if (doorExitCollider.thisDoorClosed)
			{
				phisicalDoor.SetActive (true);
				colliderWithPlayer.SetActive (false);
			} 
			else
			{
				phisicalDoor.SetActive (false);
			}
        }
    }
}
