using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(EnemyStateMachine))]
    public class EnemyStateMachine : MonoBehaviour
    {

        [HideInInspector] public EnemyState enemyState;


        public float speed = .01f;
        public int damage;
        public float attackDelay;
        public ContactFilter2D contactFilter;
        [HideInInspector] public Vector2 movement;
        [HideInInspector] public float direction = 1;
        [HideInInspector] public float timeToChangeDirection = 5f;
        [HideInInspector] public float ttcd;
        /*[HideInInspector]*/
        public Collider2D[] hitColliders;
        [HideInInspector] public int maxArray = 100;
        public LayerMask deadLayer;

        private EnemyHealth enemyHealth;
        


        public enum EnemyState
        {
            idle,
            attack,
            searchPlayer,
            stun
        }

        void Awake()
        {
            
        }

        void Start()
        {
            deadLayer = (LayerMask.NameToLayer("corpse"));
            enemyHealth = GetComponent<EnemyHealth>();
            enemyState = EnemyState.idle;
            //Debug.Log(deadLayer.value);
        }

        void Update()
        {
            if (enemyHealth.health <= 0)
            {
                enemyState = EnemyState.stun;
            }

            switch (enemyState)
            {
                case EnemyState.idle:
                    Idle();
                    break;

                case EnemyState.attack:
                    Attack();
                    break;

                case EnemyState.searchPlayer:
                    SearchPlayer();
                    break;

                case EnemyState.stun:
                    Stun();
                    break;

                default:
                    break;
            }
        }


        public virtual void Idle()
        { }

        public virtual void Attack()
        { }

        public virtual void SearchPlayer()
        { }

        public virtual void Stun()
        {
            gameObject.tag = "Corpse";
            gameObject.layer = deadLayer;
        }

    }
}

