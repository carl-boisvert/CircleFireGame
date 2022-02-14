using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    protected static Avatar player;
    void Awake()
    {
        if (player == null) { player = FindObjectOfType<Avatar>(); }
    }

    public virtual void OnPlayerLand(){
        player.transform.SetParent(this.transform);
    }
    
    /*
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

    protected virtual void OnPlayerExit(){
        player.transform.SetParent(null);
    }

    */
}
