using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObjectOnTrigger : MonoBehaviour
{
    [SerializeField] GameObject[] objToEnable;
    [SerializeField] GameObject[] objToDisable;
    // Start is called before the first frame update
    void Start()
    {  
        if (objToEnable.Length > 0) { foreach(GameObject go in objToEnable) go.SetActive(false); }
        if (objToEnable.Length > 0) { foreach(GameObject go in objToDisable) go.SetActive(true); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            EnableObjects();
            DisableObjects();
        }
    }

    public void EnableObjects()
    {
        if (objToEnable.Length > 0) { foreach (GameObject go in objToEnable) go.SetActive(true); }
    }

    public void DisableObjects(){
        if (objToEnable.Length > 0) { foreach (GameObject go in objToDisable) go.SetActive(false); }
    }
}
