using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth;
    public bool isInvincible;

    public float blinkingTimer = 0.0f;
    public float blinkingMiniDuration = 0.1f;
    public float blinkingTotalTimer = 0.0f;
    public float blinkingTotalDuration = 1.0f;
    public bool startBlinking = false;

    public SkinnedMeshRenderer playerCube;
    public SkinnedMeshRenderer playerIcosphere;
    public SkinnedMeshRenderer playerCylinder;
    //public BitHealth bitHealth;
    //public SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = 3;
    }
    void Update()
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

        if (currentHealth > 3)
        {
            currentHealth = 3;
        }
        if (currentHealth <= 0)
        {
            print("You dead");
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
