using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

using Random = UnityEngine.Random;

public class HUDDamageEffect : MonoBehaviour
{
    [SerializeField] Transform tfBody;
    [SerializeField] TextMeshPro txtMeshPro;

    int cachedDamage;

    public HUDDamageEffect SetDamage(int damage)
    {
        cachedDamage = damage;
        return this;
    }
    public HUDDamageEffect SetText(string text)
    {
        txtMeshPro.SetText(text);
        return this;
    }

    public HUDDamageEffect SetPosition(Vector3 position)
    {
        tfBody.position = position;
        return this;
    }

    public void PlayEffect(Action onComplete)
    {
        var startPos = tfBody.position;
        var isDamageOver10 = cachedDamage >= 10;
        var range = isDamageOver10 ? 1.5f : 1.25f;
        var power = isDamageOver10 ? 1.75f : 1.5f;

        var directionFactor = Random.value >= 0.5f ? 1f : -1f;
        var yPosFactor = isDamageOver10 ? 0.5f : 0.25f;

        tfBody.localScale = isDamageOver10 ? new Vector2(1.2f, 1.2f) : new Vector2(1f, 1f);

        var endPos = new Vector3(Random.Range((range - 0.5f) * directionFactor, range) , startPos.y - yPosFactor) + startPos;
        var targetPower = Random.Range(power - 0.5f, power);
        tfBody.DOKill();
        tfBody.DOJump(endPos, targetPower, 1, 0.5f).SetEase(Ease.OutFlash).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    private void OnDestroy()
    {
        tfBody.DOKill();
    }
}
