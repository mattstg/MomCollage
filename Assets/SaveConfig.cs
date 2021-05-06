using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveConfig : MonoBehaviour
{
    public TMP_InputField mainDirField;

    public void OnEnable()
    {
        if (PlayerPrefs.HasKey("MainDirPath"))
        {
            mainDirField.text = PlayerPrefs.GetString("MainDirPath");
        }
    }

    public void SaveToConfig()
    {
        PlayerPrefs.SetString("MainDirPath", mainDirField.text);
    }
}
