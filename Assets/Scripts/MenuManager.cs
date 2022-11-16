using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }
    public void ExitGame()
    {
        Application.Quit();
        print("Quit");
    }
    public void ResetSaveFile()
    {
        print("reset");
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
    }
}
