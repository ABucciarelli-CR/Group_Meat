using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerMain))]
    public class PlayerControls : MonoBehaviour
    {
    
        private PlayerMain playerMain;
        private float leftAndRightMovement;
        private bool jump = false;


        // Use this for initialization
        public void Awake()
        {
            playerMain = GetComponent<PlayerMain>();
        }
        
        public void Update()
        {
            leftAndRightMovement = Input.GetAxis("Horizontal");

            //if (!jump)
            //{
                // Read the jump input in Update so button presses aren't missed.
                jump = Input.GetButtonDown("Jump");
            //}


            playerMain.Move(jump, leftAndRightMovement);
            jump = false;

        }
    }
}
