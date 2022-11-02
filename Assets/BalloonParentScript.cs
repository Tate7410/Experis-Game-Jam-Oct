using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonParentScript : MonoBehaviour
{
    public GameObject balloon;
    public int balloonNumber;
    public string balloonIdent; // if the number received from playerPrefs is 0, it hasn't been popped and a 1 mean it has been popped
    public bool originalBalloonWasPopped; // to do: create manager that checks save data and assigns this true or false;

    public float respawnTimer = 1f;
    private float respawnTimerReset;
    private bool needsToRespawn = false;
    public GameObject whiteBalloon;

    private void Awake()
    {
        respawnTimerReset = respawnTimer;
        balloonIdent = "Balloon#" + balloonNumber;
        if (PlayerPrefs.HasKey(balloonIdent))
        {
            if (PlayerPrefs.GetInt(balloonIdent) == 1)
            {
                originalBalloonWasPopped = true;
                Destroy(balloon);
                Instantiate(whiteBalloon, transform.position, transform.rotation, this.transform);
                //needsToRespawn = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (needsToRespawn)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0)
            {
                Instantiate(whiteBalloon, transform.position, transform.rotation, this.transform);
                needsToRespawn = false;
            }
        }
    }

    public void ResetTimer()
    {
        respawnTimer = respawnTimerReset;
        needsToRespawn = true;
        if (!originalBalloonWasPopped)
        {
            originalBalloonWasPopped = true;
            PlayerPrefs.SetInt(balloonIdent, 1);
        }
    }
}
