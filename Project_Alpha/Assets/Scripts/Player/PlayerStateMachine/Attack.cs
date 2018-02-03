using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Attack : MonoBehaviour
{
    public ContactFilter2D contactFilter;
    public SpriteRenderer offenseStateSpriteRenderer;
    public GameObject playerLife;

    public int standardDamage = 20;
    public int healForDamage = 2;

    private Life life;
    [SerializeField] private GameObject damageAreaGameObject;
    private DamageAreaCollider damageAreaCollider;
    //private GlobalVariables globalVariables;
    //private Animator anim;

    private int i = 0;//counter variables

    public enum AttackType
    {
        normalAttack
    }

    private void Awake()
    {
        damageAreaCollider = damageAreaGameObject.GetComponent<DamageAreaCollider>();
        life = playerLife.GetComponent<Life>();
        //anim = GetComponent<Animator>();
    }

    private void Start()
    {
        //globalVariables = GameObject.Find("GameManager").GetComponent<GlobalVariables>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //see summary in PlayerStateMachine.cs
    public void DoAttack(int attack)
    {
        switch (attack)
        {
            case 0:
                NormalAttack();
                break;

            default:
                break;
        }
    }

    private void NormalAttack()
    {
        //Todo: Animation Start
        damageAreaCollider.CeckHit();

        i = 0;

        if (damageAreaCollider.enemyHitted != null)
        {
            if (damageAreaCollider.enemyHitted[0] != null)
            {
                foreach (Collider2D enemy in damageAreaCollider.enemyHitted)
                {

                    if (damageAreaCollider.enemyHitted[i] != null)
                    {
                        //Debug.Log(damageAreaCollider.enemyHitted[i].name + " " + damageAreaCollider.enemyHitted[i].gameObject.layer.ToString());
                        if (damageAreaCollider.enemyHitted[i].gameObject.CompareTag("Enemy"))
                        {
                            damageAreaCollider.enemyHitted[i].gameObject.SendMessage("Damage", standardDamage);
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
}

