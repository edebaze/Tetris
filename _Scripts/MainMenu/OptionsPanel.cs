using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPanel : MonoBehaviour
{
    public Text LanguageText;
    
    private string[] _languages = { "English", "Français"};
    private int _languageIndex;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ExitPanel();
        }
    }

    public void ExitPanel() {
        gameObject.SetActive(false);
    }

    public void ChangeLanguage() {
        // update index 
        _languageIndex++;
        _languageIndex %= _languages.Length;
        
        // update global language 
        LocalizationManager.Language = _languages[_languageIndex];
        LanguageText.text = LocalizationManager.Language;
    }
}
