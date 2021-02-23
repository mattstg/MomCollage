using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class SetupPage : MonoBehaviour, IPage
{

   // public Dropdown outputModeDropdown;
   // public Dropdown processedModeDropdown;
   // public Dropdown selectFolderLocation;

    public PageType pageType => PageType.SetupPage;

    public void ProgramLoad()
    {
       // outputModeDropdown.ClearOptions();
       // outputModeDropdown.AddOptions(ExtensionFuncs.GetStringListOfEnums(typeof(FolderType)));
       //
       // processedModeDropdown.ClearOptions();
       // processedModeDropdown.AddOptions(ExtensionFuncs.GetStringListOfEnums(typeof(FolderType)));
       //
       // selectFolderLocation.ClearOptions();
       // string[] folderNames = Directory.GetDirectories(GV.mainFolderLocation);
       // selectFolderLocation.AddOptions(folderNames.ToList());
    }

    public void PageOpened()
    {

    }

    public void PageClosed()
    {
    }
}

