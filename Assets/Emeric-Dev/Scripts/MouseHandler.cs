using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{
    void LateUpdate()
    {
        if (Time.timeScale == 1){
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        } else if (Time.timeScale == 0){
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
