using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingSystem : MonoBehaviour
{
    public bool hasReachedMountain = false;
    public bool hasReachedBowl = false;
    public bool hasReachedBridge = false;
    public bool hasReachedEnd = false;

    public bool resetCheckpoints = false;

    public Transform greenSpawn;
    public Transform mountSpawn;
    public Transform bowlSpawn;
    public Transform bridgeSpawn;
    //public Transform endSpawn;

    public GameObject player;
    private Rigidbody rb;

    public PlayerHealth health;

    private void Awake()
    {
        if (resetCheckpoints)
        {
            if (PlayerPrefs.HasKey("MountainReached"))
            {
                hasReachedMountain = false;
                PlayerPrefs.SetInt("MountainReached", 0);
            }
            if (PlayerPrefs.HasKey("BowlReached"))
            {
                hasReachedBowl = false;
                PlayerPrefs.SetInt("BowlReached", 0);
            }
            if (PlayerPrefs.HasKey("BridgeReached"))
            {
                hasReachedBridge = false;
                PlayerPrefs.SetInt("BridgeReached", 0);
            }
            if (PlayerPrefs.HasKey("EndReached"))
            {
                hasReachedEnd = false;
                PlayerPrefs.SetInt("EndReached", 0);
            }
        }

        rb = player.GetComponent<Rigidbody>();

        if (PlayerPrefs.HasKey("MountainReached"))
        {
            if (PlayerPrefs.GetInt("MountainReached") == 1)
            {
                hasReachedMountain = true;
            }
        }
        if (PlayerPrefs.HasKey("BowlReached"))
        {
            if (PlayerPrefs.GetInt("BowlReached") == 1)
            {
                hasReachedBowl = true;
            }
        }
        if (PlayerPrefs.HasKey("BridgeReached"))
        {
            if (PlayerPrefs.GetInt("BridgeReached") == 1)
            {
                hasReachedBridge = true;
            }
        }
        if (PlayerPrefs.HasKey("EndReached"))
        {
            if (PlayerPrefs.GetInt("EndReached") == 1)
            {
                hasReachedEnd = true;
            }
        }

        //player.transform.position = greenSpawn.position;
        //player.transform.rotation = greenSpawn.rotation;

        


    }

    private void Start()
    {
        if (hasReachedBridge)
        {
            rb.position = bridgeSpawn.position;
            rb.rotation = bridgeSpawn.rotation;
        }
        else if (hasReachedBowl)
        {
            rb.position = bowlSpawn.position;
            rb.rotation = bowlSpawn.rotation;
        }
        else if (hasReachedMountain)
        {
            rb.position = mountSpawn.position;
            rb.rotation = mountSpawn.rotation;
        }
        else
        {
            player.transform.position = greenSpawn.position;
            player.transform.rotation = greenSpawn.rotation;
        }
    }

    public void Respawn(bool damagePlayer)
    {
        if (damagePlayer)
        {
            health.DamagePlayer();
        }
        

        if (hasReachedBridge)
        {
            player.transform.position = bridgeSpawn.position;
            player.transform.rotation = bridgeSpawn.rotation;
        }
        else if (hasReachedBowl)
        {
            player.transform.position = bowlSpawn.position;
            player.transform.rotation = bowlSpawn.rotation;
        }
        else if (hasReachedMountain)
        {
            player.transform.position = mountSpawn.position;
            player.transform.rotation = mountSpawn.rotation;
        }
        else
        {
            player.transform.position = greenSpawn.position;
            player.transform.rotation = greenSpawn.rotation;
        }
    }
}
