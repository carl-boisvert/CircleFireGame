using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    protected virtual void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player")){
            OnPlayerLand(other.gameObject);
        }
    }

    protected virtual void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player")){
            OnPlayerExit(other.gameObject);
        }
    }

    protected virtual void OnPlayerLand(GameObject player){
        player.transform.SetParent(this.transform);
    }

    protected virtual void OnPlayerExit(GameObject player){
        player.transform.SetParent(null);
    }
}
