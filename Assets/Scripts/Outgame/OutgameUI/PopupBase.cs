using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PopupBase : MonoBehaviour
{
    public bool isOpened { get; protected set; }
    protected Action onOpen, onClose;

    public void SetActionOnOpen(Action callback) => onOpen = callback;
    public void SetActionOnClose(Action callback) => onClose = callback;

    public abstract void Open();
    public abstract void Close();

    private void OnDisable()
    {
        onOpen = null;
        onClose = null;
    }
}
