using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlCheckpoint : MonoBehaviour
{
    private SavingSystem save;

    private void Awake()
    {
        save = FindObjectOfType<SavingSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            save.hasReachedBowl = true;
            PlayerPrefs.SetInt("BowlReached", 1);
        }
    }
}
