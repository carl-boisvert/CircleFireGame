using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBouncing : Platform
{
    [SerializeField] float force = 1f;
    
    public override void OnPlayerLand()
    {
        if (player.TryGetComponent<Avatar>(out Avatar playerAvatar)){
            playerAvatar.SetVelocityY(force);
        }
    }
}
