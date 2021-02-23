using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPage : MonoBehaviour, IPage
{
    //public static SettingsData settingsData;

    public PageType pageType => PageType.SettingsPage;

    public void PageClosed()
    {
    }

    public void PageOpened()
    {
    }

    public void ProgramLoad()
    {
    }


    //[System.Serializable]
    //public class SettingsData
    //{
    //    public string mainFolderPath;
    //
    //}
   
}
