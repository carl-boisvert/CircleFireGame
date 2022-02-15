using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    [SerializeField] GameObject fadeIn;
    [SerializeField] float transitionDuration = 2f;

    public void ChangeToScene(string sceneName){
        Time.timeScale = 1f;
        StartCoroutine(ChangeToSceneCoroutine(sceneName));
    }
    
    IEnumerator ChangeToSceneCoroutine(string sceneName){
        Instantiate(fadeIn, Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(transitionDuration);

        SceneManager.LoadScene(sceneName);
    }

    public void QuitApplication(){
        Application.Quit();
    }
    //DELETE. Test commit - Justin
}
