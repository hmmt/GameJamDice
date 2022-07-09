using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpriteRendererAnimator : MonoBehaviour
{
    [SerializeField] string animationName;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] List<Sprite> spriteList;
    [SerializeField] float delay;
    [SerializeField] bool isInfinite;
    [SerializeField] bool playOnAwake, playOnEnable;

    public bool isPlaying => (bool)spritePlayingChecker?.isPlaying;
    Coroutine coroutine;
    public string rendererAnimationName => animationName;
    readonly SpriteAnimatorPlayChecker spritePlayingChecker = new SpriteAnimatorPlayChecker();

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

    public void SetSpriteList(IEnumerable<Sprite> sprites)
    {
        spriteList = sprites as List<Sprite>;
    }

    public void PlayAnim(Action callback = null)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(spriteRenderer.AnimateSpriteRenderer(spriteList, 
                                                                        delay, 
                                                                        spritePlayingChecker, 
                                                                        isInfinite,
                                                                        callback));
    }
    public void PlayAnim(Vector2 position, Action callback = null)
    {
        transform.position = position;
        PlayAnim(callback);
    }

    public void StopAnim()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
    }

    public void SetActive(bool enable)
    {
        gameObject.SetActive(enable);
    }
}
