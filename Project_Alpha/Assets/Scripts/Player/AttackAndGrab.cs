using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerMain))]
    [RequireComponent(typeof(Life))]
    public class AttackAndGrab : MonoBehaviour
    {

        public int normalDamage = 20;
        public int grabDamage = 5;
        public int heal = 50;


        //private bool enemyIsGrabbed = false;
        private Life life;
        [SerializeField] private GameObject damageAreaGameObject;
        public ContactFilter2D contactFilter;
        private DamageAreaCollider damageAreaCollider;
        public Collider2D eatCollider;
        private Animator anim;
        private int i = 0;

        private Collider2D[] enemyDeadHitted;
        private int maxEnemyDeadHittedArray = 100;

        //RaycastHit2D ray2D;


        void Awake()
        {
            life = GetComponent<Life>();
            enemyDeadHitted = new Collider2D[maxEnemyDeadHittedArray];
            anim = GetComponent<Animator>();
            //eatCollider = GetComponent<Collider2D>();
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
                                if(damageAreaCollider.enemyHitted[i].gameObject.CompareTag("Enemy"))
                                {
                                    damageAreaCollider.enemyHitted[i].gameObject.SendMessage("Damage", normalDamage);
                                }
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

        public void EatEnemy(bool eat)
        {
            if(eat)
            {
                //Debug.Log("Eating");
                eatCollider.OverlapCollider(contactFilter, enemyDeadHitted);
                foreach (Collider2D collider in enemyDeadHitted)
                {
                    if (enemyDeadHitted[i].CompareTag("Corpse"))
                    {
                        Destroy(enemyDeadHitted[i].gameObject);
                        life.Heal(heal);
                        //Debug.Log("Eated");
                    }
                    i++;
                }
            }
        }
        
        IEnumerator WaitForAnimation(Animation animation)
        {
            //Debug.Log("DebugghiIn");
            yield return new WaitForSeconds(animation["Attack"].length);
        }

    }
}
