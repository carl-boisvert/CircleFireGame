using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBouncing : Platform
{
    [SerializeField] float _bounceforce = 1f;

    [Header("Audio")]
    [SerializeField] Audio_AudioPlayer _audioPlayer;
    [SerializeField] Vector2Int _audioIndexRange = new Vector2Int(0, 1);
    
    void Awake()
    {
        if (_audioPlayer == null) { this.gameObject.TryGetComponent<Audio_AudioPlayer>(out _audioPlayer); }
    }
    
    public override void OnPlayerLand()
    {
        if (player.TryGetComponent<Avatar>(out Avatar playerAvatar)){
            playerAvatar.SetVelocityY(_bounceforce);
            _audioPlayer.PlayAudioClipsFromRange(_audioIndexRange.x, _audioIndexRange.y);
        }
    }
}
