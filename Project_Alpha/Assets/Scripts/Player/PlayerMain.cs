using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Tween))]
    public class PlayerMain : MonoBehaviour
    {
        
        [HideInInspector] public bool facingRight = true;
        public bool airControl = false;

        public float moveForce = 10f;
        public float gravity = -10f;
        public float jumpForce = 1000f;
        public float dashTime = .01f;
        private float dashSpeed = 5f;
        
        [SerializeField] private bool isGrounded = false;
        private float currentDashTime;
        private float lastMove;

        private bool dashing = false;

        private bool avoidJumpAfterDash = false;
        private float avoidJumpTime = .5f;
        private float avoidedJumpForTime = 0;

        private float groundRadiusCollision = .3f;
        //private float ceilingRadiusCollision = .2f;

        [SerializeField] private LayerMask whatIsGround;

        private Animator anim;

        private Rigidbody2D rb2d;

        private Transform transform2d;
        public Transform groundCheck;
        public Transform ceilingCheck;

        private Tween tween;


        void Awake()
        {
            anim = GetComponent<Animator>();
            rb2d = GetComponent<Rigidbody2D>();

            tween = GetComponent<Tween>();
            
        }

        private void Start()
        {
            currentDashTime = dashTime;
        }

        private void Update()
        {
            /*
            if(avoidJumpAfterDash)
            {
                avoidedJumpForTime += Time.deltaTime;
                if(avoidedJumpForTime >= avoidJumpTime)
                {
                    avoidedJumpForTime = 0;
                    avoidJumpAfterDash = false;
                    rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
                }
            }*/
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


        public void Move(bool jump, float leftRightMove, bool dash)
        {

            if(leftRightMove != 0)
            {
                lastMove = leftRightMove;
            }

            if (dash)
            {
                //rb2d.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                currentDashTime = 0f;
                dashing = true;
                //Debug.Log("Dashing");
            }

            if (currentDashTime < dashTime && dashing)
            {
                gameObject.layer = 13;
                //Debug.Log(Mathf.Sign(lastMove) * dashSpeed);

                if (isGrounded || !isGrounded)
                {
                    //rb2d.AddForce(new Vector2(Mathf.Sign(lastMove) * dashSpeed, 0));
                    //transform.Translate(- Mathf.Sign(lastMove) * Vector2.left * 5);

                    rb2d.MovePosition(rb2d.position + new Vector2(Mathf.Sign(lastMove) * dashSpeed, 0));
                }
                else
                {
                    //rb2d.AddForce(new Vector2(Mathf.Sign(lastMove) * dashSpeed, 0));
                    //transform2d.Translate(new Vector2(Mathf.Sign(lastMove) * dashSpeed + transform2d.position.x, transform2d.position.y));
                }


                //tween.Move(Mathf.Sign(lastMove));
                currentDashTime += Time.deltaTime;
            }
            else if( currentDashTime >= dashTime && dashing)
            {
                dashing = false;
                rb2d.velocity = new Vector2(0, 0);
                avoidJumpAfterDash = true;
                gameObject.layer = 8;
            }
            


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
            //Debug.Log("Do Flip");
            facingRight = !facingRight;
            Vector3 normalScale = transform.localScale;
            normalScale.x *= -1;
            transform.localScale = normalScale;
        }
    }
}
