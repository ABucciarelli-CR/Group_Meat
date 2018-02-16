using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Attack : MonoBehaviour
{
    [Title("ReadOnly, modifiche disabilitate.")]
    [ReadOnly]
    public ContactFilter2D contactFilter;
    [ReadOnly]
    public SpriteRenderer offenseStateSpriteRenderer;
    [ReadOnly]
    public GameObject playerLife;
    [ReadOnly]
    [SerializeField] private GameObject damageAreaGameObject;
    [ReadOnly]
    [SerializeField] private GameObject frenzyAnimation;
    [ReadOnly]
    public bool frenzyActive = false;
    [ReadOnly]
    public bool abilityEnded = false;

    [Title("Modifiche abilitate.")]
    [InfoBox("Valori standard")]
    public int standardDamage = 20;
    public int healForDamage = 2;

    [Title("Valori della frenzy.")]
    [InfoBox("Consumo vita al secondo")]
    public float maxHealthConsume = 5f;
    [InfoBox("Durata(sec)")]
    [MinValue(0)]
    public float maxFrenzyTime = 5f;

    [Title("Modificatori frenzy.")]
    [InfoBox("valore minimo 1, il moltiplicatore funziona secondo la seguente formula: attacco * moltiplicatore")]
    [MinValue(1)]
    public float standardAttackModifier = 1.5f;
    [MinValue(1)]
    public float standardHealthModifier = 1.5f;

    private Life life;
    private DamageAreaCollider damageAreaCollider;
    private GameObject frenziPlaceholder;
    private float damageDeltaTime = 0;
    //private GlobalVariables globalVariables;
    //private Animator anim;

    private int i = 0;//counter variables

    public enum AttackType
    {
        normalAttack,
        frenzy
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
        //disattivo la frenzy se è passato il tempo
        if(abilityEnded && frenzyActive)
        {
            frenzyActive = false;
            abilityEnded = false;
        }

        if(frenzyActive)
        {
            damageDeltaTime += (maxHealthConsume * Time.deltaTime);
            if(damageDeltaTime >= 1)
            {
                playerLife.SendMessage("DecrementLife", 1);
                damageDeltaTime = 0;
            }
            
        }
    }

    //see summary in PlayerStateMachine.cs
    public void DoAttack(int attack)
    {
        switch (attack)
        {
            case 0:
                NormalAttack();
                break;

            case 1:
                FrenzyState();
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
                            if(frenzyActive)
                            {
                                damageAreaCollider.enemyHitted[i].gameObject.SendMessage("Damage", (int)(standardDamage * standardAttackModifier));
                                life.Heal((int)(healForDamage * standardHealthModifier));
                            }
                            else
                            {
                                damageAreaCollider.enemyHitted[i].gameObject.SendMessage("Damage", standardDamage);
                                life.Heal(healForDamage);
                            }
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

    private void FrenzyState()
    {
        if(!frenzyActive)
        {
            frenziPlaceholder = Instantiate(frenzyAnimation, this.transform.position + new Vector3(0,30,0), Quaternion.identity, this.gameObject.transform);
            frenziPlaceholder.gameObject.transform.localScale = new Vector3(10, 25, 0);
            frenziPlaceholder.GetComponent<SpriteRenderer>().color = Color.red;
            //int[] startAndEnd = new int[2];
            //startAndEnd[0] = 4;
            //startAndEnd[0] = 10;
            frenziPlaceholder.SendMessage("Loop"/*, startAndEnd*/);
            
            frenzyActive = true;
            StartCoroutine(EndAbility(maxFrenzyTime));
        }
    }

    IEnumerator EndAbility(float sec)
    {
        yield return new WaitForSeconds(sec);
        Destroy(frenziPlaceholder);
        abilityEnded = true;
    }
}

