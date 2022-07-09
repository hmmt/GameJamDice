using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class CanvasScalerSetter : MonoBehaviour
{
    public static Vector2 screenReferenceResolution { get; private set; }
    public static Vector2 rootSize { get; private set; }
    public static float ratio { get; private set; }

    CanvasScaler scaler;
    RectTransform rectTransform;
    // Start is called before the first frame update
    void Awake()
    {
        scaler = GetComponent<CanvasScaler>();
        ratio = (float)Screen.height / Screen.width;
        if (ratio >= 1.9f)
            screenReferenceResolution = new Vector2(720f, 1560f);
        else if (ratio >= 1.7f)
            screenReferenceResolution = new Vector2(720f, 1280f);
        else
            screenReferenceResolution = new Vector2(960f, 1280f);

        rectTransform = GetComponent<RectTransform>();
        SetResolution(screenReferenceResolution); 
    }

    public void SetResolution(Vector2 resolution)
    {
        scaler.referenceResolution = resolution;
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        Canvas.ForceUpdateCanvases();
        rootSize = rectTransform.sizeDelta;
    }
}
