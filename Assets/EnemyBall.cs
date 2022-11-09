using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBall : MonoBehaviour
{
    public float attackDist;
    public float speed;
    public bool chasePlayer = false;
    private GameObject player;
    private Rigidbody rb;
    public float bounceForce = 2f;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Movement>().gameObject;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= attackDist)
        {
            chasePlayer = true;
        }
        else
        {
            chasePlayer = false;
        }
    }
    private void FixedUpdate()
    {
        if (chasePlayer)
        {
            rb.AddForce((player.transform.position - transform.position).normalized * speed * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            // Calculate Angle Between the collision point and the player
            Vector3 dir = collision.contacts[0].point - transform.position;
            // We then get the opposite (-Vector3) and normalize it
            dir = -dir.normalized;
            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the player
            //rb.velocity = Vector3.zero;
            rb.velocity = dir * bounceForce;
        }
    }

    public void PopEnemy()
    {
        // create death effect
        Destroy(gameObject);
    }
}
