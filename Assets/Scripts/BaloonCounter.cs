using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaloonCounter : MonoBehaviour
{
    public bool resetAllBalloons = false;
    public int balloonCount;
    [SerializeField]
    public GameObject[] balloons;
    // Start is called before the first frame update
    void Start()
    {
        if (resetAllBalloons)
        {
            for (int i = 0; i <= 20; i++)
            {
                PlayerPrefs.SetInt("Balloon#" + i, 0);
            }
        }
    }

    private void Update()
    {
        balloonCount = CountBalloons();
    }

    public int CountBalloons()
    {
        int balloonsPopped = 0;
        for (int i = 0; i < balloons.Length; i++)
        {
            if (balloons[i].GetComponent<BalloonParentScript>().originalBalloonWasPopped)
            {
                balloonsPopped++;
            }
            if (i == balloons.Length)
            {
                return balloonsPopped;
            }
        }
        return balloonsPopped;
    }
}
