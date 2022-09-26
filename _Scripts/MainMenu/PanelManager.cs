using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public GameObject optionsPanel;
    
    public void ExitGame()
    {
        GetComponent<Game>().SaveScore();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    
    }

    public void OpenOptions() {
        optionsPanel.SetActive(true);
    }
}
