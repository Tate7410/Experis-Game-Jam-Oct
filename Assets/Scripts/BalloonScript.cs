using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonScript : MonoBehaviour
{
    public GameObject confetti;

    public void PopBalloon()
    {
        Instantiate(confetti, transform.position, new Quaternion(0, 0, 0, 0));
        GetComponentInParent<BalloonParentScript>().ResetTimer();
        Destroy(gameObject);
    }
}
