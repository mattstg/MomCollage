using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;


public class PictureSelectionPage : MonoBehaviour, IPage
{
    public static string[] SelectedImagePaths;

    public Dropdown selectFolderLocation;
    public GridLayoutGroup picLayoutGroup;
    string folderNameSelected;
    public GameObject selectPicPrefab;
    List<PictureSelectScript> pictureSelectScripts = new List<PictureSelectScript>();
    public PageType pageType => PageType.PicSelectionPage;
    public void PageClosed()
    {
    }

    public void PageOpened()
    {
    }

    public void PicturesSelectedSubmitted()
    {
        List<string> pssSelected = new List<string>();
        foreach (PictureSelectScript pss in pictureSelectScripts)
        {
            if (pss.wasSelected)
                pssSelected.Add(pss.filePath);
        }

        SelectedImagePaths = pssSelected.ToArray();
    }

    public void ProgramLoad()
    {
        selectFolderLocation.ClearOptions();
        string[] folderNames = Directory.GetDirectories(GV.mainFolderLocation);
        selectFolderLocation.AddOptions(folderNames.ToList());
    }

    public void FolderOptionSelected(int newIndex)
    {
        folderNameSelected = selectFolderLocation.options[newIndex].text;
        foreach(Transform t in picLayoutGroup.transform)
        {
            GameObject.Destroy(t.gameObject);
        }

        StartCoroutine(CreateAllPictureSelectables());
        
    }

    public IEnumerator CreateAllPictureSelectables()
    {
        const int maxPerLoop = 12;
        
        string[] allFilesInNewFolder = GetAllFilesAtFolder(folderNameSelected);
        pictureSelectScripts.Clear();


        for(int i = 0; i < allFilesInNewFolder.Length; i++)
        {
            if (i % maxPerLoop == 0 && i != 0)
                yield return null;
            else
                pictureSelectScripts.Add(CreatePictureSelection(allFilesInNewFolder[i]));
        }
    }

    private PictureSelectScript CreatePictureSelection(string imgPath)
    {
        PictureSelectScript toReturn = GameObject.Instantiate(selectPicPrefab, picLayoutGroup.transform).GetComponent<PictureSelectScript>();
        toReturn.Initialize(imgPath);
        return toReturn;
    }

    private string[] GetAllFilesAtFolder(string folderName)
    {
        string totalPath = folderName;
        return Directory.GetFiles(totalPath);
    }
}
