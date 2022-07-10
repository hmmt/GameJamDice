using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ImageAnimator : MonoBehaviour
{
    [SerializeField] Image imgAnim;
    [SerializeField] float delay;
    [SerializeField] List<Sprite> spriteList;
    [SerializeField] bool isInfinite, playOnAwake, playOnEnable;

    Coroutine coroutine;

    private void Awake()
    {
        if (playOnAwake)
            PlayAnim();
    }

    private void OnEnable()
    {
        if (playOnEnable)
            PlayAnim();
    }

    public void SetSpriteList(List<Sprite> spriteList)
    {
        this.spriteList = spriteList;
    }

    public void PlayAnim(Action callback = null)
    {
        if (!gameObject.activeInHierarchy)
            return;
        SetAlpha(1f);
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(imgAnim.AnimateImage(spriteList, delay, isInfinite, callback));
    }

    public void PlayAnim(bool isInfinite, Action callback = null)
    {
        SetAlpha(1f);
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(imgAnim.AnimateImage(spriteList, delay, isInfinite, callback));
    }

    public void StopAnim()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
    }

    public void SetAlpha(float alpha)
    {
        imgAnim.SetColor(imgAnim.color.CopyColor(a: alpha));
    }

    public void SetColor(Color32 color)
    {
        imgAnim.SetColor(color);
    }

    public void SetSprite(Sprite sprite)
    {
        imgAnim.sprite = sprite;
    }

    private void OnDisable()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
    }
}
