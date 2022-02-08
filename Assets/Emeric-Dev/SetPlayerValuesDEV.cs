using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerValuesDEV : MonoBehaviour
{
    //IGNORE THIS SCRIPT, IT IS JUST TO COUNTACT THE VALUES BEING INITIALIZED AT START TEMPORARILY
    [SerializeField] Avatar player;

    [SerializeField] float gravity = -1f;
    [SerializeField] float jumpSpeed = 0.5f;
    [SerializeField] float moveSpeed = 0.5f;

    void Update()
    {
        player.SetGravity(gravity);
        player.SetJumpSpeed(jumpSpeed);
        player.SetMoveSpeed(moveSpeed);
    }
}
