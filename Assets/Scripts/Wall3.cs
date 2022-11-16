using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall3 : MonoBehaviour
{
    public GameObject bear;
    public GameObject rabbit;
    public GameObject fox;
    public GameObject crow;

    // Update is called once per frame
    void Update()
    {
        if (bear == null && rabbit == null && fox == null && crow == null)
        {
            Destroy(gameObject);
        }
    }
}
