using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldBalloon : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // load win screen
        }
    }
}
