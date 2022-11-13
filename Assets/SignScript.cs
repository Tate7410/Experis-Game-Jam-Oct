using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignScript : MonoBehaviour
{
    public GameObject sign;
    public float signDist;
    private GameObject player;

    private void Start()
    {
        player = FindObjectOfType<Movement>().gameObject;
    }

    private void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= signDist)
        {
            sign.SetActive(true);
        }
        else
        {
            sign.SetActive(false);
        }
    }
}
