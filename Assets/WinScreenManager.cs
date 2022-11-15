using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenManager : MonoBehaviour
{
    public void QuitGame()
    {
        print("quit game");
        Application.Quit();
    }
    
    public void RestartGame()
    {
        if (PlayerPrefs.HasKey("MountainReached"))
        {
            PlayerPrefs.SetInt("MountainReached", 0);
        }
        if (PlayerPrefs.HasKey("BowlReached"))
        {
            PlayerPrefs.SetInt("BowlReached", 0);
        }
        if (PlayerPrefs.HasKey("BridgeReached"))
        {
            PlayerPrefs.SetInt("BridgeReached", 0);
        }
        if (PlayerPrefs.HasKey("EndReached"))
        {
            PlayerPrefs.SetInt("EndReached", 0);
        }
        if (PlayerPrefs.HasKey("Lives"))
        {
            PlayerPrefs.SetInt("Lives", 3);
        }

        SceneManager.LoadScene(1);
    }
}
