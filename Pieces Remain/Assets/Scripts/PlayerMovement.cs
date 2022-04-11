using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform feetPos;
    public LayerMask whatIsGround;

    private Animator anim;
    private HashIDs hash;

    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private Transform GroundCheck;

    public float speedDampTime = 0.01f;
    public float checkRadius;
    public float jumpForce;
    public float speed;
    public float dashSpeed;
    public float startDashTime;

    private bool facingRight = true;
    private bool dashAvailability = true;
    private float moveInput;
    private float dashTime;
    private int direction;

    
    const float GroundedRadius = 0.1f;
    //private bool grounded;
    bool jump = false;
    bool isGrounded;
    bool isColliding;

    void Start()
    {
        dashTime = startDashTime;
    }


    private void Awake()
    {
        anim = GetComponent<Animator> ();
        hash = GameObject.FindGameObjectWithTag("GameController").GetComponent<HashIDs>();

    }

    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if (isGrounded == true && Input.GetButtonDown("Jump"))
        {
            anim.SetBool("Jump", true);

            rb.velocity = Vector2.up * jumpForce;
        }



        if(direction == 0)
        {
            if(moveInput < 0 && Input.GetButtonDown("Dash"))
            {
                print("left");
                direction = 1;
            }

            else if(moveInput > 0 && Input.GetButtonDown("Dash"))
            {
                print("right");
                direction = 2;
            }
        }

        else
        {
            if(dashTime <= 0)
            {
                direction = 0;
                dashTime = startDashTime;
                rb.velocity = Vector2.zero;
                dashAvailability = true;
            }

            else
            {
                dashTime -= Time.deltaTime;

                if(direction == 1)
                {
                    dashAvailability = false;
                    print("dashLeft");
                    rb.velocity = new Vector2(dashSpeed * -10f, 0f);
                }

                else if(direction == 2)
                {
                    dashAvailability = false;
                    print("dashRight");
                    rb.velocity = new Vector2(dashSpeed * 10f, 0f);
                }
            }
        }
        
        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    private void FixedUpdate()
    {
        //bool wasGrounded = grounded;

        float h = Input.GetAxis("Horizontal");
        
        MovementManager(h);

        isGrounded = false;

        isColliding = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        if(isColliding == true)
        {
            isGrounded = true;
            anim.SetBool("Jump", !isGrounded);
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheck.position, GroundedRadius, WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i].gameObject != gameObject)
            {
                anim.SetBool("Jump", true);
            }

            else
            {
                anim.SetBool("Jump", true);
            }
        }
    }

    void MovementManager(float horizontal)
    {
        if(dashAvailability == true)
        {
            moveInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

            if(horizontal > 0)
            {
                anim.SetFloat(hash.speedFloat, 1.0f, speedDampTime, Time.deltaTime);
            }

            else if(horizontal < 0)
            {
                anim.SetFloat(hash.speedFloat, 1.0f, speedDampTime, Time.deltaTime);
            }

            else
            {
                anim.SetFloat(hash.speedFloat, 0);
            }


            //if(horizontal > 0)
            //{
            //    anim.SetFloat(hash.speedFloat, 1.0f, speedDampTime, Time.deltaTime);

            //    Rigidbody2D ourBody = this.GetComponent<Rigidbody2D>();
            //   Vector2 right = new Vector2 (5f, 0.0f);
            //    ourBody.velocity = right;           
            //}

            //else if(horizontal < 0)
            //{
            //    anim.SetFloat(hash.speedFloat, 1.0f, speedDampTime, Time.deltaTime);

            //    Rigidbody2D ourBody = this.GetComponent<Rigidbody2D>();
            //    Vector2 left = new Vector2 (-5f, 0.0f);
            //    ourBody.velocity = left;
            //}

            //else
            //{
            //    anim.SetFloat(hash.speedFloat, 0);
            //}

            if(horizontal > 0 && !facingRight)
            {
                Flip();
            }

            if(horizontal < 0 && facingRight)
            {
                Flip();
            }
        

            //void JumpManager(bool jump)
            //{
            //    if(jump)
            //    {
            //        Rigidbody2D ourBody = this.GetComponent<Rigidbody2D>();
            //        Vector2 up = new Vector2 (0f, 10f);
            //        ourBody.velocity = up;
            //    }
            //}


        }
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
