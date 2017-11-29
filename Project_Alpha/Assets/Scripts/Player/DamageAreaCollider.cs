using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class DamageAreaCollider : MonoBehaviour
    {
        public bool onTriggerEnter = false;
        public bool onTriggerExit = false;
        public bool onTriggerStay = false;
        public Collider2D whoIs;

        private void OnTriggerEnter2D(Collider2D other)
        {
            //Debug.Log("Enter");
            onTriggerEnter = true;
            onTriggerExit = false;
            onTriggerStay = false;
            whoIs = other;
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            //Debug.Log("Exit");
            onTriggerEnter = false;
            onTriggerExit = true;
            onTriggerStay = false;
            whoIs = null;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            //Debug.Log("Stay");
            onTriggerEnter = false;
            onTriggerExit = false;
            onTriggerStay = true;
            whoIs = other;
        }
    }
}
