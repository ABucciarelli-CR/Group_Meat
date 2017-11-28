using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerMain : MonoBehaviour
    {
        
        [HideInInspector] public bool facingRight = true;
        public bool airControl = false;

        public float moveForce = 10f;
        public float gravity = -10f;
        public float jumpForce = 1000f;
        public Transform groundCheck;
        public Transform ceilingCheck;


        [SerializeField] private bool isGrounded = false;
        private Animator anim;
        [SerializeField] private LayerMask whatIsGround;
        private Rigidbody2D rb2d;
        private Transform transform2d;
        private float groundRadiusCollision = .3f;
        private float ceilingRadiusCollision = .2f;

       
        void Awake()
        {
            anim = GetComponent<Animator>();
            rb2d = GetComponent<Rigidbody2D>();


        }
        
        void FixedUpdate()
        {
            isGrounded = false;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundRadiusCollision, whatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    isGrounded = true;
                }
            }
            anim.SetBool("Ground", isGrounded);
            // Set the vertical animation
            anim.SetFloat("vSpeed", rb2d.velocity.y);
        }

        public void Move(bool jump, float leftRightMove)
        {

            if (isGrounded && jump)
            {
                isGrounded = false;
                rb2d.AddForce(new Vector2(0f, jumpForce));

                anim.SetBool("Ground", false);
            }

            if (airControl || isGrounded)
            {
                anim.SetFloat("Speed", Mathf.Abs(leftRightMove));

                rb2d.velocity = new Vector2(leftRightMove * moveForce, rb2d.velocity.y);

            }

            if (leftRightMove < 0 && facingRight)
            {
                Flip();
            }
            else if (leftRightMove > 0 && !facingRight)
            {
                Flip();
            }
        }

        public void Flip()
        {
            Debug.Log("Do Flip");
            facingRight = !facingRight;
            Vector3 normalScale = transform.localScale;
            normalScale.x *= -1;
            transform.localScale = normalScale;
        }
    }
}
