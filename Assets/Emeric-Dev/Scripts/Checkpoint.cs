using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    public static Checkpoint currentCheckpoint;
    
    public Transform spawnPoint;

    private void OnTriggerEnter(Collider other) {
        currentCheckpoint = this;
    }
}
