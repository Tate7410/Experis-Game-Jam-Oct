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

    // speed variables
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

    // animator
    public Animator anim;


    private void Start()
    {
        maxSpeed = speed;
        SetupJumpVariables();
    }

    void SetupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        GroundCheck();



        HandleMovement(direction);




        //jumping

        if (!isJumping && isGrounded && Input.GetButtonDown("Jump"))
        {
            //rb.AddRelativeForce(Vector3.up * jumpForce);
            isJumping = true;
            useGroundedGravity = false;
            anim.SetTrigger("Jump");
            anim.SetBool("Jumping", true);
            rb.velocity = new Vector3(rb.velocity.x, initialJumpVelocity * 0.5f, rb.velocity.z);
        }

        HandleGravity();
    }

    private void HandleGravity()
    {
        bool isFalling = rb.velocity.y <= 0.0f || !Input.GetButton("Jump");
        float fallMultiplier = 2.0f;

        if (useGroundedGravity)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + groundedGravity * Time.deltaTime, rb.velocity.z);
        }
        else if (isFalling)
        {
            float previousYVelocity = rb.velocity.y;
            float newYVelocity = rb.velocity.y + (gravity * fallMultiplier * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            rb.velocity = new Vector3(rb.velocity.x, nextYVelocity, rb.velocity.z);
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
    }

    private void HandleMovement(Vector3 direction)
    {
        if (direction.magnitude >= 0.5f)
        {

            if (speed < maxSpeed)
            {
                speed += Time.deltaTime * speedUpTime;
            }
            else
            {
                speed = maxSpeed;
            }

            float angle;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            
            if (isJumping)
            {
                angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 1f); // don't turn
            }
            else
            {
                angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            }

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDir = moveDir.normalized;

            if (isJumping)
            {
                rb.AddForce(new Vector3(moveDir.x * airSpeed * Time.deltaTime, 0f, moveDir.z * airSpeed * Time.deltaTime));
            }
            else
            {
                rb.velocity = new Vector3(moveDir.x, 0f, moveDir.z).normalized * speed * Time.deltaTime;
                anim.SetBool("Moving", true);
            }

            //controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        else
        {
            speed = rb.velocity.magnitude;
            anim.SetBool("Moving", false);
            if (!isJumping)
            {
                rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            }
            print(speed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = false;
            anim.SetBool("Jumping", false);
            useGroundedGravity = true;
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