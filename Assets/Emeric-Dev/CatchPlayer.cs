using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CatchPlayer : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            //Go to Edit > Project Settings > Physics and then check the box “Auto Sync Transforms”. 
            other.gameObject.transform.position = Checkpoint.currentCheckpoint.spawnPoint.position;          
        }
    }
}
