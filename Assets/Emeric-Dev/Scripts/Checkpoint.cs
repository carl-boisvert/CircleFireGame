using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    public static Checkpoint currentCheckpoint;

    public Transform spawnPoint;

    [SerializeField] BoxCollider _myCollider;
    [SerializeField] bool DEBUG = false;

    private void OnTriggerEnter(Collider other) {
        currentCheckpoint = this;
    }

    void OnDrawGizmos()
    {
        if (_myCollider && DEBUG){
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(transform.position, _myCollider.size);
        }
    }
}
