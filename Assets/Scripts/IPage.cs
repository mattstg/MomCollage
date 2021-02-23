using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPage
{
    void ProgramLoad();
    void PageOpened();
    void PageClosed();

    PageType pageType { get; }
    GameObject gameObject { get; }
}
