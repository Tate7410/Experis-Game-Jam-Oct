using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallBarrier : MonoBehaviour
{
    public SavingSystem savingSystem;

    private void Awake()
    {
        savingSystem = FindObjectOfType<SavingSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            savingSystem.Respawn(true);
        }
        if (other.gameObject.tag == "Ball")
        {
            Destroy(other.gameObject);
        }
    }
}
