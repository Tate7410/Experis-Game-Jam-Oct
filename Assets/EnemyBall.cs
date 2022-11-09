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
}
