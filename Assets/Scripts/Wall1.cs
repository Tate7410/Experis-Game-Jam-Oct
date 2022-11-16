using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall1 : MonoBehaviour
{
    public GameObject bear1;
    public GameObject bear2;

    // Update is called once per frame
    void Update()
    {
        if (bear1 == null && bear2 == null)
        {
            Destroy(gameObject);
        }
    }
}
