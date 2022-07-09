using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemDiceViewer : MonoBehaviour
{
    [SerializeField] List<IconViewer> iconViewerList;

    public UIItemDiceViewer SetIconViewer(Action<List<IconViewer>> callback)
    {
        callback?.Invoke(iconViewerList);
        return this;
    }
}
