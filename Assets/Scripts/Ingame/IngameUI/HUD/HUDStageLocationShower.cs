using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDStageLocationShower : MonoBehaviour
{
    [SerializeField] List<HUDMapLocator> mapLocatorList;
    [SerializeField] RectTransform rtTarget;

    int currentStage;

    public HUDStageLocationShower SetCurrentStage(int currentStage)
    {
        this.currentStage = currentStage;
        return this;
    }

    public void Init()
    {
        StartCoroutine(InitNextFrame());
    }
    IEnumerator InitNextFrame()
    {
        yield return null;
        var endPos = mapLocatorList[currentStage].anchoredPosition + new Vector2(0f, 30f);
        rtTarget.anchoredPosition = endPos;
    }

    public void MoveToTargetStage(int stage, float duration)
    {
        StartCoroutine(CoMoveToTarget(stage, duration));
    }

    private IEnumerator CoMoveToTarget(int stage, float duration)
    {
        var startPos = mapLocatorList[currentStage].anchoredPosition + new Vector2(0f, 30f);
        var endPos = mapLocatorList[stage].anchoredPosition + new Vector2(0f, 30f);
        var t = 0f;

        rtTarget.anchoredPosition = startPos;

        while (t <= duration)
        {
            t += Time.deltaTime;
            rtTarget.anchoredPosition = Vector2.Lerp(startPos, endPos, t / duration);
            yield return null;
        }

    }
}
