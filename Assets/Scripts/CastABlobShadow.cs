using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastABlobShadow : MonoBehaviour
{
    RaycastHit hit;
    public LayerMask layers;
    public GameObject shadow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit,  20f, layers))
        {
            shadow.SetActive(true);
            shadow.transform.position = hit.point;
            shadow.transform.up = hit.normal;
        }
        else
        {
            shadow.SetActive(false);
        }
    }
}
