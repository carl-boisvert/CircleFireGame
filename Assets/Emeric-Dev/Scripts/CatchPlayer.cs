using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CatchPlayer : MonoBehaviour
{
    [SerializeField] BoxCollider _myCollider;
    [SerializeField] bool DEBUG = false;

    void Awake()
    {
        if (_myCollider == null) { this.gameObject.TryGetComponent<BoxCollider>(out _myCollider); }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            //Go to Edit > Project Settings > Physics and then check the box “Auto Sync Transforms”. 
            other.gameObject.transform.position = Checkpoint.currentCheckpoint.spawnPoint.position;          
        }
    }

    void OnDrawGizmos()
    {
        if (_myCollider && DEBUG){
            Gizmos.color = Color.cyan;
            Gizmos.DrawCube(transform.position, _myCollider.size);
        }
    }

}
