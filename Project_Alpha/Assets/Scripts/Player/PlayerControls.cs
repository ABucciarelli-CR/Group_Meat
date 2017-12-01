using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerMain))]
    public class PlayerControls : MonoBehaviour
    {
    
        private PlayerMain playerMain;
        private AttackAndGrab attackAndGrab;
        private float leftAndRightMovement;
        private bool attack = false;
        //private bool grab = false;
        private bool jump = false;
        private bool dash = false;


        // Use this for initialization
        void Awake()
        {
            playerMain = GetComponent<PlayerMain>();
            attackAndGrab = GetComponent<AttackAndGrab>();
        }
        
        void Update()
        {
            leftAndRightMovement = Input.GetAxis("Horizontal");
            attack = Input.GetButtonDown("Attack");
            //grab = Input.GetButtonDown("Grab");
            dash = Input.GetButtonDown("Dash");

            //if (!jump)
            //{
            // Read the jump input in Update so button presses aren't missed.
            jump = Input.GetButtonDown("Jump");
            //}


            playerMain.Move(jump, leftAndRightMovement, dash);
            attackAndGrab.AttackEnemy(attack);
            //attackAndGrab.GrabEnemy(grab);


            jump = false;



        }
    }
}
