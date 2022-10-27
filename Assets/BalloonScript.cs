using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonScript : MonoBehaviour
{
    public void PopBalloon()
    {
        // add balloon pop effects
        Destroy(gameObject);
    }
}
