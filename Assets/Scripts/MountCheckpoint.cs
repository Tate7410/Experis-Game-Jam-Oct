using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountCheckpoint : MonoBehaviour
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
            save.hasReachedMountain = true;
            PlayerPrefs.SetInt("MountainReached", 1);
        }
    }
}
