using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGizmos : MonoBehaviour
{
    [SerializeField] bool DEBUG = false;
    
    [Header("Elements")]
    [SerializeField] float hoverRadius = 1f;
    [SerializeField] Transform grappleMaxPos;
    [SerializeField] Avatar avatar;

    void OnDrawGizmos()
    {
        if (DEBUG){
            Gizmos.color = Color.magenta;

            float radius = hoverRadius * (Mathf.InverseLerp(0f, avatar.GetMaxFuel(), avatar.GetFuel()));
            Gizmos.DrawSphere(transform.position, radius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, grappleMaxPos.position);
        }
    }
}
