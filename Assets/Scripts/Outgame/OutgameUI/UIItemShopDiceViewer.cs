using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemShopDiceViewer : MonoBehaviour
{
    [SerializeField] List<IconViewer> iconViewerList;
    [SerializeField] Text txtPrice;
    [SerializeField] GameObject goDimedCover;

    int type, index;
    Action<int, int> onClickItem;

    public UIItemShopDiceViewer SetDimed(bool enable)
    {
        goDimedCover.SetActive(enable);
        return this;
    }

    public UIItemShopDiceViewer SetType(int type)
    {
        this.type = type;
        return this;
    }

    public UIItemShopDiceViewer SetIndex(int index)
    {
        this.index = index;
        return this;
    }

    public UIItemShopDiceViewer SetIconViewer(Action<List<IconViewer>> callback)
    {
        callback?.Invoke(iconViewerList);
        return this;
    }

    public UIItemShopDiceViewer SetTextPrice(string text)
    {
        txtPrice.SetText(text);
        return this;
    }

    public UIItemShopDiceViewer SetActionOnClickItem(Action<int, int> callback)
    {
        onClickItem = callback;
        return this;
    }

    public void OnClickItem()
    {
        onClickItem?.Invoke(type, index);
    }
}
