using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using System.Diagnostics;


/*
  For Mom: Video of errors: File already open(orginal or editted), Multiple instances, Permissions 
  
 */

public class MainScript : MonoBehaviour
{
    public static string mainFolderLocation = "C:/Users/mgermain/Pictures/TestPictures/";

    public GameObject previewSelectablePrefab;

    public HorizontalLayoutGroup picturePreviewsParent;
    public Button nextBtn;
    public Button prevBtn;
    public Button submitBtn;
    public Dropdown selectFolderLocation;

    public Image selectedPicture;
    public InputField annotationField;
    public InputField fontSize;

    List<PictureSelectScript> selectablePicturesList;

    int currentlySelectedIndex;
    string currentImagePathSelected => selectablePicturesList[currentlySelectedIndex].filePath;
    string folderNameSelected;
    Coroutine runningCR;
    bool crIsRunning;
    public void Awake()
    {
        selectablePicturesList = new List<PictureSelectScript>();
        //fill folderSelection dropdown
        FillSelectFolderDropdown();
    }

    private void FillSelectFolderDropdown()
    {
        selectFolderLocation.ClearOptions();
        string[] folderNames = Directory.GetDirectories(mainFolderLocation);
        selectFolderLocation.AddOptions(folderNames.ToList());
    }

    private void ExecuteIMCode()
    {
        string targetFileName = Path.GetFileName(currentImagePathSelected);
        string outputFileName = Path.GetFileName(currentImagePathSelected);
        string folderPath = Path.Combine(mainFolderLocation, selectFolderLocation.options[selectFolderLocation.value].text).Replace("/","\\");
        string outputFolderPath = Path.Combine(mainFolderLocation, selectFolderLocation.options[selectFolderLocation.value].text, "Labelled\\");
        string outputForOriginalFilePath = Path.Combine(mainFolderLocation, selectFolderLocation.options[selectFolderLocation.value].text, "Processed\\");
        string captionText = annotationField.text;
        captionText = captionText + '\n';

        if(!Directory.Exists(outputFolderPath))
        {
            Directory.CreateDirectory(outputFolderPath);
        }

        if(!Directory.Exists(outputForOriginalFilePath))
        {
            Directory.CreateDirectory(outputForOriginalFilePath);
        }

        string arguments = $" convert {folderPath}\\{targetFileName} -font Times-New-Roman -pointsize {fontSize.text} -background white -fill black label:\"{captionText}\" -gravity Center -append {outputFolderPath}{outputFileName}";
        //convert d1.png -font Times-New-Roman -pointsize 24 -background white -fill black label:"This is a very long caption line." -gravity center -append d2.png
       
        // cd C:\Users\mgermain\Pictures\TestPictures\CatFolder\


        var startInfo = new ProcessStartInfo
        {
            Arguments = arguments,
            FileName = @"C:/Program Files/ImageMagick/convert.exe"
        };
        Process.Start(startInfo).WaitForExit();
        
        if(File.Exists(outputForOriginalFilePath + outputFileName))
        {
            File.Delete(outputForOriginalFilePath + outputFileName);
        }


        File.Move(currentImagePathSelected, outputForOriginalFilePath + outputFileName);
        GameObject.Destroy(selectablePicturesList[currentlySelectedIndex].gameObject);
        selectablePicturesList.RemoveAt(currentlySelectedIndex);
        SelectPreviousImage();
    }

    public void SubmitButtonPressed()
    {
        if(selectablePicturesList.Count > 0 && currentlySelectedIndex >= 0)
            ExecuteIMCode();
        //Run ImageMagik
        //move files to correct folders
    }

    public void OpenFolderLocation()
    {
        ShowExplorer();
    }

    private void ShowExplorer()
    {
        string itemPath = "";
        if(selectablePicturesList.Count > 0)
        {
            itemPath = selectablePicturesList[0].filePath;
        }
        else if(!string.IsNullOrEmpty(folderNameSelected))
        {
            itemPath = folderNameSelected;
        }
        else
        {
            return;
        }
        itemPath = itemPath.Replace(@"/", @"\");   // explorer doesn't like front slashes
        System.Diagnostics.Process.Start("explorer.exe", "/select," + itemPath);
    }

    private void SelectImageByName(string imgName)
    {
        SelectImageIndex(GetImageIndexByName(imgName));
    }

    private int GetImageIndexByName(string filePath)
    {
        for(int i = 0; i < selectablePicturesList.Count; i++)
        {
            if (selectablePicturesList[i].filePath == filePath)
                return i;
        }
        return -1;
    }

    private void SelectImageIndex(int indexSelected)
    {
        if (indexSelected >= selectablePicturesList.Count)
            return;

        currentlySelectedIndex = indexSelected;
        string path = selectablePicturesList[indexSelected].filePath;

        var bytes = System.IO.File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);
        //photoGraphic.material.SetTexture("_MainTex", texture);
        selectedPicture.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2());
    }

    public void SelectNextImage()
    {
        if (selectablePicturesList.Count > 0)
        {
            SelectImageIndex((currentlySelectedIndex + 1) % selectablePicturesList.Count);
        }
    }

    public void SelectPreviousImage()
    {
        if (selectablePicturesList.Count > 0)
        {
            int newIndex = (currentlySelectedIndex - 1) % selectablePicturesList.Count;
            newIndex = (newIndex < 0) ? selectablePicturesList.Count - 1 : newIndex;
            SelectImageIndex(newIndex);
        }
    }

    public void FolderSelected(int newIndex)
    {
        if(crIsRunning)
        {
            StopCoroutine(runningCR);
            crIsRunning = false;
        }

        folderNameSelected = selectFolderLocation.options[newIndex].text;
        foreach (Transform t in picturePreviewsParent.transform)
        {
            GameObject.Destroy(t.gameObject);
        }
        
        runningCR = StartCoroutine(CreateAllPictureSelectables());

        SelectImageIndex(0);
    }

    public IEnumerator CreateAllPictureSelectables()
    {
        const int maxPerLoop = 16;
        crIsRunning = true;
        selectablePicturesList = new List<PictureSelectScript>();

        string[] allFilesInNewFolder = GetAllFilesAtFolder(folderNameSelected);

        for (int i = 0; i < allFilesInNewFolder.Length; i++)
        {
            if (i % maxPerLoop == 0 && i != 0)
                yield return null;
            else
            {
                selectablePicturesList.Add(CreatePreviewSelectionPicture(allFilesInNewFolder[i]));
            }
        }
        //Resources.UnloadUnusedAssets();
        crIsRunning = false;
    }

    private string[] GetAllFilesAtFolder(string folderName)
    {
        string totalPath = folderName;
        return Directory.GetFiles(totalPath);
    }

    private PictureSelectScript CreatePreviewSelectionPicture(string imgPath)
    {
        PictureSelectScript toReturn = GameObject.Instantiate(previewSelectablePrefab, picturePreviewsParent.transform).GetComponent<PictureSelectScript>();
        toReturn.Initialize(imgPath, SelectImageByName);
        return toReturn;
    }
}
