using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LightUnlocks : MonoBehaviour
{
    [SerializeField] List<GameObject> lightsToEnable = new List<GameObject>();
    int currentLightIndex = 0;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioClip;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && PowerCell.amountHeld > 0){
            for(int i = 0; i < PowerCell.amountHeld; i++){
                lightsToEnable[currentLightIndex].SetActive(true);
                currentLightIndex++;

                audioSource.PlayOneShot(audioClip);
            }

            PowerCell.amountHeld = 0;
        }
    }
}
