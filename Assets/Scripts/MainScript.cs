using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    Dictionary<PageType, IPage> pageDict;
    public Transform parentRoot;

    public void Awake()
    {
        pageDict = new Dictionary<PageType, IPage>();
        IPage[] allPages = parentRoot.GetComponentsInChildren<IPage>(true);
        foreach(IPage ip in allPages)
        {
            pageDict.Add(ip.pageType, ip);
        }

        pageDict.ForEach(kv => kv.Value.ProgramLoad());
        pageDict[PageType.PicSelectionPage].PageOpened();

    }

    public void ChangePage(int pageType)
    {
        pageDict.ForEach(kv => { kv.Value.PageClosed(); kv.Value.gameObject.SetActive(false); });
        pageDict[(PageType)pageType].gameObject.SetActive(true);
        pageDict[(PageType)pageType].PageOpened();

    }

    

}
