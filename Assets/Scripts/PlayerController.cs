using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


//Animation States
public enum AnimationStates { Idle, Walking, Jumping, Falling,  Dashing }
public class PlayerController : MonoBehaviour
{
    //Singleton
    public static PlayerController S;

    //Public Variables
    [Header("Player Basic Property Values")]
    public float speed;
    public float jumpStrength;
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer sprite;
    public bool isGrappleActivated = false;
    private bool doubleJump = false;
    public bool isFacingRight = true;
    public AnimationStates animState = AnimationStates.Idle;

    [Header("Player Attack Property Values")]
    public float attackXOffset;
    public float attackYOffset;
    public float attackRadius;
    public bool isAttacking = false;
    public LayerMask attackLayer;

    [Header("Player Wall Property Values")]
    public float wallSlideSpeed;
    public Transform wallHitbox;
    public LayerMask wallLayer;
    private float wallJumpDirection;
    private float wallJumpLongevity;
    private float wallJumpCount;
    private float wallJumpTotalTime;
    private bool isCurrentlyWallSliding = false;
    private bool isCurrentlyWallJumping = false;
    private Vector2 wallJumpPower = new Vector2(6f, 8f);

    [Header("Grappling Data Values")]
    public LayerMask grappleLayer;
    public float maxDistance = 10f;
    public float grappleSpeed = 10f;
    private bool IsGrappleModeActive = false;
    private bool IsLatchedOn = false;
    public LineRenderer lineRen;
    private List<Vector2> points = new List<Vector2>();
    public Camera cam;

    [Header("Player Dash Property Values")]
    public float dashPower;
    public float dashTimer;
    public float dashCooldown;
    public const float setTimeToDash = 0.55f;
    private float playerPressedTime = 0.0f;
    private bool resetDashVariables = false;
    private bool isCurrentlyDashing = false;
    private bool DashEnabled = true;
    
    [Header("Player Stun State Values")]
    public float knockback;
    public float hitStunTimer;
    public float horizontalMovement;
    public bool rightDirectionKnockback = false;
    public bool enemyHitConfirm = false;

    [Header("Ground Functionality Values")]
    public Transform groundHitbox;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0f;
    public float groundCheckYOffset = 0f;

    [Header("Input Essentials")]
    private bool hasPressed = false;

   

    
   
    

    private void Awake()
    {
        if (S)
        {
            Destroy(this.gameObject);
        }
        else
        {
            S = this;
        }
    }
    // Start is called before the first frame update
    private void Start()
    {
        //Player Basic Essentials
        speed = 200.0f;
        jumpStrength = 20f;

        ////Attack Essentials
        //attackRadius = 0f;
        //attackXOffset = 0f;
        //attackYOffset = 0f;

        ////Dashing Essentials
        dashPower = 30f;
        dashTimer = 0.5f;
        dashCooldown = 0.01f;

        ////Grappling Hook Essentials
        //maxDistance = 10f;
        //grappleSpeed = 10f;


        //Wall Jump Essentials
        wallSlideSpeed = 10f;
        wallJumpPower = new Vector2(6f, 8f);
        wallJumpLongevity = 0.2f;
        wallJumpTotalTime = 0.1f;
        isCurrentlyWallSliding = false;
        isCurrentlyWallJumping = false;

        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        //Is the player currently dashing?
        if (isCurrentlyDashing)
        {
            return;
        }
        //Check if the player is not recovering from hitstun
        if (!enemyHitConfirm && hitStunTimer <= 0 && !isCurrentlyWallJumping && !isGrappleActivated && !isCurrentlyWallSliding)
        {
            //Move left and right
            horizontalMovement = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(horizontalMovement * speed, rb.velocity.y);
        }

        //Call the animator and set the proper animations
        //Set the new condition for the jump animation
        animator.SetBool("IsGrounded", hasLanded());

        //Set the jump velocity to activate the falling animation
        animator.SetFloat("VerticalSpeed", rb.velocity.y);

        //Set the respective value for movement animation
        animator.SetFloat("HorizontalSpeed", Mathf.Abs(horizontalMovement));

        //Set wall sliding animation
        animator.SetBool("IsWallSliding", isCurrentlyWallSliding);

        //Set wall jumping animation
        animator.SetBool("IsWallJumping", isCurrentlyWallJumping);

        //Set the player dying Amimation
        animator.SetBool("HasDied", Player.S.HasDied);

        //Set the hitstun animation
        animator.SetBool("HitStunned", enemyHitConfirm);




        //Double Jump Ability
        if (hasLanded() && !Input.GetKey(KeyCode.UpArrow))
        {
            doubleJump = false;

        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && !isGrappleActivated)
        {

            //animator.SetTrigger("TakingOff");
            if (hasLanded() || doubleJump || isCurrentlyWallJumping || isCurrentlyWallSliding)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
                doubleJump = !doubleJump;
            }

        }

        //Jumping Ability
        if (Input.GetKeyDown(KeyCode.UpArrow) && rb.velocity.y > 0.0f && !isGrappleActivated)
        {

            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (!isGrappleActivated)
        {
            //Dashing Ability--Right Direction
            CheckDashRight();
            //Dashing Ability--Left Direction
            CheckDashLeft();
        }
        
        //Reset Dashing Variables
        if (resetDashVariables)
        {
            hasPressed = false;
            resetDashVariables = false;
        }

        //Is character wall sliding?
        WallSliding();
        //Is character wall jumping?
        WallJumping();

        if (!isCurrentlyWallJumping)
        {
            //Is character facing the correct direction
            FlipDirection();
        }


       
       

    }

    private void FixedUpdate()
    {

        //if (DialogueManagerScript.instance.IsDialoguePlaying)
        //{
        //    return;
        //}


        //If Player has taken damage
        if (hitStunTimer > 0 && enemyHitConfirm)
        {
            if (rightDirectionKnockback)
            {
                rb.velocity = new Vector2(-knockback, knockback);
            }
            else if (!rightDirectionKnockback)
            {
                rb.velocity = new Vector2(knockback, knockback);
            }
            hitStunTimer -= Time.deltaTime;
            if (hitStunTimer <= 0)
            {
                enemyHitConfirm = false;
            }
        }

        if(Mathf.Abs(horizontalMovement) > 0.1f && !DialogueManagerScript.instance.IsDialoguePlaying)
        {
            rb.AddForce(new Vector2(horizontalMovement * speed, 0f),ForceMode2D.Force);
        }
    }

    
    private void CheckDashRight()
    {        
        if ((Input.GetKeyDown(KeyCode.RightArrow) && isFacingRight) && hasPressed && !Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (Time.time - playerPressedTime < setTimeToDash)
            {
                StartCoroutine(DashAbility());
            }
            else
            {
                Debug.Log("Too Slow");
            }
            //Reset Variables
            resetDashVariables = true;

        }


        if ((Input.GetKeyDown(KeyCode.RightArrow) && isFacingRight) && !hasPressed)
        {
            hasPressed = true;
            playerPressedTime = Time.time;
        }

    }

    private void CheckDashLeft()
    {
        //Dashing Ability--Right Direction
        if ((Input.GetKeyDown(KeyCode.LeftArrow) && !isFacingRight) && hasPressed && !Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Time.time - playerPressedTime < setTimeToDash)
            {
                StartCoroutine(DashAbility());
            }
            else
            {
                Debug.Log("Too Slow");
            }
            //Reset Variables
            resetDashVariables = true;

        }


        if ((Input.GetKeyDown(KeyCode.LeftArrow) && !isFacingRight) && !hasPressed)
        {
            hasPressed = true;
            playerPressedTime = Time.time;
        }
    }

    //Check if player is looking in the correct direction
    private void FlipDirection()
    {
        if(((isFacingRight && horizontalMovement  < 0) || (!isFacingRight && horizontalMovement > 0)) && !isCurrentlyWallJumping)
        {
            
            isFacingRight = !isFacingRight;
            Vector3 tempScale = transform.localScale;
            tempScale.x *= (-1.0f);
            transform.localScale = tempScale;
        }
    }

    //Is player on the ground?
    public bool hasLanded()
    {
        //animator.SetBool("IsJumping", false);
        return Physics2D.OverlapCircle(groundHitbox.position, 0.1f, groundLayer);
    }

    //Is player on the wall?
    public bool LatchedOntoWall()
    {
        return Physics2D.OverlapCircle(wallHitbox.position, 0.2f, groundLayer) && !hasLanded();
    }

    //Wall Sliding Mechanic
    private void WallSliding()
    {
        if(LatchedOntoWall() && !hasLanded() && horizontalMovement != 0)
        {
           
            isCurrentlyWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
            //rb.AddForce(new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue)),ForceMode2D.Force);
        }
        else
        {
            isCurrentlyWallSliding = false;
            
        }
    }

    //Wall Jumping Mechanic
    private void WallJumping()
    {
        if (isCurrentlyWallSliding)
        {
            
            isCurrentlyWallJumping = false;
            wallJumpDirection = transform.localScale.x;
            wallJumpCount = wallJumpLongevity;
            CancelInvoke(nameof(StopWallJump));
        }
        else
        {
            wallJumpCount -= Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.UpArrow) && wallJumpCount > 0f)
        {
            
            isCurrentlyWallJumping = true;
            //Set the respective value for wall jump 
            animator.SetTrigger("WallJump");
            rb.velocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
            wallJumpCount = 0f;

            if (transform.localScale.x != wallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 tempLocalScale = transform.localScale;
                tempLocalScale.x *= -1f;
                transform.localScale = tempLocalScale;
            } 
            Invoke(nameof(StopWallJump), wallJumpTotalTime);
        }
    }

    //Cease Wall Jumping
    private void StopWallJump()
    {
        isCurrentlyWallJumping = false;
    }

    //Dashing Ability
    private IEnumerator DashAbility()
    {
        animator.SetBool("HasStoppedDashing", false);
        DashEnabled = false;
        isCurrentlyDashing = true;
        float tempMass = rb.mass;
        float tempGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.mass = 0.5f;
        rb.velocity = new Vector2(transform.localScale.x * dashPower, 0f);
        rb.AddRelativeForce(new Vector2(transform.localScale.x * dashPower, 0f), ForceMode2D.Force);


        yield return new WaitForSeconds(dashTimer);
        rb.gravityScale = tempGravity;
        rb.mass = tempMass;
        isCurrentlyDashing = false;
        animator.SetBool("HasStoppedDashing",true);
        yield return new WaitForSeconds(dashCooldown);
        DashEnabled = true;
    }

   
}
