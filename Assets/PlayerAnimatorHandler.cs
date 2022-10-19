using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorHandler : MonoBehaviour
{
    public Movement movement;

    public void DoubleJump()
    {
        movement.DoubleJump();
    }

    public void Slam()
    {
        movement.Slam();
    }
}
