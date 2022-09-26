using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public TMP_InputField pseudoField;
    
    [SerializeField] private Sprite _default, _pressed;
    [SerializeField] private AudioClip _compressClip, _uncompressClip;
   
    private Image _img;
    private AudioSource _audioSource;

    private void Start() {
        _img = GetComponent<Image>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _img.sprite = _pressed;
        _audioSource.PlayOneShot(_compressClip);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _img.sprite = _default;
        _audioSource.PlayOneShot(_uncompressClip);
    }

    public void Play() {
        var playerName = pseudoField.text;
        if (playerName == "") {
            print("Please select a pseudo");
            return;
        }
        
        PlayerPrefs.SetString("playerName", playerName);
        SceneManager.LoadScene("Game");
    }
}