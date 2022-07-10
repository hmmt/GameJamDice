using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemDiceSummary : MonoBehaviour
{
    [SerializeField] List<IconViewer> behaviourIconList;
    [SerializeField] List<IconViewer> actingPowerIconList;

    Action<SessionDeck> onClickReleaseBehaviour, onClickReleaseActingPower;
    SessionDeck deck;

    public UIItemDiceSummary SetActionOnClickReleaseBehaviour(Action<SessionDeck> callback)
    {
        onClickReleaseBehaviour = callback;
        return this;
    }

    public UIItemDiceSummary SetActionOnClickReleaseActingPower(Action<SessionDeck> callback)
    {
        onClickReleaseActingPower = callback;
        return this;
    }

    public UIItemDiceSummary SetDeck(SessionDeck deck)
    {
        this.deck = deck;
        return this;
    }

    public UIItemDiceSummary SetBehaviours(List<BehaviourState> behaviourList)
    {
        var count = behaviourIconList.Count;
        for (int i = 0; i < count; i++)
        {
            var enable = behaviourList.Count > 0;
            var behaviourIcon = behaviourIconList[i];
            if (enable)
            {
                var behaviourState = behaviourList[i];
                var sprite = SpriteManager.instance.GetBevaiourIconSprite((int)behaviourState);
                behaviourIcon.SetSpriteIcon(sprite);
            }
            else
            {
                behaviourIcon.SetActive(false);
            }
        }
        return this;
    }

    public UIItemDiceSummary SetActingPowers(List<int> actingPowerList)
    {
        var count = actingPowerIconList.Count;
        for (int i = 0; i < count; i++)
        {
            var enable = actingPowerList.Count > 0;
            var actingPowerIcon = actingPowerIconList[i];

            if (enable)
            {
                var actingPower = actingPowerList[i];
                var sprite = SpriteManager.instance.GetActingPowerIconSprite(actingPower);
                actingPowerIcon.SetSpriteIcon(sprite);
            }
            else
            {
                actingPowerIcon.SetActive(false);
            }
        }
        return this;
    }

    public void Clear()
    {
        behaviourIconList.ForEach(iconViewer => iconViewer.SetActive(false));
        actingPowerIconList.ForEach(iconViewer => iconViewer.SetActive(false));
    }

    public void OnClickReleaseBehaviour()
    {
        onClickReleaseBehaviour?.Invoke(deck);
    }

    public void OnClickReleaseActingPower()
    {
        onClickReleaseActingPower?.Invoke(deck);
    }
}
