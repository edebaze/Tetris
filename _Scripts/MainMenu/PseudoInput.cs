using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PseudoInput : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        GetComponent<TMP_InputField>().text = PlayerPrefs.GetString("playerName");
    }
}
