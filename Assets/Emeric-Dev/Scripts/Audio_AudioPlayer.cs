using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_AudioPlayer : MonoBehaviour
{
    //Used in conjunction with Animator Trigger Events
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips;

    public void PlayAudioClip(int index){
        if (audioClips.Length > 0){
            if (index < audioClips.Length){
                audioSource.PlayOneShot(audioClips[index]);
            }
        }
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }
}