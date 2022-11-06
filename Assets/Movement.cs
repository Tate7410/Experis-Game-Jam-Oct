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
    float groundedGravity = -1f;

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

    //  slope handling
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    public LayerMask slopeCheckLayers;

    // balloon pop
    public Transform balloonCheck;
    public float balloonPopRadius = 1f;

    // platforms
    /*
    public LayerMask platforms;
    public bool isOnPlatform;
    */
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
        HandleSlide();
        HandleSlam();
        HandleJumping();
        BalloonBounce();
        HandleGravity();
    }

    private void BalloonBounce()
    {
        Collider[] balloons = Physics.OverlapSphere(balloonCheck.position, groundCheckRadius, whatIsGround);
        foreach (Collider balloon in balloons)
        {
            if (balloon.gameObject.tag == "Balloon")
            {
                ResetVariablesOnBounce();
                rb.velocity = new Vector3(rb.velocity.x, initialJumpVelocity * 0.8f, rb.velocity.z);
                // pop balloon
                balloon.GetComponent<BalloonScript>().PopBalloon();
            }
        }
    }

    private void HandleSlide()
    {
        if (direction.magnitude >= 0.5f && Input.GetAxisRaw("Attack") >= 0.5f && !isStartingSlam && !startDoubleJump && !isSlamming && !hitReaction && !isSliding || direction.magnitude >= 0.5f && Input.GetButtonDown("Fire2") && !isStartingSlam && !startDoubleJump && !isSlamming && !hitReaction && !isSliding)
        {
            anim.SetBool("SlideStop", false);
            anim.SetTrigger("Slide");
            isSliding = true;
            rb.velocity = (moveDir + new Vector3(0, 1f, 0)) * InitialSlideSpeed;
        }
        else if (!isGrounded && anim.GetBool("Jumping") == false && direction.magnitude >= 0.5f && Input.GetAxisRaw("Attack") >= 0.5f && !isStartingSlam && !startDoubleJump && !isSlamming && !hitReaction && !isSliding || !isGrounded && anim.GetBool("Jumping") == false && direction.magnitude >= 0.5f && Input.GetButtonDown("Fire2") && !isStartingSlam && !startDoubleJump && !isSlamming && !hitReaction && !isSliding)
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
        else if (!isGrounded && anim.GetBool("Jumping") == false && isJumping && Input.GetAxisRaw("Attack") >= 0.5f && !isStartingSlam && !startDoubleJump && !isSlamming && !isSliding || isJumping && Input.GetButtonDown("Fire2") && !isStartingSlam && !startDoubleJump && !isSlamming && !isSliding)
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
            doubleJumpTimer = doubleJumpTimerReset;
            //hasDoubleJumped = true;
        }
        if (startDoubleJump && !hasDoubleJumped)
        {
            doubleJumpTimer -= Time.deltaTime;
        }
        if (isJumping && doubleJumpTimer >= 0 && Input.GetButtonUp("Jump") && startDoubleJump)
        {
            anim.SetTrigger("DoubleJump");
            anim.SetBool("Jumping", true);
            isDoubleJumping = true;
        }
        if (doubleJumpTimer <= 0 && !hasDoubleJumped)
        {
            startDoubleJump = false;
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
        else if (!isGrounded && anim.GetBool("Jumping") == false && Input.GetButton("Jump") && !isStartingSlam && !isSlamming && floatTime >= 0f && !fallingFromFloat && !hitReaction && !isSliding)
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
        if (!isGrounded && anim.GetBool("Jumping") == false && !isSliding)
        {
            isJumping = true;
            useGroundedGravity = false;
            anim.SetTrigger("Jump");
            anim.SetBool("Jumping", true);
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

            //isOnPlatform = Physics.Raycast(groundCheckPos.position, Vector3.down, groundCheckRadius, platforms);

            if (isJumping && !isSliding)
            {
                rb.AddForce(new Vector3(moveDir.x * airSpeed * Time.fixedDeltaTime, 0f, moveDir.z * airSpeed * Time.fixedDeltaTime));
            }
            else if (isSliding)
            {
                // don't apply directional velocity
            }
            else if (OnSlope())
            {
                rb.velocity = GetSlopeMoveDirection() * speed * Time.fixedDeltaTime;
            }
            /*
            else if (isOnPlatform && isGrounded)
            {
                rb.velocity = new Vector3(moveDir.x, moveDir.y, moveDir.z).normalized * speed * Time.fixedDeltaTime;
                print("onplatform");
            }
            */
            else
            {
                rb.velocity = new Vector3(moveDir.x, 0f, moveDir.z).normalized * speed * Time.fixedDeltaTime;
            }
            anim.SetBool("Moving", true);
            //controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        else
        {
            //isOnPlatform = Physics.Raycast(groundCheckPos.position, Vector3.down, groundCheckRadius, platforms);
            speed = rb.velocity.magnitude;
            anim.SetBool("Moving", false);
            if (!isJumping && !hitReaction && !isSliding)
            {
                rb.velocity = new Vector3(0f, 0f, 0f);
            }
            else if (hitReaction)
            {
                // don't apply velocity
            }
            /*
            else if (isOnPlatform && isGrounded)
            {
                rb.velocity = new Vector3(0f, moveDir.y, 0f);
                print("onplatform");
            }
            */
            //print(speed);
        }
    }

    public void DoubleJump()
    {
        rb.velocity = new Vector3(rb.velocity.x, initialJumpVelocity * 0.7f, rb.velocity.z);
        hasDoubleJumped = true;
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
        if (collision.gameObject.tag == "Balloon")
        {
            ResetVariablesOnBounce();
            rb.velocity = new Vector3(rb.velocity.x, initialJumpVelocity * 0.8f, rb.velocity.z);
            // pop balloon
            collision.gameObject.GetComponent<BalloonScript>().PopBalloon();
        }
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

    private void ResetVariablesOnBounce()
    {
        // reset variables
        startDoubleJump = false;
        doubleJumpTimer = doubleJumpTimerReset;
        isStartingSlam = false;
        isSlamming = false;
        hasDoubleJumped = false;
        floatTime = floatTimeReset;
        fallingFromFloat = false;
        //reset double jump variables
        isDoubleJumping = false;
        startDoubleJump = false;
        // jump
        isJumping = true;
        isSliding = false;
        jumpTimer = jumpTimerReset;
        useGroundedGravity = false;
        anim.SetTrigger("Jump");
        anim.SetBool("Jumping", true);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPos != null)
        {
            Gizmos.DrawWireSphere(groundCheckPos.position, groundCheckRadius);
        }
        if (balloonCheck != null)
        {
            Gizmos.DrawWireSphere(balloonCheck.position, balloonPopRadius);
        }
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(groundCheckPos.position, Vector3.down, out slopeHit, 1f, slopeCheckLayers))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDir, slopeHit.normal).normalized;
    }
}
