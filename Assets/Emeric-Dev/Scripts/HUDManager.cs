using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [Header("Fuel")]
    [SerializeField] Transform fuelGauge;
    [SerializeField] Avatar player;

    void Update()
    {
        fuelGauge.localScale = new Vector3(Mathf.InverseLerp(0f, player.GetMaxFuel(), player.GetFuel()), 1f, 1f);
    }
}
