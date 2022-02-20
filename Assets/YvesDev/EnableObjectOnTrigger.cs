using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObjectOnTrigger : MonoBehaviour
{
    GameObject[] objToEnable;
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject go in objToEnable) go.SetActive(false);
    }

    public void UnableObj()
    {
        foreach (GameObject go in objToEnable) go.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player")) UnableObj();
    }
}
