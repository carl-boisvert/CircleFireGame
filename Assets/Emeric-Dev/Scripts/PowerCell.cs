using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCell : MonoBehaviour
{
    public static int amountHeld = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            amountHeld++;
            Destroy(this);
        }
    }
}
