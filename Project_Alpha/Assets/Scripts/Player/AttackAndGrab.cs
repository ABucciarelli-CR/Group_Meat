using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerMain))]
    public class AttackAndGrab : MonoBehaviour
    {

        public int normalDamage = 20;
        public int grabDamage = 5;


        //private bool enemyIsGrabbed = false;
        [SerializeField] private GameObject damageAreaGameObject;
        private DamageAreaCollider damageAreaCollider;
        private Animator anim;
        private int i = 0;

        //RaycastHit2D ray2D;


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
                //Debug.Log("DebugghiOut");
                WaitForAnimation(anim.GetComponent<Animation>());
                anim.SetBool("Attack", false);

                damageAreaCollider.CeckHit();

                if(damageAreaCollider.enemyHitted != null)
                {
                    if(damageAreaCollider.enemyHitted[0] != null)
                    {
                        foreach (Collider2D enemy in damageAreaCollider.enemyHitted)
                        {
                            //Debug.Log("Hitted" + i);
                            if (damageAreaCollider.enemyHitted[i] != null)
                            {
                                damageAreaCollider.enemyHitted[i].gameObject.SendMessage("Damage", normalDamage);
                                i++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        //Debug.Log("Exit");
                        i = 0;
                        System.Array.Clear(damageAreaCollider.enemyHitted, 0, damageAreaCollider.maxArrayEnemy);
                    }
                    
                }

                

            }
            /*
            if (attack && damageAreaCollider.whoIs != null)
            {

                //Debug.Log("Attacked");
                if(!damageAreaCollider.whoIs.CompareTag("Shield"))
                {
                    damageAreaCollider.whoIs.gameObject.SendMessage("Damage", normalDamage);
                }
                //Debug.Log(damageAreaCollider.whoIsEnter);
                //damageAreaCollider.whoIsExit.SendMessage("Damage", normalDamage);
                //damageAreaCollider.whoIsStay.SendMessage("Damage", normalDamage);
            }*/
            
        }
        /*
        public void GrabEnemy(bool grab)
        {
            if (grab && damageAreaCollider.whoIs != null)
            {
                //Debug.Log("Grabbed");
                damageAreaCollider.whoIs.SendMessage("Damage", grabDamage);
            }
            
        }*/
        
        IEnumerator WaitForAnimation(Animation animation)
        {
            Debug.Log("DebugghiIn");
            yield return new WaitForSeconds(animation["Attack"].length);
        }

    }
}
