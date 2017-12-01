using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Enemy
{
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
        /*[HideInInspector]*/ public Collider2D[] hitColliders;
        [HideInInspector] public int maxArray = 100;


        public enum EnemyState
        {
            idle,
            attack,
            searchPlayer
        }

        void Awake()
        {
            
        }

        void Start()
        {
            enemyState = EnemyState.idle;
        }

        void Update()
        {

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

    }
}
