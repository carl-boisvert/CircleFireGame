using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Avatar))]
public class AvatarPlatformCheck : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] float rayLength = 0.2f;
    [SerializeField] Avatar player;
    [SerializeField] Vector3 checkBoxSize = Vector3.one;

    void Awake()
    {
        if (characterController == null) { this.gameObject.TryGetComponent<CharacterController>(out characterController); }
        if (player == null) { this.gameObject.TryGetComponent<Avatar>(out player); }
    }

    void Update()
    {
        Vector3 rayPoint = new Vector3(transform.position.x, transform.position.y - (characterController.height / 2), transform.position.z);
        RaycastHit hit;
        if (Physics.Raycast(rayPoint, Vector3.down, out hit, rayLength)){
        //if (Physics.CheckBox(rayPoint, checkBoxSize, Quaternion.identity)){
            if (hit.transform.tag.Contains("platform")){
                if (hit.transform.TryGetComponent<Platform>(out Platform platform)){
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

