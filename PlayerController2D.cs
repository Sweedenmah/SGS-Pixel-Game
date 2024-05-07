/*
##########################################################################
Please Note:
    - All code made by sgs_Justin and sgs_Jake
    - Code will not compile in unity if animations within argments [such as
    "animator.Play(<<ANIMATION NAME>>)"] do not exist
##########################################################################
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using System.Timers;

public class PlayerController2D : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;

    //attack variabless
    bool isAttacking = false;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public int attackDamage = 40;
    public float attackRange = 0.5f;

    public int noOfClicks = 0;
    float lastClickedTime = 0;
    float maxComboDelay = 0.5f;
     
    bool jumpAttack;

    //jump variables
    bool isGrounded;
    bool candoublejump;
    bool singlejump;
    bool isWalking;
  

    //cooldown variables
    public float cooldownTime = 0.40f;

    private float nextFireTime = 0f;

    [SerializeField]
    Transform groundCheck;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        isAttacking = false;
        noOfClicks = 0;
   
    }


    private void Update()
    {
        //initial input 
        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            isAttacking = true;
            jumpAttack = true;
            lastClickedTime = Time.time; //counts clicks for combo 
            noOfClicks++;

            if (noOfClicks == 1 && isGrounded)
            {
                animator.Play("Player_down_slash");
                SoundManagerScript.PlaySound("playerSlash1");
            }
            noOfClicks = Mathf.Clamp(noOfClicks, 0, 3); //cap clicks at 3

            if (jumpAttack && !isGrounded)
            {
                animator.Play("Player_air_attack1");
                SoundManagerScript.PlaySound("playerSlash3");
            }

            //enemy hitbox + damage 
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            }

            //reset attack timer
            Invoke("ResetAttack", .32f);

            if (noOfClicks >= 2 && isGrounded)
            {
                animator.Play("Player_up_slash");
                SoundManagerScript.PlaySound("playerSlash2");
            }

            if (noOfClicks >= 2 && jumpAttack)
            {
                animator.Play("Player_air_attack2");
                SoundManagerScript.PlaySound("playerSlash1");
            }



            Invoke("ResetAttack", .32f);

            if (noOfClicks >= 3 && isGrounded)
            {
                animator.Play("Player_spin_slash");
                SoundManagerScript.PlaySound("playerSlash3");
                noOfClicks = 0;

            }

            Invoke("ResetAttack", .32f);
        }
    }
    void ResetAttack()
    {
        isAttacking = false;
        jumpAttack = false;
    }

    private void FixedUpdate()
    {
        if (Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground")))  //Classification of Grounded
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }


        if (Input.GetKey("d") || Input.GetKey("right"))      //Walk right
        {
            isWalking = true;
            rb2d.velocity = new Vector2(6, rb2d.velocity.y);

            if (isGrounded && !isAttacking)
                animator.Play("Player_run");

            transform.localScale = new Vector3(5, 5, 5);


        }
        else if (Input.GetKey("a") || Input.GetKey("left")) //Walk left
        {
            isWalking = true;
            rb2d.velocity = new Vector2(-6, rb2d.velocity.y);

            if (isGrounded && !isAttacking)
                animator.Play("Player_run");

            transform.localScale = new Vector3(-5, 5, 5);

        }
        else if (isGrounded) //Idle anim
        {
            if (!isAttacking)
            {
                animator.Play("Player_idle");
            }

            isWalking = false;
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }

        if (isGrounded && Input.GetKey(KeyCode.LeftShift) && Input.GetKey("a") || isGrounded && Input.GetKey(KeyCode.LeftShift) && Input.GetKey("left")) //Sprint left
        {
            rb2d.velocity = new Vector2(-9, rb2d.velocity.y);
            isWalking = true;

            if (isGrounded && !isAttacking)
                animator.Play("Player_run");

            transform.localScale = new Vector3(-5, 5, 5);

        }

        if (isGrounded && Input.GetKey(KeyCode.LeftShift) && Input.GetKey("d") || isGrounded && Input.GetKey(KeyCode.LeftShift) && Input.GetKey("right")) //Sprint right
        {
            rb2d.velocity = new Vector2(9, rb2d.velocity.y);
            isWalking = true;

            if (isGrounded && !isAttacking)
                animator.Play("Player_run");

            transform.localScale = new Vector3(5, 5, 5);

        }
        //For faster jump update (Every Frame)
        if (Input.GetKey("space") && isGrounded && !isAttacking || Input.GetKey("up") && isGrounded && !isAttacking || Input.GetKey("w") && isGrounded && !isAttacking)        //Single Jump
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 7);
            nextFireTime = Time.time + cooldownTime;
            singlejump = true;
            animator.Play("Player_jump");
   
            
        }

        if (Input.GetKey("space") && candoublejump && Time.time > nextFireTime && !isAttacking || Input.GetKey("up") && candoublejump && Time.time > nextFireTime && !isAttacking || Input.GetKey("w") && candoublejump && Time.time > nextFireTime && !isAttacking) //Double Jump
        {
            singlejump = false;
            rb2d.velocity = new Vector2(rb2d.velocity.x, 8);
            animator.Play("Player_doublejump");
            candoublejump = false;
        }

        if (isGrounded && !isAttacking)
        {
            candoublejump = true;
            singlejump = false;
        }

    }

}

 