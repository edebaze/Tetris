using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject gameOverScreen;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (Time.timeScale == 0) {
                Resume();
            }
            else {
                Pause();
            }
        }
    }

    public void GameOverScreen() {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Pause() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    
    public void Resume() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    public void NewGame() {
        var gameMaster = GetComponent<Game>();
        gameMaster.Reset();
        gameMaster.Start();
        
        Resume();
    }

    public void ExitToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    
    }
    
}
