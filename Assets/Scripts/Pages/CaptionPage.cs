using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CaptionPage : MonoBehaviour, IPage
{
    public PageType pageType => PageType.CaptionPage;
    public Dropdown outputModeDropdown;
    public Dropdown processedModeDropdown;
    public Dropdown selectFolderLocation;
    string folderNameSelected;
    string[] fileNamesInFolder;
    public Image displayImg;
    int picIndex;

    public void PageClosed()
    {
        
    }

    public void PageOpened()
    {
        
    }

    public void ProgramLoad()
    {
        outputModeDropdown.ClearOptions();
        outputModeDropdown.AddOptions(ExtensionFuncs.GetStringListOfEnums(typeof(FolderType)));

        processedModeDropdown.ClearOptions();
        processedModeDropdown.AddOptions(ExtensionFuncs.GetStringListOfEnums(typeof(FolderType)));

        selectFolderLocation.ClearOptions();
        string[] folderNames = Directory.GetDirectories(GV.mainFolderLocation);
        selectFolderLocation.AddOptions(folderNames.ToList());
    }

    public void FolderOptionSelected(int newIndex)
    {
        folderNameSelected = selectFolderLocation.options[newIndex].text;
        fileNamesInFolder = GetAllFilesAtFolder(folderNameSelected);
        picIndex = 0;
        displayImg.sprite = LoadImages(fileNamesInFolder[picIndex]);

    }

    private string[] GetAllFilesAtFolder(string folderName)
    {
        string totalPath = folderName;
        return Directory.GetFiles(totalPath);
    }

    private Sprite LoadImages(string filePath)
    {
        var bytes = System.IO.File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);
        //photoGraphic.material.SetTexture("_MainTex", texture);
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2());
        //WWW www = new WWW(filePath);
        //yield return www;
        //Texture2D texTmp = new Texture2D(1024, 1024, TextureFormat.DXT1, false);
        ////LoadImageIntoTexture compresses JPGs by DXT1 and PNGs by DXT5     
        //www.LoadImageIntoTexture(texTmp);
        ////photoGraphic.material.SetTexture("_MainTex", texTmp);
    }

    public void SaveButtonPressed()
    {
        Texture2D texture = 

        byte[] _bytes = _texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(_fullPath, _bytes);
        Debug.Log(_bytes.Length / 1024 + "Kb was saved as: " + _fullPath)
    }

    public void NextBtnPressed()
    {
        picIndex = (picIndex + 1) % fileNamesInFolder.Length;
        displayImg.sprite = LoadImages(fileNamesInFolder[picIndex]);
    }

    public void PrevButtonPressed()
    {
        picIndex = (picIndex - 1) % fileNamesInFolder.Length;
        displayImg.sprite = LoadImages(fileNamesInFolder[picIndex]);
    }
}
