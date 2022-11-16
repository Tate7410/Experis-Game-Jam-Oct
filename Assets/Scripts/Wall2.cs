using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall2 : MonoBehaviour
{
    public GameObject fox;
    public GameObject crow;
    public GameObject rabbit;


    // Update is called once per frame
    void Update()
    {
        if (fox == null && crow == null && rabbit == null)
        {
            Destroy(gameObject);
        }
    }
}
