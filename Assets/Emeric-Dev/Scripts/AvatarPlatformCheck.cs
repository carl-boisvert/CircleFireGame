using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarPlatformCheck : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] float rayLength = 0.2f;
    [SerializeField] Avatar player;

    void Awake()
    {
        if (characterController == null) { GameObject.FindGameObjectWithTag("Player").TryGetComponent<CharacterController>(out characterController); }
        if (player == null) { GameObject.FindGameObjectWithTag("Player").TryGetComponent<Avatar>(out player); }
    }

    void Update()
    {
        Vector3 rayPoint = new Vector3(transform.position.x, transform.position.y - (characterController.height / 2), transform.position.z);
        RaycastHit hit;
        if (Physics.Raycast(rayPoint, Vector3.down, out hit, rayLength)){
            if (hit.transform.tag.Contains("platform")){
                Debug.Log("hit plaftorm");
                if (hit.transform.TryGetComponent<Platform>(out Platform platform)){
                    Debug.Log("OnPlayLand() called");
                    platform.OnPlayerLand();
                }
            }
        } else {
            OnPlayerExitPlatform();
        }
    }

    void OnPlayerExitPlatform(){
        player.transform.SetParent(null);
    }
}

