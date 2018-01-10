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
        private int heal = 50;
        private int healForDamage = 2;
        private int waitFrame = 4;
        private int waitedFrame = 0;

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

        private GlobalVariables globalVariables;

        public SpriteRenderer offenseStateSpriteRenderer;

        private Color playerOffenseStateStandardColor;
        private Color playerOffenseStateAttackColor;

        //RaycastHit2D ray2D;


        void Awake()
        {
            life = GetComponent<Life>();
            enemyDeadHitted = new Collider2D[maxEnemyDeadHittedArray];
            anim = GetComponent<Animator>();

            playerOffenseStateStandardColor = Color.white;
            playerOffenseStateAttackColor = Color.red;
            //eatCollider = GetComponent<Collider2D>();

            damageAreaCollider = damageAreaGameObject.GetComponent<DamageAreaCollider>();
        }

        private void Update()
        {
            if(offenseStateSpriteRenderer.color == playerOffenseStateAttackColor)
            {
                waitedFrame++;
            }
        }

        void Start()
        {
            globalVariables = GameObject.Find("GameManager").GetComponent<GlobalVariables>();
        }


        public void AttackEnemy(bool attack)
        {
            if(attack)
            {
                offenseStateSpriteRenderer.color = playerOffenseStateAttackColor;

                anim.SetBool("Attack", true);
                //Debug.Log("DebugghiOut");
                WaitForAnimation(anim.GetComponent<Animation>());
                anim.SetBool("Attack", false);

                damageAreaCollider.CeckHit();

                i = 0;

                if (damageAreaCollider.enemyHitted != null)
                {
                    if(damageAreaCollider.enemyHitted[0] != null)
                    {
                        foreach (Collider2D enemy in damageAreaCollider.enemyHitted)
                        {
                            
                            if (damageAreaCollider.enemyHitted[i] != null)
                            {
                                //Debug.Log(damageAreaCollider.enemyHitted[i].name + " " + damageAreaCollider.enemyHitted[i].gameObject.layer.ToString());
                                if (damageAreaCollider.enemyHitted[i].gameObject.CompareTag("Enemy"))
                                {
                                    damageAreaCollider.enemyHitted[i].gameObject.SendMessage("Damage", normalDamage);
                                    life.Heal(healForDamage);
                                }
                                i++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        //Debug.Log("Exit");
                        
                        System.Array.Clear(damageAreaCollider.enemyHitted, 0, damageAreaCollider.maxArrayEnemy);
                    }
                }
            }
            else
            {
                if(waitedFrame >= waitFrame)
                {
                    waitedFrame = 0;
                    offenseStateSpriteRenderer.color = playerOffenseStateStandardColor;
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
                i = 0;
                foreach (Collider2D collider in enemyDeadHitted)
                {
                    if(enemyDeadHitted[i] != null)
                    {
                        if (enemyDeadHitted[i].CompareTag("Corpse"))
                        {
                            Destroy(enemyDeadHitted[i].gameObject);
                            life.Heal(heal);
                            globalVariables.enemyDead++;
                            //Debug.Log(globalVariables.enemyDead);
                        }
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
