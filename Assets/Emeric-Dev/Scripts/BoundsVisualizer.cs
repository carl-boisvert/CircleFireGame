using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsVisualizer : MonoBehaviour
{
    [SerializeField] BoxCollider _myCollider;
    [SerializeField] bool DEBUG = false;
    
    void OnDrawGizmos()
    {
        if (_myCollider && DEBUG){
            Gizmos.matrix = this.transform.localToWorldMatrix;
            Gizmos.color = Color.green;
            Gizmos.DrawCube(Vector3.zero, _myCollider.size);
        }
    }
}
