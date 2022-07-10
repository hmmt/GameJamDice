using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemDiceViewer : MonoBehaviour
{
    [SerializeField] List<IconViewer> iconViewerList;

    int type, index;
    Action<int, int> onClickAdd;

    public UIItemDiceViewer SetType(int type)
    {
        this.type = type;
        return this;
    }

    public UIItemDiceViewer SetIndex(int index)
    {
        this.index = index;
        return this;
    }

    public UIItemDiceViewer SetIconViewer(Action<List<IconViewer>> callback)
    {
        callback?.Invoke(iconViewerList);
        return this;
    }

    public UIItemDiceViewer SetActionOnClick(Action<int, int> callback)
    {
        onClickAdd = callback;
        return this;
    }

    public void OnClickAddIcon()
    {
        onClickAdd?.Invoke(type, index);
    }
}
