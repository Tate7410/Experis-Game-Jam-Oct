using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject overlay;
    public GameObject button;

    public bool isPaused = false;
    public bool isPressingButton = false;
    public bool canSwitch = true;

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && !isPaused && canSwitch)
        {
            isPressingButton = true;
            canSwitch = false;
            isPaused = true;
            Time.timeScale = 0f;
            overlay.SetActive(true);
            button.SetActive(true);
        }
        if (Input.GetButtonDown("Cancel") && isPaused && canSwitch)
        {
            isPressingButton = true;
            canSwitch = false;
            isPaused = false;
            Time.timeScale = 1f;
            overlay.SetActive(false);
            button.SetActive(false);
        }
        if (isPressingButton && Input.GetButtonUp("Cancel"))
        {
            isPressingButton = false;
            canSwitch = true;
        }

    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
