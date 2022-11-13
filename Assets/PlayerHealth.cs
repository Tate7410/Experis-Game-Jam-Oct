using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth;
    public int currentLives = 3;
    public bool isInvincible;
    public bool resetLives = false;

    // save system
    private SavingSystem savingSystem;

    // blinking
    public float blinkingTimer = 0.0f;
    public float blinkingMiniDuration = 0.1f;
    public float blinkingTotalTimer = 0.0f;
    public float blinkingTotalDuration = 1.0f;
    public bool startBlinking = false;

    public SkinnedMeshRenderer playerCube;
    public SkinnedMeshRenderer playerIcosphere;
    public SkinnedMeshRenderer playerCylinder;

    // UI
    public TextMeshProUGUI textMesh;
    public Image image;
    public Sprite fullHealth;
    public Sprite twoHealth;
    public Sprite oneHealth;
    public Sprite noHealth;

    private void Awake()
    {
        savingSystem = FindObjectOfType<SavingSystem>();

        currentHealth = 3;
        if (PlayerPrefs.HasKey("Lives"))
        {
            currentLives = PlayerPrefs.GetInt("Lives");
        }

        if (resetLives)
        {
            if (PlayerPrefs.HasKey("Lives"))
            {
                PlayerPrefs.SetInt("Lives", 3);
            }
        }
    }

    void Update()
    {
        HandleBlinking();
        HandleHealthUI();
        HandleLivesUI();

        if (currentHealth > 3)
        {
            currentHealth = 3;
        }

        HandleDeath();
    }

    private void HandleLivesUI()
    {
        if (currentLives == 3)
        {
            textMesh.text = "3 X";
        }
        else if (currentLives == 2)
        {
            textMesh.text = "2 X";
        }
        else if (currentLives == 1)
        {
            textMesh.text = "1 X";
        }
        else if (currentLives == 0)
        {
            textMesh.text = "0 X";
        }
    }

    private void HandleHealthUI()
    {
        if (currentHealth == 3)
        {
            image.sprite = fullHealth;
        }
        else if (currentHealth == 2)
        {
            image.sprite = twoHealth;
        }
        else if (currentHealth == 1)
        {
            image.sprite = oneHealth;
        }
        else if (currentHealth == 0)
        {
            image.sprite = noHealth;
        }
    }

    private void HandleBlinking()
    {
        if (startBlinking == true)
        {
            BlinkingEffect();
            isInvincible = true;
        }
        if (startBlinking == false)
        {
            isInvincible = false;
        }
    }

    private void HandleDeath()
    {
        if (currentHealth <= 0)
        {
            if (currentLives > 0)
            {
                currentLives--;
                PlayerPrefs.SetInt("Lives", currentLives);
                currentHealth = 3;
                savingSystem.Respawn(false);
            }
            else
            {
                if (PlayerPrefs.HasKey("MountainReached"))
                {
                    savingSystem.hasReachedMountain = false;
                    PlayerPrefs.SetInt("MountainReached", 0);
                }
                if (PlayerPrefs.HasKey("BowlReached"))
                {
                    savingSystem.hasReachedBowl = false;
                    PlayerPrefs.SetInt("BowlReached", 0);
                }
                if (PlayerPrefs.HasKey("BridgeReached"))
                {
                    savingSystem.hasReachedBridge = false;
                    PlayerPrefs.SetInt("BridgeReached", 0);
                }
                if (PlayerPrefs.HasKey("EndReached"))
                {
                    savingSystem.hasReachedEnd = false;
                    PlayerPrefs.SetInt("EndReached", 0);
                }
                currentLives = 3;
                PlayerPrefs.SetInt("Lives", 3);
                SceneManager.LoadScene(0); // remember to change this once the scenes are organized properly
            }
        }
    }

    private void BlinkingEffect()
    {
        blinkingTotalTimer += Time.deltaTime;
        if (blinkingTotalTimer >= blinkingTotalDuration)
        {
            startBlinking = false;
            blinkingTotalTimer = 0.0f;
            playerCube.enabled = true;
            playerCylinder.enabled = true;
            playerIcosphere.enabled = true;                                    
            return;
        }

        blinkingTimer += Time.deltaTime;
        if (blinkingTimer >= blinkingMiniDuration)
        {
            blinkingTimer = 0.0f;
            if (playerCube.enabled == true)
            {
                playerCube.enabled = false;
                playerCylinder.enabled = false;
                playerIcosphere.enabled = false;
                //playerGFX.SetActive(false);
            }
            else
            {
                playerCube.enabled = true;
                playerCylinder.enabled = true;
                playerIcosphere.enabled = true;
                //playerGFX.SetActive(true);
            }
        }
    }

    

    public void DamagePlayer()
    {
        startBlinking = true;
        currentHealth--;
    }

    public void HealPlayer()
    {
        currentHealth++;
    }



}
