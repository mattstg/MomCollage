using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveConfig : MonoBehaviour
{
    public TMP_InputField mainDirField;
    public TMP_InputField imgMagikField;

    public void OnEnable()
    {
        if (PlayerPrefs.HasKey("MainDirPath"))
        {
            mainDirField.text = PlayerPrefs.GetString("MainDirPath");
        }
        if (PlayerPrefs.HasKey("ImgMagikDirPath"))
        {
            imgMagikField.text = PlayerPrefs.GetString("ImgMagikDirPath");
        }
    }

    public void SaveToConfig()
    {
        PlayerPrefs.SetString("MainDirPath", mainDirField.text);
        PlayerPrefs.SetString("ImgMagikDirPath", imgMagikField.text);
    }
}
