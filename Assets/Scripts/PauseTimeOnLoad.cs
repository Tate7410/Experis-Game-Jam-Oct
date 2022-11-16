using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseTimeOnLoad : MonoBehaviour
{
    public void PauseTime()
    {
        Time.timeScale = 0f;
    }

    public void UnpauseTime()
    {
        Time.timeScale = 1f;
    }
}
