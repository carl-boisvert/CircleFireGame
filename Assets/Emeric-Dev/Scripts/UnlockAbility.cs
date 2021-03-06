using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class UnlockAbility : MonoBehaviour
{
    enum PlayerAbilities {hover, boost, hookshot};
    [Header("Ability to unlock")]
    [SerializeField] PlayerAbilities abilityToUnlock = PlayerAbilities.hover;

    [Header("References")]
    [SerializeField] Avatar player;
    [SerializeField] GameObject jetpack;
    [SerializeField] GameObject drone;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip unlockAudioClip;
    [SerializeField] float volume = 1f;

    void Awake()
    {
        if (player == null) { GameObject.FindGameObjectWithTag("Player").TryGetComponent<Avatar>(out player); }
        if (jetpack == null) { jetpack = GameObject.FindGameObjectWithTag("jetpack"); }
        if (drone == null) { drone = GameObject.FindGameObjectWithTag("drone"); }
    }

    void Start()
    {
        jetpack.SetActive(false);
        drone.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player")) && (player != null)){
            switch (abilityToUnlock){
                case PlayerAbilities.hover: UnlockHover(); break;
                case PlayerAbilities.boost: UnlockBoost(); break;
                case PlayerAbilities.hookshot: UnlockHookshot(); break;
                default: break;
            }

            if (audioSource) { audioSource.PlayOneShot(unlockAudioClip, volume); }
            Destroy(this.gameObject);
            //StartCoroutine(IDestroyThisObj());
        }
    }

    void UnlockHover(){
        player.SetHoverUnlocked(true); 
        jetpack.SetActive(true);
    }

    void UnlockBoost(){
        player.SetAirBoostUnlocked(true); 
        jetpack.SetActive(true);
    }

    void UnlockHookshot(){
        player.SetGrappleUnlocked(true); 
        drone.SetActive(true);
    }

    IEnumerator IDestroyThisObj(){
        yield return new WaitForSeconds(0.01f);
        Destroy(this.gameObject);
    }
}
