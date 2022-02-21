using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    bool inTransition = false;

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
        if (Input.GetKeyDown(KeyCode.Escape) && !inTransition){
            PauseGame();
        }
    }

    void PauseGame(){
        inTransition = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame(){
        inTransition = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }
}
