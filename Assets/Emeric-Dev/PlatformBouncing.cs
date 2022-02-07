using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBouncing : Platform
{
    [SerializeField] float force = 1f;
    
    protected override void OnPlayerLand(GameObject player)
    {
        if (player.TryGetComponent<Avatar>(out Avatar playerAvatar)){
            playerAvatar.velY += force;
        }
    }
}
