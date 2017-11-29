using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerMain))]
    public class AttackAndGrab : MonoBehaviour
    {

        public int normalDamage = 10;
        public int grabDamage = 5;


        private bool enemyIsGrabbed = false;
        [SerializeField] private GameObject damageAreaGameObject;
        private DamageAreaCollider damageAreaCollider;
        private Animator anim;


        void Awake()
        {
            anim = GetComponent<Animator>();
            damageAreaCollider = damageAreaGameObject.GetComponent<DamageAreaCollider>();
        }



        public void AttackEnemy(bool attack)
        {
            if(attack)
            {
                anim.SetBool("Attack", true);
                Debug.Log("DebugghiOut");
                WaitForAnimation(anim.GetComponent<Animation>());
                anim.SetBool("Attack", false);
            }
            if (attack && damageAreaCollider.whoIs != null)
            {

                //Debug.Log("Attacked");
                damageAreaCollider.whoIs.gameObject.SendMessage("Damage", normalDamage);
                //Debug.Log(damageAreaCollider.whoIsEnter);
                //damageAreaCollider.whoIsExit.SendMessage("Damage", normalDamage);
                //damageAreaCollider.whoIsStay.SendMessage("Damage", normalDamage);
            }
            
        }

        public void GrabEnemy(bool grab)
        {
            if (grab && damageAreaCollider.whoIs != null)
            {
                //Debug.Log("Grabbed");
                damageAreaCollider.whoIs.SendMessage("Damage", grabDamage);
            }
            
        }
        
        IEnumerator WaitForAnimation(Animation animation)
        {
            Debug.Log("DebugghiIn");
            yield return new WaitForSeconds(animation["Attack"].length);
        }

    }
}
