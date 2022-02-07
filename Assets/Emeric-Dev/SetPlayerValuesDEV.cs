using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerValuesDEV : MonoBehaviour
{
    //IGNORE THIS SCRIPT, IT IS JUST TO COUNTACT THE VALUES BEING INITIALIZED AT START TEMPORARILY
    [SerializeField] Avatar player;

    [SerializeField] float gravity = 1f;
    [SerializeField] float jumpHeight = 0.5f;
    [SerializeField] float moveSpeed = 0.5f;

    void Update()
    {
        player.gravity = gravity;
        player.jumpAcc = jumpHeight;
        player.acc = moveSpeed;
    }
}
