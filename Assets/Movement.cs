using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // rigidbody
    public Rigidbody rb;

    // camera
    public Transform cam;

    // grounded variables
    public bool isGrounded = false;
    public Transform groundCheckPos;
    public LayerMask whatIsGround;
    public float groundCheckRadius = 0.25f;

    // movement variables
    public Vector3 direction;
    Vector3 moveDir;
    public float speed = 0f;
    public float maxSpeed;
    public float airSpeed = 200f;
    public float speedUpTime = 0.5f;

    // smoothing
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    // gravity variables
    bool useGroundedGravity = true;
    float gravity = -9.8f;
    float groundedGravity = -0.05f;

    // jump variables
    public float jumpForce = 5f;
    public bool isJumping = false;
    //bool isJumpPressed = false;
    float initialJumpVelocity;
    public float maxJumpHeight = 1.0f;
    public float maxJumpTime = 0.5f;
    public float jumpTimer;
    float jumpTimerReset;
    public float doubleJumpTimer = 0.25f;
    public bool startDoubleJump = false;
    public bool hasDoubleJumped = false;
    float doubleJumpTimerReset;
    public bool isDoubleJumping;

    // floating variables
    public bool isFloating = false;
    public GameObject balloon;
    public float floatTime = 3f;
    float floatTimeReset;
    bool fallingFromFloat = false;

    // slamming
    bool isStartingSlam = false;
    bool isSlamming = false;
    public float slamSpeed = 5f;

    // animator
    public Animator anim;

    // hit reaction
    public bool hitReaction = false;
    public bool hitRecover = false;
    public float hitForce = 3f;
    public float hitTimer;
    float hitTimerReset;

    // slide
    public bool isSliding = false;
    public float InitialSlideSpeed = 5f;
    public float slideSlowSpeed = 0.1f;
    public float slideSpeedThreshold = 0.5f;


    private void Start()
    {
        maxSpeed = speed;
        jumpTimerReset = jumpTimer;
        doubleJumpTimerReset = doubleJumpTimer;
        floatTimeReset = floatTime;
        hitTimerReset = hitTimer;
        SetupJumpVariables();
    }

    void SetupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;

        //jumpTimer = timeToApex;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        GroundCheck();
        HandleHitReaction();
        if (direction.magnitude >= 0.5f && Input.GetAxisRaw("Attack") >= 0.5f && !isStartingSlam && !startDoubleJump && !isSlamming && !hitReaction && !isSliding || direction.magnitude >= 0.5f && Input.GetButtonDown("Fire2") && !isStartingSlam && !startDoubleJump && !isSlamming && !hitReaction && !isSliding)
        {
            anim.SetBool("SlideStop", false);
            anim.SetTrigger("Slide");
            isSliding = true;
            rb.velocity = (moveDir + new Vector3(0, 1f, 0)) * InitialSlideSpeed;
        }

        if (rb.velocity.magnitude <= slideSpeedThreshold && isSliding)
        {
            anim.SetBool("SlideStop", true);
            isSliding = false;
        }
        HandleSlam();
        HandleJumping();
        HandleGravity();
    }

    private void HandleInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontal, 0f, vertical).normalized;
    }

    private void HandleHitReaction()
    {
        if (hitRecover)
        {
            anim.SetBool("Recover", true);
        }
        else
        {
            anim.SetBool("Recover", false);
        }
        if (hitReaction)
        {
            hitTimer -= Time.deltaTime;
            direction = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        if (isGrounded && isSliding)
        {
            rb.velocity = rb.velocity * slideSlowSpeed;
        }
        HandleMovement();
    }

    private void HandleSlam()
    {
        if (isJumping && Input.GetAxisRaw("Attack") >= 0.5f && !isStartingSlam && !startDoubleJump && !isSlamming && !isSliding || isJumping && Input.GetButtonDown("Fire2") && !isStartingSlam && !startDoubleJump && !isSlamming && !isSliding)
        {
            anim.SetTrigger("Slam");
            isStartingSlam = true;
        }
    }

    private void HandleJumping()
    {
        if (!isJumping && isGrounded && Input.GetButtonDown("Jump"))
        {
            //rb.AddRelativeForce(Vector3.up * jumpForce);
            isJumping = true;
            isSliding = false;
            useGroundedGravity = false;
            anim.SetTrigger("Jump");
            anim.SetBool("Jumping", true);
            rb.velocity = new Vector3(rb.velocity.x, initialJumpVelocity * 0.5f, rb.velocity.z);
        }
        if (isJumping)
        {
            jumpTimer -= Time.deltaTime;
        }

        // double jump
        HandleDoubleJump();

        HandleFloating();
    }

    private void HandleDoubleJump()
    {
        if (isJumping && Input.GetButtonDown("Jump") && jumpTimer <= 0.38f && !startDoubleJump && !isStartingSlam && !isSlamming && !hasDoubleJumped)
        {
            startDoubleJump = true;
            hasDoubleJumped = true;
        }
        if (startDoubleJump)
        {
            doubleJumpTimer -= Time.deltaTime;
        }
        if (isJumping && doubleJumpTimer >= 0 && Input.GetButtonUp("Jump") && startDoubleJump)
        {
            anim.SetTrigger("DoubleJump");
            isDoubleJumping = true;
        }
    }

    private void HandleFloating()
    {
        // floating
        if (isJumping && jumpTimer <= 0 && Input.GetButton("Jump") && !isStartingSlam && !isSlamming && floatTime >= 0f && !fallingFromFloat && !hitReaction && !isSliding)
        {
            isFloating = true;
            anim.SetBool("Floating", true);
            floatTime -= Time.deltaTime;
        }
        else
        {
            isFloating = false;
            anim.SetBool("Floating", false);
            balloon.SetActive(false);
        }

        if (floatTime <= 0 && !fallingFromFloat)
        {
            anim.SetTrigger("Fall");
            fallingFromFloat = true;
        }
    }

    private void HandleGravity()
    {
        bool isFalling = rb.velocity.y <= 0.0f || !Input.GetButton("Jump");
        float fallMultiplier = 2.0f;

        if (useGroundedGravity)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + groundedGravity * Time.deltaTime, rb.velocity.z);
        }
        else if (isFalling && !isFloating && !isDoubleJumping && !isStartingSlam && !isSlamming)
        {
            float previousYVelocity = rb.velocity.y;
            float newYVelocity = rb.velocity.y + (gravity * fallMultiplier * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            rb.velocity = new Vector3(rb.velocity.x, nextYVelocity, rb.velocity.z);
        }
        else if (isFloating)
        {
            rb.velocity = new Vector3(rb.velocity.x, -1f, rb.velocity.z);
        }
        else if (isDoubleJumping)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        }
        else if (isStartingSlam)
        {
            rb.velocity = Vector3.zero;
        }
        else if (isSlamming)
        {
            rb.velocity = new Vector3(rb.velocity.x, -slamSpeed, rb.velocity.z);
        }
        else
        {
            float previousYVelocity = rb.velocity.y;
            float newYVelocity = rb.velocity.y + (gravity * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            rb.velocity = new Vector3(rb.velocity.x, nextYVelocity, rb.velocity.z);
        }
    }

    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheckPos.position, groundCheckRadius, whatIsGround);
        if (!isGrounded)
        {
            isJumping = true;
            useGroundedGravity = false;
        }
        if (isGrounded)
        {
            startDoubleJump = false;
            
            doubleJumpTimer = doubleJumpTimerReset;
            isStartingSlam = false;
            isSlamming = false;
            hasDoubleJumped = false;
            floatTime = floatTimeReset;
            fallingFromFloat = false;
            //hitRecover = true;
        }
        if (isGrounded && hitTimer <= 0)
        {
            hitRecover = true;
            hitTimer = hitTimerReset;
        }
        if (isGrounded && jumpTimer <= 0.4f)
        {
            isJumping = false;
            jumpTimer = jumpTimerReset;
            anim.SetBool("Jumping", false);
            useGroundedGravity = true;
        }
    }

    private void HandleMovement()
    {
        if (direction.magnitude >= 0.5f)
        {

            if (speed < maxSpeed)
            {
                speed += Time.fixedDeltaTime * speedUpTime;
            }
            else
            {
                speed = maxSpeed;
            }

            float angle;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            
            if (isJumping && !isSliding)
            {
                angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 1f);
            }
            else if (isFloating && isJumping && !isSliding)
            {
                angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            }
            else
            {
                angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            }

            if (!isSliding)
            {
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
            else
            {
                Vector3 velocityDirection = rb.velocity;
                velocityDirection.y = 0;
                transform.rotation = Quaternion.LookRotation(velocityDirection);
            }
            

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDir = moveDir.normalized;

            if (isJumping && !isSliding)
            {
                rb.AddForce(new Vector3(moveDir.x * airSpeed * Time.fixedDeltaTime, 0f, moveDir.z * airSpeed * Time.fixedDeltaTime));
            }
            else if (isSliding)
            {
                // don't apply directional velocity
            }
            else
            {
                rb.velocity = new Vector3(moveDir.x, 0f, moveDir.z).normalized * speed * Time.fixedDeltaTime;
                
            }
            anim.SetBool("Moving", true);
            //controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        else
        {
            speed = rb.velocity.magnitude;
            anim.SetBool("Moving", false);
            if (!isJumping && !hitReaction && !isSliding)
            {
                rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            }
            else if (hitReaction)
            {
                // don't apply velocity
            }
            
            //print(speed);
        }
    }

    public void DoubleJump()
    {
        rb.velocity = new Vector3(rb.velocity.x, initialJumpVelocity * 0.7f, rb.velocity.z);
        isDoubleJumping = false;
        startDoubleJump = false;
    }

    public void Slam()
    {
        isStartingSlam = false;
        isSlamming = true;
    }

    public void HitReaction()
    {
        hitReaction = true;
        hitRecover = false;
        anim.SetTrigger("Hit");
    }

    public void HitRecovery()
    {
        hitReaction = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = false;
            anim.SetBool("Jumping", false);
            useGroundedGravity = true;
        }
        */
        if (collision.gameObject.tag == "Enemy")
        {
            // Calculate Angle Between the collision point and the player
            Vector3 dir = collision.contacts[0].point - transform.position;
            // We then get the opposite (-Vector3) and normalize it
            dir = -dir.normalized;
            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the player
            HitReaction();
            //rb.velocity = Vector3.zero;
            rb.velocity = dir * hitForce;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPos != null)
        {
            Gizmos.DrawWireSphere(groundCheckPos.position, groundCheckRadius);
        }
    }
}
