using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupConfirmation : PopupBase
{
    [SerializeField] List<IconViewer> iconViewerList;
    [SerializeField] Text txtPrice;

    int type, index;
    Action<int, int> onClickConfirm;

    public PopupConfirmation SetType(int type)
    {
        this.type = type;
        return this;
    }

    public PopupConfirmation SetIndex(int index)
    {
        this.index = index;
        return this;
    }

    public PopupConfirmation SetIconViewerList(Action<List<IconViewer>> callback)
    {
        callback?.Invoke(iconViewerList);
        return this;
    }

    public PopupConfirmation SetTextPrice(string text)
    {
        txtPrice.SetText(text);
        return this;
    }

    public PopupConfirmation SetActionOnClickConfirm(Action<int, int> callback)
    {
        onClickConfirm = callback;
        return this;
    }

    public override void Open()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

    public void OnClickCancel()
    {
        Close();
    }

    public void OnClickConfirm()
    {
        onClickConfirm?.Invoke(type, index);
        Close();
    }
}
