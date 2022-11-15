using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorHandler : MonoBehaviour
{
    public Movement movement;

    public AudioManager audioManager;

    public void DoubleJump()
    {
        movement.DoubleJump();
    }

    public void Slam()
    {
        movement.Slam();
    }

    public void HitRecover()
    {
        movement.HitRecovery();
    }

    // audio

    public void PlayJump()
    {
        audioManager.Play("Jump");
    }

    public void PlayPop()
    {
        audioManager.Play("Pop");
    }

    public void PlaySlide()
    {
        audioManager.Play("Slide");
        audioManager.StopAudio("Jump");
    }
    public void PlaySlam()
    {
        audioManager.Play("Slam");
        audioManager.StopAudio("Jump");
    }
    public void PlayHurt()
    {
        audioManager.Play("Hurt");
    }
}
