using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDMapLocator : MonoBehaviour
{
    [SerializeField] RectTransform rtBody;

    public Vector2 anchoredPosition => rtBody.anchoredPosition;

    public HUDMapLocator SetScale(Vector2 scale)
    {
        rtBody.localScale = scale;
        return this;
    }
}
