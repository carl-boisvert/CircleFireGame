using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCrumbling : Platform
{
    [SerializeField] Animator animator;
    [Tooltip("The delay before the leaf starts crumbling once the player has landed on it.")]
    [SerializeField] float crumbleDelay = 0.5f;
    [Tooltip("The delay before the leaf resets to its normal position.")]
    [SerializeField] float rechargeDelay = 2f;
    [Tooltip("The speed at which the leaf crumbles. This is set on awake, so changing it at runtime will not affect change the speed.")]
    [SerializeField] float crumbleSpeed = 1f;
    bool recharging = false;
    
    void Awake()
    {
        animator.speed = crumbleSpeed;
    }

    protected override void OnPlayerLand(GameObject player)
    {
        if (!recharging)
            StartCoroutine(Crumble());
    }

    IEnumerator Crumble(){
        recharging = true;

        yield return new WaitForSeconds(crumbleDelay);
        animator.SetTrigger("crumble");

        yield return new WaitForSeconds(rechargeDelay);

        animator.SetTrigger("recharge");
        recharging = false;
    }
}
