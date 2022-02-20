using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableSparkleOnGrappleUnlock : MonoBehaviour
{
    static Avatar player;
    
    bool sparklesEnabled = false;
    [SerializeField] GameObject sparkleObject;

    void Awake()
    {
        if (player == null) { GameObject.FindGameObjectWithTag("Player").TryGetComponent<Avatar>(out player); }
    }

    void Start(){
        sparkleObject.SetActive(false);
    }

    void LateUpdate()
    {
        if (sparklesEnabled) { return; }

        if (player.GetGrappleUnlocked()){
            sparklesEnabled = true;
            sparkleObject.SetActive(true);
        }
    }
}
