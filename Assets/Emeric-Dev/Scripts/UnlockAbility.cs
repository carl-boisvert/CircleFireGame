using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class UnlockAbility : MonoBehaviour
{
    enum PlayerAbilities {hover, boost, hookshot};
    [SerializeField] PlayerAbilities abilityToUnlock = PlayerAbilities.hover;
    [SerializeField] Avatar player;
    [SerializeField] GameObject jetpack;
    [SerializeField] GameObject drone;

    void Awake()
    {
        if (player == null) { GameObject.FindGameObjectWithTag("Player").TryGetComponent<Avatar>(out player); }
    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player")) && (player != null)){
            switch (abilityToUnlock){
                case PlayerAbilities.hover: 
                    player.SetHoverUnlocked(true); 
                    jetpack.SetActive(true);
                    break;
                case PlayerAbilities.boost: 
                    player.SetAirBoostUnlocked(true); 
                    jetpack.SetActive(true);
                    break;
                case PlayerAbilities.hookshot: 
                    player.SetGrappleUnlocked(true); 
                    drone.SetActive(true);
                    break;
                default: break;
            }
        }
    }
}
