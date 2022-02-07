using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCrumbling : Platform
{
    [SerializeField] Animator animator;
    [SerializeField] float crumbleDelay = 0.5f;
    [SerializeField] float rechargeDelay = 2f;
    bool recharging = false;
    
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
