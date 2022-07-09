using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconViewer : MonoBehaviour
{
    [SerializeField] Image imgIcon;

    public IconViewer SetActive(bool enable)
    {
        imgIcon.gameObject.SetActive(enable);
        return this;
    }

    public IconViewer SetSpriteIcon(Sprite sprite)
    {
        imgIcon.SetSprite(sprite);
        return this;
    }
}
