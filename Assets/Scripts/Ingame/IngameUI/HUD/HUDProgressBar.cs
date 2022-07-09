using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

[ExecuteInEditMode]
public class HUDProgressBar : MonoBehaviour
{
    [SerializeField] SpriteRenderer bg, progressBar;

    [SerializeField] float maxValue;
    [Range(0f, 1f)] [SerializeField] float value;
    
    float currentValue;
    Action<float> onValueChangeProgress;
    IDisposable disposable;

    private void OnEnable()
    {
        disposable = this.ObserveEveryValueChanged(progressBar => progressBar.value).Subscribe(value =>
        {
            currentValue = maxValue * value;
            CalculateProgress();
        }).AddTo(this);
    }

    public HUDProgressBar SetBgColor(Color32 color)
    {
        bg.color = color;
        return this;
    }

    public HUDProgressBar SetProgressBgColor(Color32 color)
    {
        progressBar.color = color;
        return this;
    }

    public HUDProgressBar SetMaxValue(float value)
    {
        maxValue = value;
        return this;
    }

    public HUDProgressBar SetCurrentValue(float value)
    {
        currentValue = value;
        return this;
    }

    public HUDProgressBar SetActionOnValueChangeProgress(Action<float> callback)
    {
        onValueChangeProgress = callback;
        return this;
    }

    public void CalculateProgress()
    {
        var progress = currentValue / maxValue;
        var clampedValue = progress.Clamp01Value();
        progressBar.size = progressBar.size.SetX(progress);
        value = progress;
        onValueChangeProgress?.Invoke(progress);
    }

    private void OnDisable()
    {
        disposable.Dispose();
    }
}
