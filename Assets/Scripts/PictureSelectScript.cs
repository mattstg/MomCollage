using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Diagnostics;

public class PictureSelectScript : MonoBehaviour
{
    public Image photoGraphic;
    public Image imgToColor;
    public Text fileNameText;
    public string filePath;
    // Start is called before the first frame update

    PictureSelectedDelg selectionDelg;

    public void Initialize(string _filePath, PictureSelectedDelg _selectionDelg)
    {
        selectionDelg = _selectionDelg;
        filePath = _filePath;
        LoadImages(filePath);
        fileNameText.text = Path.GetFileNameWithoutExtension(filePath);
        //StartCoroutine(LoadImages(filePath));
    }

    public void ButtonPressed()
    {
        selectionDelg.Invoke(filePath);
    }

    private void LoadImages(string filePath)
    {
        var bytes = System.IO.File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);
        //photoGraphic.material.SetTexture("_MainTex", texture);
        photoGraphic.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),new Vector2());
        //WWW www = new WWW(filePath);
        //yield return www;
        //Texture2D texTmp = new Texture2D(1024, 1024, TextureFormat.DXT1, false);
        ////LoadImageIntoTexture compresses JPGs by DXT1 and PNGs by DXT5     
        //www.LoadImageIntoTexture(texTmp);
        ////photoGraphic.material.SetTexture("_MainTex", texTmp);
    }

    private void LoadImg()
    {

    }

}
