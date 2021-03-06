using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_AudioPlayer : MonoBehaviour
{
    //Used in conjunction with Animator Trigger Events
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips;

    #region Public Functions

    public void PlayAudioClip(int index){

        if (CanPlayClip(index)){
            audioSource.PlayOneShot(audioClips[index]);
        }
    }
    public void PlayAudioClip(int index, float volume){
        if (CanPlayClip(index)){
            audioSource.PlayOneShot(audioClips[index], volume);
        }
    }

    public void PlayAudioClipRandomFromRange(int startRange, int endRange){
        if (CanPlayClip(startRange, endRange)){
            int randomIndex = Random.Range(startRange, endRange);
            audioSource.PlayOneShot(audioClips[randomIndex]);
        }
    }
    public void PlayAudioClipRandomFromRange(int startRange, int endRange, float volume){
        if (CanPlayClip(startRange, endRange)){
            int randomIndex = Random.Range(startRange, endRange);
            audioSource.PlayOneShot(audioClips[randomIndex], volume);
        }
    }

    public void PlayAudioClipsFromRange(int startRange, int endRange){
        if (CanPlayClip(startRange, endRange)){
            for (int i = startRange; i <= endRange; i++){
                audioSource.PlayOneShot(audioClips[i]);
            }
        }
    }
    public void PlayAudioClipsFromRange(int startRange, int endRange, float volume){
        if (CanPlayClip(startRange, endRange)){
            for (int i = startRange; i <= endRange; i++){
                audioSource.PlayOneShot(audioClips[i], volume);
            }
        }
    }

    public void PlayRandomClip()
    {
        PlayAudioClipRandomFromRange(0, (audioClips.Length - 1));
    }

    public void PlayRandomClipAtVolume(float volume){
        PlayAudioClipRandomFromRange(0, (audioClips.Length - 1), volume);
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }

    #endregion
    #region Private Functions

    bool CanPlayClip(int index){
        bool canPlay = true;

        if (audioClips.Length <= 0)     { canPlay = false; }
        if (index < 0)                 { canPlay = false; }
        if (index >= audioClips.Length) { canPlay = false; }

        return canPlay;
    }

    bool CanPlayClip(int startIndex, int endIndex){
        bool canPlay = true;

        if (audioClips.Length <= 0)                                             { canPlay = false; }
        if (startIndex < 0 || endIndex < 0)                                     { canPlay = false; }
        if (startIndex >= audioClips.Length || startIndex >= audioClips.Length) { canPlay = false; }
        if (startIndex > endIndex)                                              { canPlay = false; }

        return canPlay;
    }

    #endregion
}