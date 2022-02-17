using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AudioEnvironementSwap : MonoBehaviour
{
    static AudioEnvironment environmentAudio;
    [SerializeField] AudioClip myTrack;

    void Awake()
    {
        if (environmentAudio == null) { GameObject.FindGameObjectWithTag("audio-environment").TryGetComponent<AudioEnvironment>(out environmentAudio); }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            environmentAudio.SwapTrack(myTrack);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")){
            environmentAudio.DefaultEnvironment();
        }
    }
}
