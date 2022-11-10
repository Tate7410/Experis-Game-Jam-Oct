using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeCheckpoint : MonoBehaviour
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
            save.hasReachedBridge = true;
            PlayerPrefs.SetInt("BridgeReached", 1);
        }
    }
}
