using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEnvironment : MonoBehaviour
{
    [SerializeField] AudioSource _track01;
    [SerializeField] AudioSource _track02;
    [SerializeField] AudioClip _defaultEnvironment;
    bool playingTrack01 = true;

    void Start()
    {
        DefaultEnvironment();
    }

    public void SwapTrack(AudioClip newClip){
        if (playingTrack01){
            _track02.clip = newClip;
            _track02.Play();
            _track01.Stop();
        } else {
            _track01.clip = newClip;
            _track01.Play();
            _track02.Stop();
        }

        playingTrack01 = !playingTrack01;
    }

    public void DefaultEnvironment(){
        SwapTrack(_defaultEnvironment);
    }

}
