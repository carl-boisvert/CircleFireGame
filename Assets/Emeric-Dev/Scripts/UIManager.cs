using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    void Awake()
    {
        if (pauseMenu == null) { pauseMenu = GameObject.FindGameObjectWithTag("pause-menu"); }
    }

    void Start()
    {
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            PauseGame();
        }
    }

    void PauseGame(){
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame(){
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }
}
