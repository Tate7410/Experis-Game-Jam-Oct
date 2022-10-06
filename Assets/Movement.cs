using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody rb;

    public Transform cam;

    public float speed = 0f;
    public float maxSpeed;
    public float airSpeed = 200f;
    public float speedUpTime = 0.5f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public float jumpForce = 5f;
    public bool isJumping = false;

    private void Start()
    {
        maxSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        
        

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
                angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.75f);
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
            }
            
            //controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        else
        {
            speed = rb.velocity.magnitude;
            if (!isJumping)
            {
                rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            }
            print(speed);
        }
        

        if (Input.GetButtonDown("Jump"))
        {
            rb.AddRelativeForce(Vector3.up * jumpForce);
            isJumping = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = false;
        }
    }
}
