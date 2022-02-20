using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCrumbling : Platform
{
    [SerializeField] Animator animator;
    [SerializeField] Audio_AudioPlayer audioPlayer;
    [Tooltip("The delay before the leaf starts crumbling once the player has landed on it.")]
    [SerializeField] float crumbleDelay = 0.5f;
    [Tooltip("The delay before the leaf resets to its normal position.")]
    [SerializeField] float rechargeDelay = 2f;
    [Tooltip("The speed at which the leaf crumbles. This is set on awake, so changing it at runtime will not affect change the speed.")]
    [SerializeField] float crumbleSpeed = 1f;
    bool recharging = false;
    
    protected override void Awake()
    {
        base.Awake();
        animator.speed = crumbleSpeed;
    }

    public override void OnPlayerLand()
    {
        if ((!recharging) && (player.GetVelocityY() <= 0)){
            StartCoroutine(Crumble());
            audioPlayer.PlayAudioClip(0);
        }
    }

    IEnumerator Crumble(){
        recharging = true;

        yield return new WaitForSeconds(crumbleDelay);
        animator.SetTrigger("crumble");

        yield return new WaitForSeconds(rechargeDelay);

        animator.SetTrigger("recharge");

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        recharging = false;
        audioPlayer.StopAudio();
    }
}
